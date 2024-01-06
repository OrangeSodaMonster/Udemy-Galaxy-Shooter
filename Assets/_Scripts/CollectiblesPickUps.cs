using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CollectibleType
{
    MetalCrumb,
    RareMetalCrumb,
    EnergyCristal,
    RareEnergyCristal,
}

public class CollectiblesPickUps : MonoBehaviour
{
    [SerializeField] CollectibleType type;

    [SerializeField] float minDriftMoveSpeed = 0.1f;
    [SerializeField] float maxDriftMoveSpeed = 0.6f;
    [SerializeField] float timeDuration = 30f;

    Rigidbody2D rb;

    bool isDrifting = true;
    Vector2 driftDirection = Vector2.zero;
    float currentDriftSpeed;
    float driftSpeed;

    TractorBeamScript tractorBeam = null;
    float maxAtractionSpeed;
    float acceleration = 1;
    bool isTractor;
    float tractorCurrentPull;
    Vector2 tractorVelocity = Vector2.zero;

    BlackHolePull blackHole = null;
    float bhMaxPullSpeed = 0;
    float bhAccel = 0;
    bool isBlackHole;
    float bhCurrentPull;
    Vector2 bhVelocity = Vector2.zero;

    Vector2 moveVelocity = new();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        driftDirection = Random.insideUnitCircle.normalized;
        driftSpeed = Random.Range(minDriftMoveSpeed, maxDriftMoveSpeed);
        currentDriftSpeed = driftSpeed;

        StartCoroutine(DestroyCD());
    }

    private void Update()
    {
        if (!isBlackHole && !isTractor)
            isDrifting = true;

        if (isDrifting)
        {
            currentDriftSpeed = Mathf.Clamp(currentDriftSpeed - acceleration * 0.35f * Time.smoothDeltaTime, driftSpeed, currentDriftSpeed);
            moveVelocity = driftDirection * currentDriftSpeed;
        }
        else
        {
            if (isTractor)
            {
                tractorCurrentPull = Mathf.Clamp(tractorCurrentPull + acceleration * Time.smoothDeltaTime, driftSpeed, maxAtractionSpeed);                
                tractorVelocity = tractorCurrentPull * (tractorBeam.transform.position - transform.position).normalized;
            }
            if(isBlackHole)
            {
                bhCurrentPull = Mathf.Clamp(bhCurrentPull + bhAccel * Time.smoothDeltaTime, driftSpeed, bhMaxPullSpeed);
                bhVelocity = bhCurrentPull * (blackHole.transform.position - transform.position).normalized;
            }

            moveVelocity = tractorVelocity + bhVelocity;
            driftDirection = (moveVelocity).normalized;
            currentDriftSpeed = moveVelocity.magnitude;
        }
      
        //rb.MovePosition(rb.position + moveVelocity * Time.smoothDeltaTime);
        transform.Translate(moveVelocity * Time.smoothDeltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<BlackHolePull>() != null)
        {
            blackHole = collision.GetComponent<BlackHolePull>();
            isDrifting = false;
            bhMaxPullSpeed = blackHole.CollectiblePullForce;
            bhAccel = bhMaxPullSpeed / blackHole.TimeToMaxPullCollec;
            isBlackHole = true;
        }
        else if (collision.GetComponent<TractorBeamScript>() != null)
        {
            tractorBeam = collision.GetComponent<TractorBeamScript>();
            isDrifting = false;
            maxAtractionSpeed = tractorBeam.TotalPullForce;
            acceleration = tractorBeam.TotalPullForce/tractorBeam.TimeToMaxPullSpeed;
            isTractor = true;
        }
        else if (collision.GetComponent<PlayerMove>() != null)
        {
            switch (type)
            {
                case CollectibleType.MetalCrumb:
                    PlayerCollectiblesCount.MetalCrumbsAmount += 1;
                    break;
                case CollectibleType.RareMetalCrumb:
                    PlayerCollectiblesCount.RareMetalCrumbsAmount += 1;
                    break;
                case CollectibleType.EnergyCristal:
                    PlayerCollectiblesCount.EnergyCristalAmount += 1;
                    break;
                case CollectibleType.RareEnergyCristal:
                    PlayerCollectiblesCount.CondensedEnergyCristalAmount += 1;
                    break;
            }

            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.GetComponent<BlackHolePull>()  != null)
        {
            isBlackHole = false;
            bhCurrentPull = 0;
        }
        else if(collision.GetComponent<TractorBeamScript>() != null)
        {
            tractorCurrentPull = 0;
            isTractor = false;
        }
    }

    IEnumerator DestroyCD()
    {
        yield return new WaitForSeconds(timeDuration);

        Destroy(gameObject);
    }
}