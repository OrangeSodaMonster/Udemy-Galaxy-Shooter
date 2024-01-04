using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] InputSO Input;
    [SerializeField] Transform startPositionTransform;
    [SerializeField] float maxSpeed = 4;
    [SerializeField] float timeToMaxSpeed = 0.3f;
    [SerializeField] float timeUntilStop = 4;
    [SerializeField] float maxTurningSpeed = 180;
    [SerializeField] float timeToMaxTurning = 0.1f;
    [SerializeField] float timeToStopRot = 2;
    [SerializeField] float acceleratingXDecelerationMod = 3;
    [SerializeField] float acceleratingRotationMod = 2;

    Vector2 playerVelocity = Vector2.zero;
    public Vector2 PlayerVelocity { get { return playerVelocity; } }
    public float MaxSpeed { get { return maxSpeed; } }

    PlayerUpgradesManager upgradesManager;
    Rigidbody2D rb;
    float acceleration = 0;
    float deceleration = 0;

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
    Tween defaultAngDecelTween;
    Tween yVelocityAngDecelTween;
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
            defaultAngDecelTween?.Kill();
            yVelocityAngDecelTween?.Kill();

            if (!angularAccelTween.IsActive() || angularAccelTween.IsActive() && !angularAccelTween.IsPlaying())
            {   
                angVelocity = rb.angularVelocity;
                angularAccelTween = DOTween.To(() => angVelocity, x => angVelocity = x, maxTurningSpeed * -Mathf.Sign(Input.Turning), maxTurningSpeed / timeToMaxTurning)
                    .SetEase(Ease.InSine).SetSpeedBased(true);
            }                
        }

        //Angular Deceleration, greater when accelerating
        else if (!(Mathf.Abs(Input.Turning) >= float.Epsilon) && Mathf.Abs(rb.angularVelocity) > 0)
        {            
            angularAccelTween?.Kill();            

            // Not Accelerating
            if (!(Mathf.Abs(Input.Acceleration) >= float.Epsilon) && (!defaultAngDecelTween.IsActive() || defaultAngDecelTween.IsActive() && !defaultAngDecelTween.IsPlaying()))
            {
                yVelocityAngDecelTween?.Kill();

                angVelocity = rb.angularVelocity;
                defaultAngDecelTween = DOTween.To(() => angVelocity, x => angVelocity = x, 0, maxTurningSpeed / timeToStopRot)
                    .SetEase(Ease.OutSine).SetSpeedBased(true);
            }
            // Accelerating
            else if ((Mathf.Abs(Input.Acceleration) >= float.Epsilon) && (!yVelocityAngDecelTween.IsActive() || yVelocityAngDecelTween.IsActive() && !yVelocityAngDecelTween.IsPlaying()))
            {
                defaultAngDecelTween?.Kill();

                float l_AngularSpeedToStop = maxTurningSpeed / timeToStopRot;
                l_AngularSpeedToStop *= acceleratingRotationMod;

                angVelocity = rb.angularVelocity;
                yVelocityAngDecelTween = DOTween.To(() => angVelocity, x => angVelocity = x, 0, l_AngularSpeedToStop)
                    .SetEase(Ease.OutSine).SetSpeedBased(true);
            }
        }

        angVelocity = Mathf.Clamp(angVelocity, -maxTurningSpeed, maxTurningSpeed);
        angVelocity = Mathf.Abs(angVelocity) < .5f ? 0 : angVelocity;

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