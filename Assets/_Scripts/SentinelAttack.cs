using UnityEngine;
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
    [SerializeField] VisualEffect hitVFX;
    [SerializeField] Transform origins;
    
    bool isFiring;
    float timeSinceDamage = 0;
    Gradient lineColor;
    Transform target;
    float rangeSqr;
    PlayerHP playerHP;

    private void Awake()
    {
        lineColor = lineRenderer.colorGradient;
        lineRenderer.transform.localScale /= transform.localScale.x;
        rangeSqr = range * range;
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

    void LaserAttack()
    {
        Vector3 hitPos = target.GetComponent<Collider2D>().ClosestPoint(transform.position);
        Transform closestOrigin = GetClosestOrigin(hitPos);

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

        if (timeSinceDamage >= damageInterval)
        {
            if (strenght != null && strenght.CurrentStr > 0)
                strenght.DamageStrenght(damage);
            else if (playerHP != null)
                playerHP.ChangePlayerHP(-Mathf.Abs(damage), playHitSound:true);            

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