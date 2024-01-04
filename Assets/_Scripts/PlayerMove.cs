using DG.Tweening;
using System;
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
    [SerializeField] float timeToStopRot = 2;
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

    Tween angularAccelTween;
    Tween angularDecelTween;
    float angVelocity;

    float velocityFraction;
    void FixedUpdate()
    {
        UpdateValues();
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

        //Angular Accel
        
        if (Mathf.Abs(Input.Turning) >= float.Epsilon)
        {
            if (angularDecelTween != null)
            {
                angularDecelTween.Kill();
                angularDecelTween = null;
            }

            if (angularAccelTween == null || angularAccelTween.IsActive() && !angularAccelTween.IsPlaying())
            {   
                angVelocity = rb.angularVelocity;
                angularAccelTween = DOTween.To(() => angVelocity, x => angVelocity = x, maxTurningSpeed * -Mathf.Sign(Input.Turning), maxTurningSpeed / timeToMaxTurning)
                    .SetEase(Ease.InCirc).SetSpeedBased(true);
            }                
        }

        //Angular Deceleration, greater when accelerating
        else if (!(Mathf.Abs(Input.Turning) >= float.Epsilon))
        {
            if (angularAccelTween != null)
            {
                angularAccelTween.Kill();
                angularAccelTween = null;
            }
            if(angularDecelTween == null || angularDecelTween.IsActive() && !angularDecelTween.IsPlaying())
            {
                float l_AngularSpeedToStop = maxTurningSpeed / timeToStopRot;
                if (Mathf.Abs(Input.Acceleration) >= float.Epsilon)
                    l_AngularSpeedToStop *= acceleratingRotationMod;

                angVelocity = rb.angularVelocity;
                angularDecelTween = DOTween.To(() => angVelocity, x => angVelocity = x, 0, l_AngularSpeedToStop)
                    .SetEase(Ease.OutSine).SetSpeedBased(true);
            }            
        }

        angVelocity = Mathf.Clamp(angVelocity, -maxTurningSpeed, maxTurningSpeed);
        rb.angularVelocity = angVelocity;
    }

    void UpdateValues()
    {
        maxSpeed = upgradesManager.ShipUpgradesInfo.SpeedUpgrade[upgradesManager.CurrentUpgrades.ShipUpgrades.SpeedLevel - 1].Speed;
        maxTurningSpeed = upgradesManager.ShipUpgradesInfo.ManobrabilityUpgrade[upgradesManager.CurrentUpgrades.ShipUpgrades.ManobrabilityLevel - 1].TurningSpeed;
        timeUntilStop = upgradesManager.ShipUpgradesInfo.ManobrabilityUpgrade[upgradesManager.CurrentUpgrades.ShipUpgrades.ManobrabilityLevel - 1].TimeToStop;
        timeToStopRot = upgradesManager.ShipUpgradesInfo.ManobrabilityUpgrade[upgradesManager.CurrentUpgrades.ShipUpgrades.ManobrabilityLevel - 1].TimeToStopRotating;

        acceleration = maxSpeed / timeToMaxSpeed;
        deceleration = maxSpeed / timeUntilStop;
    }

}