using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.VFX;

public class SentinelAttack : MonoBehaviour
{
    [SerializeField] float range = 3;
    [SerializeField] int damage = 5;
    [SerializeField] float damageInterval = 1;
    [SerializeField] float vfxScaleMultiplier = 1.3f;
    [SerializeField] LayerMask layersToHit;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] VisualEffect beamVFX;
    [SerializeField] Transform origins;
    
    bool isFiring;
    float timeSinceDamage = 0;
    Gradient lineColor;
    RaycastHit2D[] hits;
    Transform target;

    private void Awake()
    {
        //lineRenderer = GetComponentInChildren<LineRenderer>();
        lineColor = lineRenderer.colorGradient;
        lineRenderer.transform.localScale /= transform.localScale.x;
        //beamVFX = GetComponentInChildren<VisualEffect>();
    }

    private void OnEnable()
    {
        StopAttack();
    }

    void FixedUpdate()
    {
        target = GetClosestTarget();        

        if (target != null)
        {
            PlayerHP playerHP = target.GetComponent<PlayerHP>();
            ShieldStrenght shieldStr = target.GetComponent<ShieldStrenght>();

            LaserAttack(playerHP, shieldStr);
        }
        else if (isFiring)
        {
            StopAttack();
        }
    }

    void LaserAttack(PlayerHP playerHP, ShieldStrenght strenght)
    {
        lineRenderer.gameObject.SetActive(true);
        lineRenderer.positionCount = 3;
        Vector3 hitPos = target.GetComponent<Collider2D>().ClosestPoint(transform.position);
        Transform closestOrigin = GetClosestOrigin(hitPos);
        lineRenderer.SetPosition(0, closestOrigin.position - transform.position);
        lineRenderer.SetPosition(1, (closestOrigin.position + (hitPos - closestOrigin.position) / 2) - transform.position);
        lineRenderer.SetPosition(2, hitPos - transform.position);

        beamVFX.transform.position = closestOrigin.position;
        beamVFX.gameObject.SetActive(true);

        isFiring = true;

        timeSinceDamage += Time.deltaTime;

        if (timeSinceDamage >= damageInterval)
        {
            if(playerHP != null)
                playerHP.ChangePlayerHP(-Mathf.Abs(damage), playHitSound:true);
            else if(strenght != null)
                strenght.DamageStrenght(damage);

            timeSinceDamage = 0;

            GameObject vfx = VFXPoolerScript.Instance.DroneAttackVFXPooler.GetPooledGameObject();
            vfx.GetComponent<VisualEffect>().SetGradient("ColorOverLife", lineColor);
            vfx.transform.position = lineRenderer.GetPosition(2) + transform.position;
            vfx.transform.localScale = vfx.transform.localScale * vfxScaleMultiplier;
            vfx.SetActive(true);
        }       
    }

    Transform GetClosestOrigin(Vector3 hitPos)
    {
        Transform closestOrigin = null;
        foreach (Transform t in origins)
        {
            if(closestOrigin == null || Vector2.SqrMagnitude(hitPos - t.position) < Vector2.SqrMagnitude(hitPos - closestOrigin.position))
            {
                closestOrigin = t;
            }
        }

        return closestOrigin;
    }

    void StopAttack()
    {
        lineRenderer.gameObject.SetActive(false);
        beamVFX.gameObject.SetActive(false);
        timeSinceDamage = 0;

        isFiring= false;
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
                    && (hit.transform.GetComponent<PlayerHP>() != null || hit.transform.GetComponent<ShieldStrenght>() != null))
                {
                    minDistance = Vector2.SqrMagnitude((Vector2)hit.transform.position - (Vector2)transform.position);
                    closestTarget = hit.transform;
                }
            }
        }
        return closestTarget;
    }

}