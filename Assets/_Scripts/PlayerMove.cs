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
    [SerializeField] float timeUntilStop = 4;
    [SerializeField] float timeToStopRotation = 2;
    [SerializeField] float acceleratingXDecelerationMod = 3;
    [SerializeField] float acceleratingRotationMod = 2;

    public Vector2 PlayerVelocity { get { return playerVelocity; } }
    public float MaxSpeed { get { return maxSpeed; } }

    Rigidbody2D rb;
    float acceleration = 0;
    float deceleration = 0;
    float AngularAccel = 0;
    float AngDeceleration = 0;

    ContactPoint2D[] contacts = new ContactPoint2D[10];

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        acceleration = maxSpeed / timeToMaxSpeed;
        AngularAccel = maxTurningSpeed / timeToMaxTurning;
        deceleration = maxSpeed / timeUntilStop;
        AngDeceleration = maxTurningSpeed / timeToStopRotation;
    }

    void Start()
    {
        transform.position = startPositionTransform.position;
    }

    float velocityFraction;
    void FixedUpdate()
    {
        Vector2 velocity = transform.InverseTransformDirection(rb.velocity);              

        //Acceleration
        if (Input.Acceleration >= float.Epsilon)
        {
            velocity.y = Mathf.Clamp(velocity.y + acceleration * Time.fixedDeltaTime, -maxSpeed, maxSpeed);
        }
        else if (Input.Acceleration <= -float.Epsilon)
        {
            velocity.y = Mathf.Clamp(velocity.y + acceleration * -0.5f * Time.fixedDeltaTime, -maxSpeed, maxSpeed);
        }
        else //Deceleration
        {           
            //Em X
            if (velocity.x >= float.Epsilon)
            {
                if (Input.Acceleration != 0  & rb.GetContacts(contacts) == 0)
                {
                    velocity.x = Mathf.Clamp(velocity.x - deceleration * acceleratingXDecelerationMod * Time.fixedDeltaTime, 0, maxSpeed);
                }
                else { velocity.x = Mathf.Clamp(velocity.x - deceleration * Time.fixedDeltaTime, 0, maxSpeed); }
            }
            else if (velocity.x <= float.Epsilon)
            {
                if (Input.Acceleration != 0  & rb.GetContacts(contacts) == 0)
                {
                    velocity.x = Mathf.Clamp(velocity.x + deceleration * acceleratingXDecelerationMod * Time.fixedDeltaTime, -maxSpeed, 0);
                }
                else { velocity.x = Mathf.Clamp(velocity.x + deceleration * Time.fixedDeltaTime, -maxSpeed, 0); }
            }
            //Em Y
            if (velocity.y >= float.Epsilon) { velocity.y = Mathf.Clamp(velocity.y - deceleration * Time.fixedDeltaTime, 0, maxSpeed); }
            else if (velocity.y <= float.Epsilon) { velocity.y = Mathf.Clamp(velocity.y + deceleration * Time.fixedDeltaTime, -maxSpeed, 0); }
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
        if (Input.Turning >= float.Epsilon)
        {
            rb.angularVelocity -= AngularAccel * Time.fixedDeltaTime;
        }
        else if (Input.Turning <= -float.Epsilon)
        {
            rb.angularVelocity += AngularAccel * Time.fixedDeltaTime;
        }
        else //Angular Deceleration, greater when accelerating
        {
            if (rb.angularVelocity >= float.Epsilon)
            {
                if (Input.Acceleration != 0) {
                    rb.angularVelocity = Mathf.Clamp(rb.angularVelocity - AngDeceleration * acceleratingRotationMod * Time.fixedDeltaTime, 0, maxTurningSpeed); }
                else {
                    rb.angularVelocity = Mathf.Clamp(rb.angularVelocity - AngDeceleration * Time.fixedDeltaTime, 0, maxTurningSpeed); }
            }
            if (rb.angularVelocity <= float.Epsilon)
            {
                if (Input.Acceleration != 0) { 
                    rb.angularVelocity = Mathf.Clamp(rb.angularVelocity + AngDeceleration * acceleratingRotationMod * Time.fixedDeltaTime, -maxTurningSpeed, 0); }
                else { 
                    rb.angularVelocity = Mathf.Clamp(rb.angularVelocity + AngDeceleration * Time.fixedDeltaTime, -maxTurningSpeed, 0); }
            }
        }

        rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, -maxTurningSpeed, maxTurningSpeed);
    }


}