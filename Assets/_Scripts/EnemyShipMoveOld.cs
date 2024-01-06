using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Cinemachine.CinemachineTargetGroup;

public class EnemyShipMoveOld : MonoBehaviour
{
    [SerializeField] float baseSpeed = 4;
    [SerializeField] float baseRotationSpeed = 60;
    [SerializeField] float rotationSpeedVarPerc = 30;
    [SerializeField] float rotationChangeTime = 6f;
    [SerializeField] float rotationChangeTimeVar = 3f;
    [SerializeField] float timeToMaxSpeed = 1;
    [SerializeField] float distanceToKeep = 9;
    [SerializeField] float distanceTolerance = 0.7f;
    [SerializeField] float distanceCheckFreq = 0.5f;

    Transform player;
    float currentSpeed = 0;
    float acceleration = 0;
    Vector3 moveDir = Vector3.zero;
    float maxRotationSpeed = 0;
    float currentRotationSpeed = 0;
    float rotationAcceleration = 0;
    float timeToChangeRotation;
    int rotationMod = 0; // -1, 0 ou 1.
    int lastRotationMod = 0; 

    void Start()
    {       
        player = FindAnyObjectByType<PlayerMove>().transform;

        currentSpeed = baseSpeed;
        acceleration = baseSpeed / timeToMaxSpeed;

        if (maxRotationSpeed == 0)
            maxRotationSpeed = Mathf.Abs(Random.Range(baseRotationSpeed - baseRotationSpeed*(rotationSpeedVarPerc/100), baseRotationSpeed + baseRotationSpeed*(rotationSpeedVarPerc/100)));

        rotationAcceleration = baseRotationSpeed / timeToMaxSpeed;
        currentRotationSpeed = 0;

        StartCoroutine(DistanceCheckFrequency());
        StartCoroutine(RotationCheckFrequency());
        rotationMod = 0;
    }

  
    void Update()
    {
        transform.up = player.position - transform.position;


        if(moveDir == Vector3.zero)
            currentSpeed = 0;
        else
            currentSpeed = Mathf.Clamp(currentSpeed + acceleration * Time.deltaTime, 0, baseSpeed);

        transform.Translate(moveDir.normalized * currentSpeed * Time.deltaTime, Space.World);


        if (rotationMod != lastRotationMod)
            currentRotationSpeed = 0;
        else
            currentRotationSpeed = Mathf.Clamp(currentRotationSpeed + rotationAcceleration * Time.deltaTime, 0, maxRotationSpeed);

        transform.RotateAround(player.position, Vector3.forward, maxRotationSpeed * rotationMod * Time.deltaTime);

        lastRotationMod = rotationMod;
    }

    IEnumerator DistanceCheckFrequency()
    {
        do
        {          
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= distanceToKeep - distanceTolerance)
            {
                moveDir = -(player.position - transform.position);
            }
            else if (distanceToPlayer >= distanceToKeep + distanceTolerance)
            {
                moveDir = (player.position - transform.position);
            }
            else
                moveDir = Vector3.zero;

            yield return new WaitForSeconds(distanceCheckFreq);

        } while (true);
    }

    IEnumerator RotationCheckFrequency()
    {
        do
        {
            float randomRotationState = Random.Range(0, 10);
            if (randomRotationState <= 4)
                rotationMod = -1;
            else if (randomRotationState <= 8)
                rotationMod = 1;
            else
                rotationMod = 0;

            timeToChangeRotation = Random.Range(-rotationChangeTimeVar, rotationChangeTimeVar) + rotationChangeTime;
            if (rotationMod == 0)
                timeToChangeRotation *= 0.5f;
            yield return new WaitForSeconds(timeToChangeRotation);

        } while (true);
    }
}