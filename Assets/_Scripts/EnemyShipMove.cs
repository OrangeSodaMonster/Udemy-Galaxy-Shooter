using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyShipMove : MonoBehaviour
{
    [SerializeField] float distanceToKeep;
    [SerializeField] float distanceToleranceFraction = .1f;
    [SerializeField] float timeToMaxSpeed = 1;
    [SerializeField] float maxXSpeed;
    [SerializeField] float maxYSpeed;

    [SerializeField] bool isRotating;
    [SerializeField] bool rotateClockWise = true;
    [SerializeField] float rotationChangeTime = 6f;
    [SerializeField] float rotationChangeTimeVar = 3f;

    int rotateDirection = 1;
    float currentMaxXSpeed;
    Transform player;
    Rigidbody2D playerRB;
    Rigidbody2D rb;
    Vector2 newVelocity;
    float timeToChangeRotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        distanceToKeep *= distanceToKeep;
        distanceToleranceFraction *= distanceToKeep;
    }

    void OnEnable()
    {
        player = FindObjectOfType<PlayerMove>()?.transform;
        playerRB = player?.GetComponent<Rigidbody2D>();        

        StartCoroutine(RotationCheckFrequency());
    }

    void FixedUpdate()
    {
        if (rotateClockWise) rotateDirection = -1;
        else rotateDirection = 1;        

        if(player != null && !GameStatus.IsGameover)
        {
            RotateAroundPlayer();
        }
        else
        {
            FowardMovement();
        }

        rb.velocity = transform.TransformDirection(newVelocity);
    }

    bool shouldMoveFoward = false;
    bool shouldStop = true;
    void FowardMovement()
    {
        if (shouldStop)
        {
            DOTween.To(() => newVelocity.x, x => newVelocity.x = x, 0, 1);
            DOTween.To(() => newVelocity.y, y => newVelocity.y = y, 0, 1).OnComplete(() => StartCoroutine(Delay())); 
            shouldStop = false;
        }

        if (shouldMoveFoward)
        {
            DOTween.To(() => newVelocity.y, y => newVelocity.y = y, maxYSpeed, 1.5f);
            shouldMoveFoward = false;
        }

        IEnumerator Delay()
        {
            yield return new WaitForSeconds(1);            

            shouldMoveFoward = true;
        }
    }

    private void RotateAroundPlayer()
    {
        newVelocity = transform.InverseTransformDirection(rb.velocity);
        Vector2 playerVelocity = playerRB != null ? playerRB.velocity : Vector2.zero;
        currentMaxXSpeed = maxXSpeed + Vector2.Dot(transform.right, playerVelocity) * 0.5f;
        float xAccel = currentMaxXSpeed / timeToMaxSpeed;
        float yAccel = maxYSpeed / timeToMaxSpeed;

        Vector3 playerPos = player != null ? player.position : EnemySpawner.Instance.PlayerLastPos;
        Vector2 toPlayerVector = (Vector2)playerPos - rb.position;
        rb.rotation = Vector2.SignedAngle(Vector2.up, toPlayerVector);

        if (toPlayerVector.sqrMagnitude > distanceToKeep + distanceToleranceFraction)
        {
            //Debug.Log("Distancia enorme");
            newVelocity.y = Mathf.Clamp(newVelocity.y + yAccel * Time.fixedDeltaTime, -maxYSpeed, maxYSpeed);
        }
        else if (toPlayerVector.sqrMagnitude > distanceToKeep)
        {
            //Debug.Log("Média-Grande");
            if (newVelocity.y > 0)
                newVelocity.y = Mathf.Clamp(newVelocity.y - yAccel * 1.5f * Time.fixedDeltaTime, 0, maxYSpeed);
            else if (newVelocity.y < 0)
                newVelocity.y = Mathf.Clamp(newVelocity.y + yAccel * 1.5f * Time.fixedDeltaTime, -maxYSpeed, 0);
        }
        else if (toPlayerVector.sqrMagnitude > distanceToKeep - distanceToleranceFraction)
        {
            //Debug.Log("Pequena");
            if (newVelocity.y > 0)
                newVelocity.y = Mathf.Clamp(newVelocity.y - yAccel * 1.5f * Time.fixedDeltaTime, 0, maxYSpeed);
            else if (newVelocity.y < 0)
                newVelocity.y = Mathf.Clamp(newVelocity.y + yAccel * 1.5f * Time.fixedDeltaTime, -maxYSpeed, 0);
        }
        else if (toPlayerVector.sqrMagnitude < distanceToKeep - distanceToleranceFraction)
        {
            //Debug.Log("Mínima");
            newVelocity.y = Mathf.Clamp(newVelocity.y - yAccel * Time.fixedDeltaTime * 1.5f, -maxYSpeed, maxYSpeed);
        }

        if (!isRotating)
        {
            if (newVelocity.x > 0)
                newVelocity.x = Mathf.Clamp(newVelocity.x - xAccel * Time.fixedDeltaTime, 0, currentMaxXSpeed);
            else if (newVelocity.x < 0)
                newVelocity.x = Mathf.Clamp(newVelocity.x + xAccel * Time.fixedDeltaTime, -currentMaxXSpeed, 0);
        }
        else
            newVelocity.x = Mathf.Clamp(newVelocity.x + xAccel * Time.fixedDeltaTime * rotateDirection, -currentMaxXSpeed, currentMaxXSpeed);
    }

    IEnumerator RotationCheckFrequency()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeToChangeRotation);

            float randomRotationState = Random.Range(0, 10);
            if (randomRotationState <= 4)
            {
                rotateClockWise = true;
                isRotating = true;
            }
            else if (randomRotationState <= 8)
            {
                rotateClockWise = false;
                isRotating = true;
            }
            else
                isRotating = false;

            timeToChangeRotation = Random.Range(-rotationChangeTimeVar, rotationChangeTimeVar) + rotationChangeTime;
            if (!isRotating)
                timeToChangeRotation *= 0.4f;            

        } 
    }
}