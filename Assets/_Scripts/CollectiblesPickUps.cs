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

    Vector3 driftDirection = Vector3.zero;
    float driftSpeed;

    Vector3 moveDir = Vector3.zero;
    float currentMoveSpeed = 0;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        driftDirection = Random.insideUnitCircle.normalized;
        moveDir = driftDirection;
        driftSpeed = Random.Range(minDriftMoveSpeed, maxDriftMoveSpeed);
        currentMoveSpeed = driftSpeed;

        rb.velocity = currentMoveSpeed * moveDir;

        StartCoroutine(DestroyCD());

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerMove>() != null)
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

    IEnumerator DestroyCD()
    {
        yield return new WaitForSeconds(timeDuration);

        Destroy(gameObject);
    }
}