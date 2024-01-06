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

    bool isDrifting = true;
    Vector3 driftDirection = Vector3.zero;
    float driftSpeed;

    Rigidbody2D rb;
    TractorBeamScript tractorBeam = null;
    public Vector3 MoveDir { get; private set; } = Vector3.zero;
    float maxAtractionSpeed = 2f;
    float acceleration = 1f;
    public float CurrentMoveSpeed { get; private set; } = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        driftDirection = Random.insideUnitCircle.normalized;
        MoveDir = driftDirection;
        driftSpeed = Random.Range(minDriftMoveSpeed, maxDriftMoveSpeed);
        CurrentMoveSpeed = driftSpeed;
        rb.isKinematic = true;

        StartCoroutine(DestroyCD());
    }

    private void Update()
    {
        if (!(rb.isKinematic)) return;

        if (isDrifting)
        {
            CurrentMoveSpeed = Mathf.Clamp(CurrentMoveSpeed - acceleration * Time.smoothDeltaTime, driftSpeed, maxAtractionSpeed);
        }
        else
        {
            CurrentMoveSpeed = Mathf.Clamp(CurrentMoveSpeed + acceleration * Time.smoothDeltaTime, driftSpeed, maxAtractionSpeed);
            MoveDir = tractorBeam.transform.position - transform.position;
        }

        transform.Translate(CurrentMoveSpeed*Time.smoothDeltaTime*MoveDir, Space.World);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<TractorBeamScript>() != null)
        {
            isDrifting = false;
            tractorBeam = collision.GetComponent<TractorBeamScript>();
            maxAtractionSpeed = tractorBeam.TotalPullForce;
            acceleration = tractorBeam.TotalPullForce/tractorBeam.TimeToMaxPullSpeed;
        }
        else if (collision.GetComponent<PlayerMove>())
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
        isDrifting = true;
    }

    IEnumerator DestroyCD()
    {
        yield return new WaitForSeconds(timeDuration);

        Destroy(gameObject);
    }
}