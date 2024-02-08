using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using DG.Tweening;

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
    [SerializeField] AnimationCurve stoppingDirInputCurve;

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

        //Debug.Log(Input.Acceleration);
        //Acceleration
        if (Input.Acceleration >= float.Epsilon)
            velocity.y = Mathf.Clamp(velocity.y + acceleration * Time.fixedDeltaTime * Input.Acceleration, -maxSpeed, maxSpeed);
        else if (Input.Acceleration <= -float.Epsilon)
            velocity.y = Mathf.Clamp(velocity.y + acceleration * 0.5f * Time.fixedDeltaTime * Input.Acceleration, -maxSpeed, maxSpeed);
        
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

        //Debug.Log("Turning: " + Input.Turning);
        if (Mathf.Abs(Input.Turning) >= float.Epsilon)
        {
            turningTime += Time.fixedDeltaTime;
            float turningValue = turningTime / timeToMaxTurning;
            turningValue = Mathf.Clamp(turningValue, 0, 1);

            rb.angularVelocity -= AngularAccel * turningSpeedCurve.Evaluate(turningValue) * Time.fixedDeltaTime * Input.Turning;      
            
            // Apply deceleration when turning in the oposite direction, to turn faster
            if(Mathf.Abs(rb.angularVelocity) > 0 && Mathf.Sign(rb.angularVelocity) == Mathf.Sign(Input.Turning))
            {
                rb.angularVelocity -= AngDeceleration * Time.fixedDeltaTime * Input.Turning;
            }
        }

        else //Angular Deceleration, greater when accelerating
        {
            turningTime = 0;

            float l_decelFactor = AngDeceleration * Time.fixedDeltaTime;
            if (Input.Acceleration != 0)
                l_decelFactor *= acceleratingRotationMod;

            if (rb.angularVelocity >= float.Epsilon)
                rb.angularVelocity = Mathf.Clamp(rb.angularVelocity - l_decelFactor, 0, maxTurningSpeed);
            else if (rb.angularVelocity <= float.Epsilon)
                rb.angularVelocity = Mathf.Clamp(rb.angularVelocity + l_decelFactor, -maxTurningSpeed, 0);
        }

        DrawDirectionLine();
        TurnToDirection();
        ClampDirection();

        rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, -maxTurningSpeed, maxTurningSpeed);           
    }


    float directionTime = 0;
    Vector2 currentDirection = new();
    float directionDifference = 0;
    float analogAngleToRotateMin = 0;
    void TurnToDirection()
    {
        analogAngleToRotateMin = maxTurningSpeed * 0.75f * Time.fixedDeltaTime;
        currentDirection = new Vector2(Mathf.Sin(-rb.rotation * Mathf.Deg2Rad), Mathf.Cos(-rb.rotation * Mathf.Deg2Rad));
        directionDifference = Vector2.SignedAngle(currentDirection, Input.Direction);

        if (Mathf.Abs(directionDifference) < analogAngleToRotateMin || Input.Direction == Vector2.zero)
        {
            directionTime = 0;
            return;
        };

        float angleToStartDecel = Mathf.Abs(maxTurningSpeed * timeToMaxSpeed);        

        if (Mathf.Abs(directionDifference) > angleToStartDecel)
        {
            directionTime += Time.fixedDeltaTime;
            float turningValue = directionTime / timeToMaxTurning;
            turningValue = Mathf.Clamp(turningValue, 0, 1);

            rb.angularVelocity += AngularAccel * turningSpeedCurve.Evaluate(turningValue) * Time.fixedDeltaTime * Mathf.Sign(directionDifference);
        }
        else if(Mathf.Abs(directionDifference) <= angleToStartDecel)
        {
            float turningValue = 2 * Mathf.Abs(directionDifference) / angleToStartDecel;
            turningValue = Mathf.Clamp(turningValue, 0.2f, 1);
            rb.angularVelocity = maxTurningSpeed * stoppingDirInputCurve.Evaluate(turningValue) * Mathf.Sign(directionDifference);
        }
    }

    void ClampDirection()
    {
        float futureRotation = rb.rotation + rb.angularVelocity * Time.fixedDeltaTime;
        Vector2 futureDirection = new Vector2(Mathf.Sin(-futureRotation * Mathf.Deg2Rad), Mathf.Cos(-futureRotation * Mathf.Deg2Rad));
        if (Mathf.Sign(directionDifference) < Mathf.Sign(analogAngleToRotateMin) && 
            Mathf.Sign(directionDifference) != Mathf.Sign(Vector2.SignedAngle(futureDirection, Input.Direction)))
        {
            rb.MoveRotation(Vector2.SignedAngle(Input.Direction, Vector2.up) * -1);
            rb.angularVelocity = 0;
            directionTime = 0;
            //Debug.Log($"Set rotation: {Vector2.SignedAngle(Input.Direction, Vector2.up)* -1}");
        }
    }

    void DrawDirectionLine()
    {
        Debug.DrawLine(transform.position, transform.position + 5 * (Vector3)Input.Direction);

        Debug.DrawLine(transform.position, (Vector2)transform.position + 3 * new Vector2(Mathf.Sin(-rb.rotation * Mathf.Deg2Rad), Mathf.Cos(-rb.rotation * Mathf.Deg2Rad)));
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