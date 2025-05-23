using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using DG.Tweening;

public class PlayerMove : MonoBehaviour
{
    Vector2 playerVelocity = Vector2.zero;

    //[SerializeField] InputSO Input;
    public Vector3 StartPosition = Vector3.zero;
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
        transform.position = StartPosition;
        UpdateValues();
    }

    float velocityFraction;
    void FixedUpdate()
    {
        //if(GameStatus.IsPortal) return;

        UpdateValues();
        Vector2 velocity = transform.InverseTransformDirection(rb.velocity);

        #region Ymove
        //Acceleration
        if (InputHolder.Instance.Acceleration >= float.Epsilon)
            velocity.y = Mathf.Clamp(velocity.y + acceleration * Time.fixedDeltaTime * InputHolder.Instance.Acceleration, -maxSpeed, maxSpeed);
        else if (InputHolder.Instance.Acceleration <= -float.Epsilon)
            velocity.y = Mathf.Clamp(velocity.y + acceleration * 0.5f * Time.fixedDeltaTime * InputHolder.Instance.Acceleration, -maxSpeed , maxSpeed);        

        //Deceleration
        // Em X acelerando
        if(InputHolder.Instance.Acceleration != 0 && rb.GetContacts(contacts) == 0)
        {
            if (velocity.x >= float.Epsilon)
            {
                velocity.x = Mathf.Clamp(velocity.x - deceleration * acceleratingXDecelerationMod * Time.fixedDeltaTime, 0, maxSpeed);
                //Debug.Log(Input.Acceleration);
            }
            else if(velocity.x <= float.Epsilon)
            {
                velocity.x = Mathf.Clamp(velocity.x + deceleration * acceleratingXDecelerationMod * Time.fixedDeltaTime, -maxSpeed, 0);
                //Debug.Log(Input.Acceleration);
            }
        }

        else // N�o acelerando
        {
            //Em X
            if (velocity.x >= float.Epsilon)
            {               
                velocity.x = Mathf.Clamp(velocity.x - deceleration * Time.fixedDeltaTime, 0, maxSpeed);
            }
            else if (velocity.x <= float.Epsilon)
            {
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

        #endregion

        ///////////////////////////////////////////////////////////////////////////

        #region turning
        //Angular Velocity

        //Debug.Log("Turning: " + Input.Turning);
        if (Mathf.Abs(InputHolder.Instance.Turning) >= float.Epsilon)
        {
            turningTime += Time.fixedDeltaTime;
            float turningValue = turningTime / timeToMaxTurning;
            turningValue = Mathf.Clamp(turningValue, 0, 1);

            rb.angularVelocity -= AngularAccel * turningSpeedCurve.Evaluate(turningValue) * Time.fixedDeltaTime * InputHolder.Instance.Turning;      
            
            // Apply deceleration when turning in the oposite direction, to turn faster
            if(Mathf.Abs(rb.angularVelocity) > 0 && Mathf.Sign(rb.angularVelocity) == Mathf.Sign(InputHolder.Instance.Turning))
            {
                rb.angularVelocity -= AngDeceleration * Time.fixedDeltaTime * InputHolder.Instance.Turning;
            }
        }

        else //Angular Deceleration, greater when accelerating
        {
            turningTime = 0;

            float l_decelFactor = AngDeceleration * Time.fixedDeltaTime;
            if (InputHolder.Instance.Acceleration != 0)
                l_decelFactor *= acceleratingRotationMod;

            if (rb.angularVelocity >= float.Epsilon)
                rb.angularVelocity = Mathf.Clamp(rb.angularVelocity - l_decelFactor, 0, maxTurningSpeed);
            else if (rb.angularVelocity <= float.Epsilon)
                rb.angularVelocity = Mathf.Clamp(rb.angularVelocity + l_decelFactor, -maxTurningSpeed, 0);
        }

        //DrawDirectionLine();
        TurnToDirection();
        ClampDirection();

        rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, -maxTurningSpeed, maxTurningSpeed);           
    }
    #endregion


    float directionTime = 0;
    Vector2 currentDirection = new();
    float directionDifference = 0;
    float analogAngleToRotateMin = 0;
    void TurnToDirection()
    {
        analogAngleToRotateMin = (maxTurningSpeed * Time.fixedDeltaTime) + float.Epsilon;
        currentDirection = new Vector2(Mathf.Sin(-rb.rotation * Mathf.Deg2Rad), Mathf.Cos(-rb.rotation * Mathf.Deg2Rad));
        directionDifference = Vector2.SignedAngle(currentDirection, InputHolder.Instance.Direction);

        if (Mathf.Abs(directionDifference) < analogAngleToRotateMin || InputHolder.Instance.Direction == Vector2.zero)
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
        if (Mathf.Abs(directionDifference) <= Mathf.Abs(analogAngleToRotateMin) && 
            Mathf.Sign(directionDifference) != Mathf.Sign(Vector2.SignedAngle(futureDirection, InputHolder.Instance.Direction)))
        {
            rb.MoveRotation(Vector2.SignedAngle(InputHolder.Instance.Direction, Vector2.up) * -1);
            rb.angularVelocity = 0;
            directionTime = 0;
        }
        //Debug.Log($"Turning: {isTurning}");
    }    
    public int GetTurningDirection()
    {
        bool isTurning = false;
        if (InputHolder.Instance.Direction != Vector2.zero && Mathf.Abs(directionDifference) > Mathf.Abs(analogAngleToRotateMin))
        {
            isTurning = true;
        }
        else
        {
            isTurning= false;
        }

        if(!isTurning) return 0;

        return (int)Mathf.Sign(directionDifference) * -1;
    }

    void UpdateValues()
    {
        if (!GameStatus.IsPortal)
        {
            maxSpeed = PlayerStats.Instance.Ship.CurrentMaxSpeed;
            maxTurningSpeed = PlayerStats.Instance.Ship.CurrentMaxTurningSpeed;
            timeUntilStop = PlayerStats.Instance.Ship.CurrentLinearInertia;
            timeToStopRotation = PlayerStats.Instance.Ship.CurrentAngularInertia;
        }
        else
        {
            maxSpeed = upgradesManager.ShipUpgradesInfo.SpeedUpgrade[upgradesManager.CurrentUpgrades.ShipUpgrades.SpeedLevel - 1].Speed;
            maxTurningSpeed = upgradesManager.ShipUpgradesInfo.ManobrabilityUpgrade[upgradesManager.CurrentUpgrades.ShipUpgrades.ManobrabilityLevel - 1].TurningSpeed;
            timeUntilStop = upgradesManager.ShipUpgradesInfo.ManobrabilityUpgrade[upgradesManager.CurrentUpgrades.ShipUpgrades.ManobrabilityLevel - 1].TimeToStop;
            timeToStopRotation = upgradesManager.ShipUpgradesInfo.ManobrabilityUpgrade[upgradesManager.CurrentUpgrades.ShipUpgrades.ManobrabilityLevel - 1].TimeToStopRotating;
        }

        acceleration = maxSpeed / timeToMaxSpeed;
        AngularAccel = maxTurningSpeed / timeToMaxTurning;
        deceleration = maxSpeed / timeUntilStop;
        AngDeceleration = maxTurningSpeed / timeToStopRotation;        
    }
}