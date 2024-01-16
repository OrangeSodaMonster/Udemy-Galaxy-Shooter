using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DroneAttackScript : MonoBehaviour
{
    public LineRenderer AttackLineRenderer;
    public float Range = 2.5f;
    public float DamagePerSecond = 2.5f;
    public Gradient LineColor;
    public float VFXScaleMultiplier = 1;

    [SerializeField] float timeToDamage = .2f;
    [SerializeField] Transform fireOrigin;
    [SerializeField] LayerMask layersToHit;

    int damage = 0;
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
        damage = (int)Mathf.Ceil(DamagePerSecond * timeToDamage);

        target = GetClosestTarget();

        if (target != null & AttackLineRenderer != null)
        {
            transform.up = target.position - transform.position;

            AttackLineRenderer.gameObject.SetActive(true);
            AttackLineRenderer.colorGradient = LineColor;
            AttackLineRenderer.positionCount = 3;
            AttackLineRenderer.SetPosition(0, fireOrigin.position);
            AttackLineRenderer.SetPosition(1, fireOrigin.position + (target.position - fireOrigin.position) / 2);
            AttackLineRenderer.SetPosition(2, target.position);

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
            timeSinceDamage = 0;
        }

        if (target != null & timeSinceDamage > timeToDamage)
        {            
            if(target.GetComponent<EnemyHP>() != null)
            {
                target.GetComponent<EnemyHP>().ChangeHP(-Mathf.Abs(damage));
                timeSinceDamage = 0;

                GameObject vfx = VFXPoolerScript.Instance.DroneAttackVFXPooler.GetPooledGameObject();
                vfx.GetComponent<VisualEffect>().SetGradient("ColorOverLife", LineColor);
                vfx.transform.position = target.position;
                vfx.transform.localScale = (VFXScaleMultiplier) * Vector3.one;
                vfx.SetActive(true);
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
        hits = Physics2D.CircleCastAll(transform.position, Range, Vector2.zero, 0, layersToHit);
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

        Gizmos.DrawWireSphere(transform.position, Range);
    }
}