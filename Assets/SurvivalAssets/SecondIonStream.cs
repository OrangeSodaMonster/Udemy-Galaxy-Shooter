using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SecondIonStream : MonoBehaviour
{
    [SerializeField] float visualDuration = .3f;
    [SerializeField, Range(0, 1)] float fadePortion = .5f;
    [SerializeField] LayerMask layersToHit;
    [SerializeField] LineRenderer lineRenderer;

    int damage;
    float lineWidht;
    Material material;
    float numberOfHits;
    float radiusFromPlayer;
    float radiusFromLastHit;

    Transform player;
    PlayerUpgradesManager upgradesManager;
    List<Vector2> lineNodes = new List<Vector2>();
    float fadeDuration;
    float timeToStartFade;
    Color2 defaultColor = new();
    Color2 endColor = new();
    Gradient defaultLineGrad = new Gradient();

    Collider2D[] hits = new Collider2D[3];

    WaitForSeconds fadeWait;

    void Start()
    {
        player = FindAnyObjectByType<PlayerMove>().transform;
        upgradesManager = FindAnyObjectByType<PlayerUpgradesManager>();
        UpdateValues();

        lineRenderer.gameObject.SetActive(false);

        fadeDuration = visualDuration * fadePortion;
        timeToStartFade = visualDuration - fadeDuration;

        defaultColor.ca = Color.white;
        defaultColor.cb = Color.white;

        Color endC = Color.clear;
        endColor.ca = endC;
        endColor.cb = endC;

        defaultLineGrad = lineRenderer.colorGradient;

        fadeWait = new WaitForSeconds(timeToStartFade);
    }

    void Update()
    {
        if (GameStatus.IsPaused || GameStatus.IsPortal) return;

        UpdateValues();        
    }

    void UpdateValues()
    {        
        IonStreamUpgrades ionStreamUpgrades = upgradesManager.CurrentUpgrades.IonStreamUpgrades;
        int damageLevel = ionStreamUpgrades.DamageLevel - 2;
        if(damageLevel < 0) damageLevel = 0;
        damage = upgradesManager.IonStreamUpgradesInfo.PowerUpgrades[damageLevel].Damage;
        lineWidht = upgradesManager.IonStreamUpgradesInfo.PowerUpgrades[damageLevel].Widht;
        material = upgradesManager.IonStreamUpgradesInfo.PowerUpgrades[damageLevel].Material;
        int numberHitsLevel = ionStreamUpgrades.NumberHitsLevel - 2;
        if (damageLevel < 0) numberHitsLevel = 0;
        numberOfHits = upgradesManager.IonStreamUpgradesInfo.HitNumUpgrades[numberHitsLevel].NumberOfHits;
        int rangeLevel = ionStreamUpgrades.RangeLevel - 2;
        if (damageLevel < 0) rangeLevel = 0;
        radiusFromPlayer = upgradesManager.IonStreamUpgradesInfo.RangeUpgrades[rangeLevel].RangeFromPlayer;
        radiusFromLastHit = upgradesManager.IonStreamUpgradesInfo.RangeUpgrades[rangeLevel].RangeFromHit;

        if (GameManager.IsSurvival)
        {
            damage += BonusPowersDealer.Instance.IonStreamPower;
            float bonusMultiplier = 1 + BonusPowersDealer.Instance.DroneIonStreamBombRange/100;
            radiusFromPlayer *= bonusMultiplier;
            radiusFromLastHit *= bonusMultiplier;
        }
    }


    Vector2 placeHolderVector = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
    Vector2 castingOrigin;
    float castingRadius;
    Transform target;
    List<int> hitHashs = new List<int>();

    public bool HasResetMainIon = true;
    public void FireSecondIonStream()
    {
        if (HasResetMainIon && Physics2D.OverlapCircleNonAlloc(player.position, radiusFromPlayer, hits, layersToHit) > 0)
            FireIonStream();
    }

    void FireIonStream()
    {
        if(!GameManager.IsSurvival || !BonusPowersDealer.Instance.IsSecondIonStream) return;

        castingOrigin = player.position;
        castingRadius = radiusFromPlayer;
        hitHashs.Clear();
        lineNodes.Clear();
        lineNodes.Add(player.position);
        hits = new Collider2D[(int)numberOfHits+1];
        //Debug.Log(numberOfHits);

        for (int j = 0; j < numberOfHits; j++)
        {
            for (int i = 0; i < hits.Length; i++)
                hits[i] = null;

            //hits = Physics2D.CircleCastAll(castingOrigin, castingRadius, Vector2.zero, 0, layersToHit);
            Physics2D.OverlapCircleNonAlloc(castingOrigin, castingRadius, hits, layersToHit);
            lineNodes.Add(placeHolderVector);
            hitHashs.Add(0);

            //Debug.Log("Radius: " + testingRadius + " --- Origin: " + testingOrigin);

            if (hits != null)
            {
                float minDistance = float.MaxValue;
                target = null;
                //foreach (RaycastHit2D hit in hits)
                for (int i = 0; i < hits.Length; i++)
                {
                    //Debug.Log(hit.transform.name);
                    //Debug.Log(hit.transform.name + " pos: " + hit.transform.position +  " distance: " + (testingOrigin - (Vector2)hit.transform.position).sqrMagnitude);
                    if (hits[i] == null) break;

                    if (!hitHashs.Contains(hits[i].transform.GetHashCode()) &&
                        Vector2.SqrMagnitude((Vector2)hits[i].transform.position - castingOrigin) < minDistance &&
                        hits[i].transform.GetComponent<EnemyHP>() != null)
                    {
                        minDistance = Vector2.SqrMagnitude((Vector2)hits[i].transform.position - castingOrigin);
                        target = hits[i].transform;
                        lineNodes[j+1]= (Vector2)hits[i].transform.position;
                    }
                }
                castingRadius = radiusFromLastHit;
                if (target != null)
                {
                    //if(target.GetComponent<EnemyHP>() != null)
                    if (target.TryGetComponent(out EnemyHP enemyHP))
                    {
                        if (GameManager.IsSurvival)
                        {
                            SurvivalManager.CombatLog.IonStreamTotalDamage += (int)MathF.Min(Mathf.Abs(damage), enemyHP.CurrentHP);
                        }

                        enemyHP.ChangeHP(-Mathf.Abs(damage));
                    }

                    GameObject vfx = VFXPoolerScript.Instance.IonStreamVFXPooler.GetPooledGameObject();
                    //vfx.GetComponent<VisualEffect>().SetGradient("ColorOverLife", LineColor);
                    vfx.transform.position = target.position;
                    vfx.GetComponent<VisualEffect>().SetGradient("ColorOverLife",
                        upgradesManager.IonStreamUpgradesInfo.PowerUpgrades[upgradesManager.CurrentUpgrades.IonStreamUpgrades.DamageLevel-1].VFXGradient);
                    vfx.transform.localScale = (0.95f+0.05f*upgradesManager.CurrentUpgrades.IonStreamUpgrades.DamageLevel) * Vector3.one;
                    vfx.SetActive(true);

                    castingOrigin = (Vector2)target.transform.position;
                    hitHashs[j]= target.GetHashCode();
                }
            }
        }
        lineNodes.RemoveAll(node => node == placeHolderVector);

        if (lineNodes.Count >= 2)
        {
            AudioManager.Instance.IonStreamSound.PlayFeedbacks();

            if (lineNodes.Count == 2)
            {
                lineNodes.Add(lineNodes[1]);
                lineNodes[1] = lineNodes[0] + (lineNodes[1] - lineNodes[0]) / 2;
            }

            lineRenderer.gameObject.SetActive(true);
            lineRenderer.colorGradient = defaultLineGrad;
            lineRenderer.material = material;
            lineRenderer.widthMultiplier = lineWidht;
            lineRenderer.positionCount = lineNodes.Count;
            for (int i = 0; i < lineNodes.Count; i++)
            {
                lineRenderer.SetPosition(i, lineNodes[i]);
            }
            StartCoroutine(DisableLine(lineRenderer, visualDuration));
        }
    }

    IEnumerator DisableLine(LineRenderer line, float time)
    {
        yield return fadeWait;

        line.DOColor(defaultColor, endColor, fadeDuration).OnComplete(() => line.gameObject.SetActive(false));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position, radiusFromPlayer);
        Gizmos.DrawWireSphere(transform.position, radiusFromLastHit);

    }
}
