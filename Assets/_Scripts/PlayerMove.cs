using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Vector2 playerVelocity = Vector2.zero;

    [SerializeField] InputSO Input;
    [SerializeField] Transform startPositionTransform;
    [SerializeField] float maxSpeed = 4;
    [SerializeField] float timeToMaxSpeed = 0.3f;
    [SerializeField] float maxTurningSpeed = 180;
    [SerializeField] float timeToMaxTurning = 0.1f;
    [SerializeField] AnimationCurve turningSpeedCurve;
    [SerializeField] float timeUntilStop = 4;
    [SerializeField] float timeToStopRotation = 2;
    [SerializeField] float acceleratingXDecelerationMod = 3;
    [SerializeField] float acceleratingRotationMod = 2;

    public Vector2 PlayerVelocity { get { return playerVelocity; } }
    public float MaxSpeed { get { return maxSpeed; } }

    PlayerUpgradesManager upgradesManager;
    Rigidbody2D rb;
    float acceleration = 0;
    float deceleration = 0;
    float AngularAccel = 0;
    float AngDeceleration = 0;
    float turningTime = 0;

    ContactPoint2D[] contacts = new ContactPoint2D[10];

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        upgradesManager = FindObjectOfType<PlayerUpgradesManager>();
        transform.position = startPositionTransform.position;
        UpdateValues();
    }

    float velocityFraction;
    void FixedUpdate()
    {
        UpdateValues();
        Vector2 velocity = transform.InverseTransformDirection(rb.velocity);

        //Acceleration
        if (Input.Acceleration >= float.Epsilon)
            velocity.y = Mathf.Clamp(velocity.y + acceleration * Time.fixedDeltaTime, -maxSpeed, maxSpeed);
        else if (Input.Acceleration <= -float.Epsilon)
            velocity.y = Mathf.Clamp(velocity.y + acceleration * -0.5f * Time.fixedDeltaTime, -maxSpeed, maxSpeed);
       
        //Deceleration
        else
        {
            //Em X
            if (velocity.x >= float.Epsilon)
            {
                if (Input.Acceleration != 0  && rb.GetContacts(contacts) == 0)                
                    velocity.x = Mathf.Clamp(velocity.x - deceleration * acceleratingXDecelerationMod * Time.fixedDeltaTime, 0, maxSpeed);                
                else 
                    velocity.x = Mathf.Clamp(velocity.x - deceleration * Time.fixedDeltaTime, 0, maxSpeed);
            }
            else if (velocity.x <= float.Epsilon)
            {
                if (Input.Acceleration != 0  && rb.GetContacts(contacts) == 0)
                    velocity.x = Mathf.Clamp(velocity.x + deceleration * acceleratingXDecelerationMod * Time.fixedDeltaTime, -maxSpeed, 0);
                else 
                    velocity.x = Mathf.Clamp(velocity.x + deceleration * Time.fixedDeltaTime, -maxSpeed, 0);
            }

            //Em Y
            if (velocity.y >= float.Epsilon)
                velocity.y = Mathf.Clamp(velocity.y - deceleration * Time.fixedDeltaTime, 0, maxSpeed);
            else if (velocity.y <= float.Epsilon)
                velocity.y = Mathf.Clamp(velocity.y + deceleration * Time.fixedDeltaTime, -maxSpeed, 0);
        }

        //Clamp Speed when turning
        velocityFraction = velocity.magnitude / maxSpeed;
        if (velocityFraction > 1)
        {
            velocity /= velocityFraction;
        }

        //Apply Velocity
        rb.velocity = transform.TransformDirection(velocity);
        playerVelocity = rb.velocity;

        ///////////////////////////////////////////////////////////////////////////

        //Angular Velocity

        if (Mathf.Abs(Input.Turning) >= float.Epsilon)
        {
            turningTime += Time.fixedDeltaTime;
            float turningValue = turningTime / timeToMaxTurning;
            turningValue = Mathf.Clamp(turningValue, 0, 1);
            //Debug.Log(turningSpeedCurve.Evaluate(turningValue));

            rb.angularVelocity -= AngularAccel * turningSpeedCurve.Evaluate(turningValue) * Time.fixedDeltaTime * Mathf.Sign(Input.Turning);            
        }         

        else //Angular Deceleration, greater when accelerating
        {
            turningTime = 0;

            float l_decelFactor = AngDeceleration * acceleratingRotationMod * Time.fixedDeltaTime;
            if (Input.Acceleration != 0)
                l_decelFactor *= acceleratingRotationMod;

            if (rb.angularVelocity >= float.Epsilon)
                rb.angularVelocity = Mathf.Clamp(rb.angularVelocity - l_decelFactor, 0, maxTurningSpeed);
            else if (rb.angularVelocity <= float.Epsilon)
                rb.angularVelocity = Mathf.Clamp(rb.angularVelocity + l_decelFactor, -maxTurningSpeed, 0);
        }

        rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, -maxTurningSpeed, maxTurningSpeed);
    }

    void UpdateValues()
    {
        maxSpeed = upgradesManager.ShipUpgradesInfo.SpeedUpgrade[upgradesManager.CurrentUpgrades.ShipUpgrades.SpeedLevel - 1].Speed;
        maxTurningSpeed = upgradesManager.ShipUpgradesInfo.ManobrabilityUpgrade[upgradesManager.CurrentUpgrades.ShipUpgrades.ManobrabilityLevel - 1].TurningSpeed;
        timeUntilStop = upgradesManager.ShipUpgradesInfo.ManobrabilityUpgrade[upgradesManager.CurrentUpgrades.ShipUpgrades.ManobrabilityLevel - 1].TimeToStop;
        timeToStopRotation = upgradesManager.ShipUpgradesInfo.ManobrabilityUpgrade[upgradesManager.CurrentUpgrades.ShipUpgrades.ManobrabilityLevel - 1].TimeToStopRotating;

        acceleration = maxSpeed / timeToMaxSpeed;
        AngularAccel = maxTurningSpeed / timeToMaxTurning;
        deceleration = maxSpeed / timeUntilStop;
        AngDeceleration = maxTurningSpeed / timeToStopRotation;
    }

}