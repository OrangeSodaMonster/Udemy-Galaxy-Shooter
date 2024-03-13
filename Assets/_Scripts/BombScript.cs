using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class BombScript : MonoBehaviour
{
    [SerializeField] int startingBombs = 1;
    public static int BombAmount = 1;
    public static int MaxBombs = 3;
    public static UnityEvent OnChangeBombs = new();

    //[SerializeField] InputSO Input;
    [SerializeField] float radius;
    [SerializeField] LayerMask layersToHit;
    [SerializeField] int damage = 100;
    [SerializeField] float coolDown = 3;
    [SerializeField] VisualEffect vfx;
    [SerializeField] float damageDelay = .2f;

    float timeSinceUsedBomb = float.MaxValue;
    float bombAmountLastFrame;
    //RaycastHit2D[] hits;
    Collider2D[] hits = new Collider2D[12];
    WaitForSeconds damageWait;

    void Start()
    {
        BombAmount = startingBombs;
        vfx.gameObject.SetActive(false);
        bombAmountLastFrame = BombAmount;
        damageWait = new WaitForSeconds(damageDelay);
    }

    private void OnEnable()
    {
        InputHolder.Instance.Special += UseBomb;
    }

    private void OnDisable()
    {
        InputHolder.Instance.Special -= UseBomb;        
    }

    void Update()
    {
        timeSinceUsedBomb += Time.deltaTime;

        if(bombAmountLastFrame != BombAmount)
            OnChangeBombs?.Invoke();
    }
    private void LateUpdate()
    {
        bombAmountLastFrame = BombAmount;
    }

    void UseBomb()
    {
        if (BombAmount > 0 && timeSinceUsedBomb >= coolDown)
        {
            vfx.gameObject.SetActive(false);
            vfx.gameObject.SetActive(true);

            for (int i = 0; i < hits.Length; i++)
                hits[i] = null;

            //hits = Physics2D.CircleCastAll(transform.position, radius, Vector2.zero, 0, layersToHit);
            Physics2D.OverlapCircleNonAlloc(transform.position, radius, hits, layersToHit);

            foreach (Collider2D hit in hits)
            {
                if (hit == null) break;

                if (hit.transform.TryGetComponent(out EnemyHP enemyHP))
                {
                    StartCoroutine(ApplyDamage(hit, enemyHP));
                }
                else if (hit.transform.TryGetComponent(out LaserMove enemyProjectile))
                {
                    if (!enemyProjectile.IsPlayer)
                        enemyProjectile.DestroySequence();
                }
            }

            BombAmount--;
            timeSinceUsedBomb = 0;

            AudioManager.Instance.BombSound.PlayFeedbacks();
        }
    }

    private IEnumerator ApplyDamage(Collider2D hit, EnemyHP enemyHP)
    {
        yield return damageWait;

        enemyHP.ChangeHP(-Mathf.Abs(damage));
        GameObject hitVFX = VFXPoolerScript.Instance.BombHitVFXPooler.GetPooledGameObject();
        hitVFX.transform.position = hit.transform.position;
        hitVFX.gameObject.SetActive(true);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, radius);
    }
}