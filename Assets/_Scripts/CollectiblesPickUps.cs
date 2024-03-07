using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CollectibleType
{
    MetalCrumb = 1,
    RareMetalCrumb = 2,
    EnergyCristal = 3,
    RareEnergyCristal = 4,
}

public class CollectiblesPickUps : MonoBehaviour
{
    [SerializeField] CollectibleType type;

    [SerializeField] float minDriftMoveSpeed = 0.1f;
    [SerializeField] float maxDriftMoveSpeed = 0.6f;

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
    Vector3 defaultScale = new();

    private void Awake()
    {
        defaultScale = transform.localScale;
    }

    private void OnEnable()
    {
        driftDirection = Random.insideUnitCircle.normalized;
        driftSpeed = Random.Range(minDriftMoveSpeed, maxDriftMoveSpeed);
        currentDriftSpeed = driftSpeed;
        transform.localScale = defaultScale;

        GameStatus.GameOver += DriftOnGameover;
    }

    private void OnDisable()
    {
        GameStatus.GameOver -= DriftOnGameover;
        isBlackHole = false;
        isTractor = false;

    }

    void DriftOnGameover()
    {
        isTractor = false;
        isDrifting = true;
    }

    private void Update()
    {
        if (!isBlackHole && !isTractor)
            isDrifting = true;

        if (isDrifting)
        {
            currentDriftSpeed = Mathf.Clamp(currentDriftSpeed - acceleration * 0.35f * Time.deltaTime, driftSpeed, currentDriftSpeed);
            moveVelocity = driftDirection * currentDriftSpeed;
        }
        else
        {
            if (isTractor)
            {
                tractorCurrentPull = Mathf.Clamp(tractorCurrentPull + acceleration * Time.deltaTime, Mathf.Abs(driftSpeed), Mathf.Abs(maxAtractionSpeed));
                tractorVelocity = tractorCurrentPull * (tractorBeam.transform.position - transform.position).normalized;
            }
            else
                tractorVelocity = Vector2.zero;

            if(isBlackHole)
            {
                bhCurrentPull = Mathf.Clamp(bhCurrentPull + bhAccel * Time.deltaTime, Mathf.Abs(driftSpeed), Mathf.Abs(bhMaxPullSpeed));
                bhVelocity = bhCurrentPull * (blackHole.transform.position - transform.position).normalized;
            }
            else
                bhVelocity = Vector2.zero;

            moveVelocity = tractorVelocity + bhVelocity;
            driftDirection = (moveVelocity).normalized;
            currentDriftSpeed = moveVelocity.magnitude;
        }
      
        //rb.MovePosition(rb.position + moveVelocity * Time.smoothDeltaTime);
        transform.Translate(moveVelocity * Time.deltaTime, Space.World);
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
                    AudioManager.Instance.PlayMetalPickUpSound(AudioManager.Instance.MetalCrumbSound);
                    PlayerCollectiblesCount.MetalAmount += 1;
                    break;
                case CollectibleType.RareMetalCrumb:
                    AudioManager.Instance.PlayMetalPickUpSound(AudioManager.Instance.RareMetalCrumbSound);
                    PlayerCollectiblesCount.RareMetalAmount += 1;
                    break;
                case CollectibleType.EnergyCristal:
                    AudioManager.Instance.EnergyCrystalSound.PlayFeedbacks();
                    PlayerCollectiblesCount.EnergyCristalAmount += 1;
                    break;
                case CollectibleType.RareEnergyCristal:
                    AudioManager.Instance.CondensedEnergyCrystalSound.PlayFeedbacks();
                    PlayerCollectiblesCount.CondensedEnergyCristalAmount += 1;
                    break;
            }

            PlayerCollectiblesCount.ChangedCollectbleAmount();
            DestroySequence();
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

    public void DestroySequence()
    {
        gameObject.SetActive(false);
    }
}