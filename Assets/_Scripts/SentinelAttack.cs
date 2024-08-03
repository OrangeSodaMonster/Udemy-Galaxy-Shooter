using System;
using UnityEngine;
using UnityEngine.VFX;

[Serializable]
public enum SentinelLaserTarget
{
    closest = 0,
    positive = 1,
    negative = 2,
}
public class SentinelAttack : MonoBehaviour
{
    [SerializeField] SentinelLaserTarget laserTarget = SentinelLaserTarget.closest;
    public float Range = 3;
    [HideInInspector] public int Damage = 5;
    [HideInInspector] public float DamageInterval = 1;
    [SerializeField] float vfxScaleMultiplier = 1.3f;
    [SerializeField] LayerMask layersToHit;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] VisualEffect beamVFX;
    [SerializeField] VisualEffect hitVFX;
    [SerializeField] Transform origins;
    
    bool isFiring;
    float timeSinceDamage = 0;
    Gradient lineColor;
    Transform target;
    float rangeSqr;
    PlayerHP playerHP;
    SentinelShoot shootScript;

    private void Awake()
    {
        lineColor = lineRenderer.colorGradient;
        lineRenderer.transform.localScale /= transform.lossyScale.x;
        rangeSqr = Range * Range;

        shootScript = GetComponentInParent<SentinelShoot>();
    }

    private void OnEnable()
    {
        StopAttack();
    }
    private void OnDisable()
    {
        AudioManager.Instance.PauseSentinel();
    }

    private void Start()
    {
        playerHP = FindObjectOfType<PlayerHP>();
    }

    void FixedUpdate()
    {
        if(GameStatus.IsGameover || GameStatus.IsPortal || playerHP == null) return;

        if (Vector2.SqrMagnitude(playerHP.transform.position - transform.position) <= rangeSqr)
        {
            target = playerHP.transform;
            LaserAttack();

            AudioManager.Instance.PlaySentinel();
        }
        else if (isFiring)
        {
            StopAttack();
            AudioManager.Instance.PauseSentinel();
        }
    }

    private void LateUpdate()
    {
        lineRenderer.transform.rotation = Quaternion.identity;
    }

    void LaserAttack()
    {
        Vector3 hitPos = target.GetComponent<Collider2D>().ClosestPoint(transform.position);
        Transform closestOrigin = GetOrigin(hitPos);

        ShieldStrenght strenght = null;
        if(CheckForShield(closestOrigin.position, hitPos, out ShieldStrenght shieldStr, out Vector2 shieldHit))
        {
            strenght = shieldStr;
            hitPos = shieldHit;
        }

        lineRenderer.gameObject.SetActive(true);
        lineRenderer.positionCount = 3;        
        lineRenderer.SetPosition(0, closestOrigin.position - transform.position);
        lineRenderer.SetPosition(1, (closestOrigin.position + (hitPos - closestOrigin.position) / 2) - transform.position);
        lineRenderer.SetPosition(2, hitPos - transform.position);

        beamVFX.transform.position = closestOrigin.position;
        beamVFX.gameObject.SetActive(true);

        hitVFX.transform.position = lineRenderer.GetPosition(2) + transform.position;
        hitVFX.gameObject.SetActive(true);

        isFiring = true;

        timeSinceDamage += Time.deltaTime;

        if (timeSinceDamage >= DamageInterval)
        {
            if (strenght != null && strenght.CurrentStr > 0)
                strenght.DamageStrenght(Damage);
            else if (playerHP != null)
                playerHP.ChangePlayerHP(-Mathf.Abs(Damage), playHitSound:true);            

            timeSinceDamage = 0;

            GameObject vfx = VFXPoolerScript.Instance.DroneAttackVFXPooler.GetPooledGameObject();
            vfx.GetComponent<VisualEffect>().SetGradient("ColorOverLife", lineColor);
            vfx.transform.position = lineRenderer.GetPosition(2) + transform.position;
            vfx.transform.localScale = vfx.transform.localScale * vfxScaleMultiplier;
            vfx.SetActive(true);
        }       
    }

    Transform GetOrigin(Vector3 hitPos)
    {
        if(shootScript == null || shootScript.ShooterIndex < 0)
        {
            Transform closestOrigin = null;
            foreach (Transform t in origins)
            {
                if (closestOrigin == null || Vector2.SqrMagnitude(hitPos - t.position) < Vector2.SqrMagnitude(hitPos - closestOrigin.position))
                {
                    closestOrigin = t;
                }
            }

            return closestOrigin;
        }

        int originIndex;
        if (laserTarget == SentinelLaserTarget.positive)
        {
            originIndex = shootScript.ShooterIndex + 1;
            if (originIndex >= origins.childCount)
                originIndex = 0;
        }
        else if (laserTarget == SentinelLaserTarget.negative)
        {
            originIndex = shootScript.ShooterIndex - 1;
            if (originIndex < 0)
                originIndex = origins.childCount - 1;
        }
        else
        {
            int posIndex = shootScript.ShooterIndex + 1;
            if(posIndex >= origins.childCount) posIndex = 0;

            int negIndex = shootScript.ShooterIndex - 1;
            if (negIndex < 0) negIndex = origins.childCount - 1;

            originIndex = posIndex;
            if (Vector2.SqrMagnitude(hitPos - origins.GetChild(posIndex).position) > 
                Vector2.SqrMagnitude(hitPos - origins.GetChild(negIndex).position))
            {
                originIndex = negIndex;
            }
        }

        return origins.GetChild(originIndex);
    }

    void StopAttack()
    {
        lineRenderer.gameObject.SetActive(false);
        beamVFX.gameObject.SetActive(false);
        hitVFX.gameObject.SetActive(false);
        timeSinceDamage = 0;

        isFiring= false;
    }

    bool CheckForShield(Vector2 originPos, Vector2 hitPos, out ShieldStrenght shieldStr, out Vector2 shieldHit)
    {
        shieldStr = null;
        shieldHit = Vector2.zero;

        Vector2 dir = (hitPos - originPos).normalized;
        float distance = Vector2.Distance(originPos, hitPos);

        RaycastHit2D[] hits = Physics2D.RaycastAll(originPos, dir, distance, layersToHit);

        foreach (RaycastHit2D hit in hits)
        {
            if(hit.transform.TryGetComponent(out shieldStr))
            {
                shieldHit = hit.point;
                return true;
            }
        }

        return false;
    }

}