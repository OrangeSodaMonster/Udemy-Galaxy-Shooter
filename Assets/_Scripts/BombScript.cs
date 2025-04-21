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
    public static int BaseDamage = 200;
    public static float BaseRange = 5.5f;
    public static UnityEvent OnChangeBombs = new();

    //[SerializeField] InputSO Input;
    [SerializeField] float radius;
    [SerializeField] LayerMask layersToHit;
    [SerializeField] int damage = 100;
    [SerializeField] float coolDown = 3;
    [SerializeField] VisualEffect vfx;
    [SerializeField] VisualEffect superVfx;
    [SerializeField] float damageDelay = .2f;

    float timeSinceUsedBomb = float.MaxValue;
    //RaycastHit2D[] hits;
    Collider2D[] hits = new Collider2D[12];
    WaitForSeconds damageWait;

    void Start()
    {
        BombAmount = startingBombs;
        PlayerStats.Instance.Bomb.Charges = BombAmount;
        vfx.gameObject.SetActive(false);
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
        UpdateValues();
        timeSinceUsedBomb += Time.deltaTime;

        BonusBombGenerator();
    }

    void UpdateValues()
    {
        damage = PlayerStats.Instance.Bomb.CurrentPower;
        radius = PlayerStats.Instance.Bomb.CurrentRange;

        vfx.transform.localScale = Vector3.one * PlayerStats.Instance.Bomb.RangeModifier;
        superVfx.transform.localScale = Vector3.one * PlayerStats.Instance.Bomb.RangeModifier;
    }

    void UseBomb()
    {
        if(GameManager.IsSurvival && BonusPowersDealer.Instance.IsSuperBomb)
        {
            vfx = superVfx;
        }

        if (BombAmount > 0 && timeSinceUsedBomb >= coolDown)
        {
            vfx.gameObject.SetActive(false);
            vfx.gameObject.SetActive(true);

            for (int i = 0; i < hits.Length; i++)
                hits[i] = null;

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
            PlayerStats.Instance.Bomb.Charges = BombAmount;
            OnChangeBombs?.Invoke();
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

    public static void AddBomb(int number)
    {
        if(BombAmount == MaxBombs) return;

        BombAmount += number;
        if(BombAmount > MaxBombs) BombAmount = MaxBombs;

        PlayerStats.Instance.Bomb.Charges = BombAmount;
        OnChangeBombs?.Invoke();

        AudioManager.Instance.BombPickSound.PlayFeedbacks();
    }

    float timeSinceBonusBomb = 0;
    static public float TimeToBonusBombPerc = 0;
    void BonusBombGenerator()
    {
        if(!GameManager.IsSurvival) return;

        if(BonusPowersDealer.Instance.BombGeneration > 0 && timeSinceBonusBomb > BonusPowersDealer.Instance.BombGeneration)
        {
            AddBomb(1);
            timeSinceBonusBomb = 0;
        }

        timeSinceBonusBomb += Time.deltaTime;
        TimeToBonusBombPerc = timeSinceBonusBomb/BonusPowersDealer.Instance.BombGeneration;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, radius);
    }
}