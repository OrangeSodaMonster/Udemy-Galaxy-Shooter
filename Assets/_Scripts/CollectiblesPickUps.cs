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

    PlayerTractorBeam tractorBeam = null;
    Vector3 moveDir = Vector3.zero;
    float maxAtractionSpeed = 2f;
    float acceleration = 1f;
    float currentMoveSpeed = 0;

    private void Start()
    {
        driftDirection = Random.insideUnitCircle.normalized;
        moveDir = driftDirection;
        driftSpeed = Random.Range(minDriftMoveSpeed, maxDriftMoveSpeed);
        currentMoveSpeed = driftSpeed;

        StartCoroutine(DestroyCD());
    }

    private void Update()
    {
        if (isDrifting)
        {
            currentMoveSpeed = Mathf.Clamp(currentMoveSpeed - acceleration * Time.deltaTime, driftSpeed, maxAtractionSpeed);
        }
        else
        {
            currentMoveSpeed = Mathf.Clamp(currentMoveSpeed + acceleration * Time.deltaTime, driftSpeed, maxAtractionSpeed);
            moveDir = tractorBeam.transform.position - transform.position;
        }

        transform.Translate(moveDir * currentMoveSpeed * Time.deltaTime, Space.World);

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "TractorBeam")
        {
            isDrifting = false;
            tractorBeam = collision.GetComponent<PlayerTractorBeam>();
            maxAtractionSpeed = tractorBeam.MaxPullSpeed;
            acceleration = tractorBeam.MaxPullSpeed/tractorBeam.TimeToMaxPullSpeed;
        }
        else
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