using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class DroneAttackScript : MonoBehaviour
{
    [SerializeField] DroneNumber droneNumber;
    public LineRenderer AttackLineRenderer;
    public float Range = 2.5f;
    public float DamagePerSecond = 2.5f;
    public Gradient LineColor;
    public float VFXScaleMultiplier = 1;
    [SerializeField] VisualEffect beamVFX;

    [SerializeField] float timeToDamage = .2f;
    [SerializeField] Transform fireOrigin;
    [SerializeField] LayerMask layersToHit;

    int damage = 0;
    bool isFiring;
    bool wasFiringLastFrame;
    float timeSinceDamage = float.MaxValue;
    Transform target;
    Transform player;
    //RaycastHit2D[] hits;
    Collider2D[] hits = new Collider2D[3];
    bool canChangeTarget = true;
    //float minTimeToChangeTarget = 0.5f;
    //float timeSinceChangedTarget = float.MaxValue;

    private void Start()
    {
        player = FindObjectOfType<PlayerMove>().transform;

        AttackLineRenderer.gameObject.SetActive(false);
        beamVFX.gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        if (GameStatus.IsPaused) return;
        
        damage = (int)Mathf.Ceil(DamagePerSecond * timeToDamage);

        if (target != null && (!target.gameObject.activeSelf || target.GetComponent<EnemyHP>().CurrentHP == 0))
            target = null;

        if (canChangeTarget || target == null)
        {
            target = GetClosestTarget();
            if (target != null)
                canChangeTarget = false;
        }        

        if (target != null && AttackLineRenderer != null)
        {
            transform.up = target.position - transform.position;

            AttackLineRenderer.gameObject.SetActive(true);
            AttackLineRenderer.colorGradient = LineColor;
            AttackLineRenderer.positionCount = 3;
            AttackLineRenderer.SetPosition(0, fireOrigin.position);
            Vector3 hitPos = target.GetComponent<Collider2D>().ClosestPoint(transform.position);
            AttackLineRenderer.SetPosition(1, fireOrigin.position + (hitPos - fireOrigin.position) / 2);
            AttackLineRenderer.SetPosition(2, hitPos);

            beamVFX.gameObject.SetActive(true);

            isFiring = true;
        }
        else
        {
            AttackLineRenderer.gameObject.SetActive(false);
            beamVFX.gameObject.SetActive(false);

            isFiring = false;

            transform.up = transform.position - player.position;
        }

        if(isFiring & !wasFiringLastFrame)
        {
            timeSinceDamage = 0;
            beamVFX.SetGradient("Color", LineColor);            
        }

        if (target != null && timeSinceDamage >= timeToDamage)
        {            
            if(target.TryGetComponent(out EnemyHP enemyHP))
            {
                if (GameManager.IsSurvival)
                    UpdadeCombatLog(enemyHP);
                else
                    enemyHP.ChangeHP(-Mathf.Abs(damage));

                timeSinceDamage = 0;
                canChangeTarget = true;

                GameObject vfx = VFXPoolerScript.Instance.DroneAttackVFXPooler.GetPooledGameObject();
                vfx.GetComponent<VisualEffect>().SetGradient("ColorOverLife", LineColor);
                vfx.transform.position = AttackLineRenderer.GetPosition(2);
                vfx.transform.localScale = vfx.transform.localScale * VFXScaleMultiplier;
                vfx.SetActive(true);
            }
        }

        timeSinceDamage += Time.fixedDeltaTime;
        wasFiringLastFrame = isFiring;
    }

    private void OnDisable()
    {
        AttackLineRenderer.gameObject.SetActive(false);
        beamVFX.gameObject.SetActive(false);
    }

    private Transform GetClosestTarget()
    {
        //hits = Physics2D.CircleCastAll(transform.position, Range, Vector2.zero, 0, layersToHit);

        for (int i = 0; i < hits.Length; i++)
            hits[i] = null;
        float range = Range;        

        Physics2D.OverlapCircleNonAlloc(transform.position, range, hits, layersToHit);

        Transform closestTarget = null;        
        float minDistance = float.MaxValue;

        for(int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null) break;

            if (Vector2.SqrMagnitude((Vector2)hits[i].transform.position - (Vector2)transform.position) < minDistance
                && hits[i].transform.GetComponent<EnemyHP>() != null)
            {
                minDistance = Vector2.SqrMagnitude((Vector2)hits[i].transform.position - (Vector2)transform.position);
                closestTarget = hits[i].transform;
            }
        }
                
            return closestTarget;
    }

    void UpdadeCombatLog(EnemyHP enemyHP)
    {
        switch (droneNumber)
        {
            case DroneNumber.One:
                enemyHP.ChangeHP(-Mathf.Abs(damage), ref CombatLog.Instance.Drone1TotalDamage);
                break;
            case DroneNumber.Two:
                enemyHP.ChangeHP(-Mathf.Abs(damage), ref CombatLog.Instance.Drone2TotalDamage);
                break;
            case DroneNumber.Three:
                enemyHP.ChangeHP(-Mathf.Abs(damage), ref CombatLog.Instance.Drone3TotalDamage);
                break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;

        Gizmos.DrawWireSphere(transform.position, Range);
    }
}