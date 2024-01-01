using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAttackScript : MonoBehaviour
{
    public LineRenderer AttackLineRenderer;
    [SerializeField] float damage = 1f;
    [SerializeField] float timeToDamage = 1f;
    [SerializeField] float range = 2.5f;
    [SerializeField] Transform fireOrigin;
    [SerializeField] LayerMask layersToHit;

    bool isFiring;
    bool wasFiringLastFrame;
    float timeSinceDamage = float.MaxValue;
    Transform target;
    Transform player;
    RaycastHit2D[] hits;

    private void Start()
    {
        player = FindObjectOfType<PlayerMove>().transform;

        AttackLineRenderer.gameObject.SetActive(false);
    }

    void Update()
    {
        target = GetClosestTarget();

        if (target != null & AttackLineRenderer != null)
        {
            transform.up = target.position - transform.position;

            AttackLineRenderer.gameObject.SetActive(true);
            AttackLineRenderer.positionCount = 2;
            AttackLineRenderer.SetPosition(0, fireOrigin.position);
            AttackLineRenderer.SetPosition(1, target.position);

            isFiring = true;
        }
        else
        {
            AttackLineRenderer.gameObject.SetActive(false);
            isFiring = false;

            transform.up = transform.position - player.position;
        }

        if(isFiring & !wasFiringLastFrame)
        {
            Debug.Log(name + " started firing at " + target.name);
            timeSinceDamage = 0;
        }

        if (target != null & timeSinceDamage > timeToDamage)
        {            
            if(target.GetComponent<EnemyHP>() != null)
            {
                target.GetComponent<EnemyHP>().ChangeHP(-Mathf.Abs(damage));
                timeSinceDamage = 0;
            }
        }

        timeSinceDamage += Time.deltaTime;
        wasFiringLastFrame = isFiring;
    }

    private void OnDisable()
    {
        AttackLineRenderer.gameObject.SetActive(false);
    }

    private Transform GetClosestTarget()
    {
        hits = Physics2D.CircleCastAll(transform.position, range, Vector2.zero, 0, layersToHit);
        Transform closestTarget = null;
        if (hits.Length > 0)
        {
            float minDistance = float.MaxValue;
            foreach (RaycastHit2D hit in hits)
            {
                if (Vector2.SqrMagnitude((Vector2)hit.transform.position - (Vector2)transform.position) < minDistance
                    & hit.transform.GetComponent<EnemyHP>() != null)
                {
                    minDistance = Vector2.SqrMagnitude((Vector2)hit.transform.position - (Vector2)transform.position);
                    closestTarget = hit.transform;
                }
            }
        }        
            return closestTarget;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;

        Gizmos.DrawWireSphere(transform.position, range);
    }
}