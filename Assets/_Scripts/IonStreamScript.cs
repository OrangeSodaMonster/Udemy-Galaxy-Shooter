using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.VFX;

public class IonStreamScript : MonoBehaviour
{

    [SerializeField] float visualDuration = .3f;
    [SerializeField, Range(0,1)] float fadePortion = .5f;
    [SerializeField] LayerMask layersToHit;
    [SerializeField] LineRenderer lineRenderer;

    bool isIonStreamEnabled;
    int damage;
    float lineWidht;
    Material material;
    float timeBetweenActivations;
    float numberOfHits;
    float radiusFromPlayer;
    float radiusFromLastHit;

    float timeSinceFired = float.MaxValue;
    Transform player;
    PlayerUpgradesManager upgradesManager;
    //RaycastHit2D[] hits;
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

        //if (isIonStreamEnabled && timeSinceFired > timeBetweenActivations && Physics2D.CircleCast(player.position, radiusFromPlayer, Vector2.zero, 0, layersToHit))
        if (isIonStreamEnabled && timeSinceFired > timeBetweenActivations && Physics2D.OverlapCircleNonAlloc(player.position, radiusFromPlayer, hits, layersToHit) > 0)
        {
            FireIonStream();
            timeSinceFired = 0;
            //Debug.Log("Fire Ion Stream");
        }
        timeSinceFired += Time.deltaTime;
    } 

    void UpdateValues()
    {
        isIonStreamEnabled = upgradesManager.CurrentUpgrades.IonStreamUpgrades.Enabled && !upgradesManager.CurrentUpgrades.IonStreamUpgrades.DisableOverwrite;

        IonStreamUpgrades ionStreamUpgrades = upgradesManager.CurrentUpgrades.IonStreamUpgrades;
        damage = upgradesManager.IonStreamUpgradesInfo.PowerUpgrades[ionStreamUpgrades.DamageLevel - 1].Damage;
        lineWidht = upgradesManager.IonStreamUpgradesInfo.PowerUpgrades[ionStreamUpgrades.DamageLevel - 1].Widht;
        material = upgradesManager.IonStreamUpgradesInfo.PowerUpgrades[ionStreamUpgrades.DamageLevel - 1].Material;
        timeBetweenActivations = upgradesManager.IonStreamUpgradesInfo.CadencyUpgrades[ionStreamUpgrades.CadencyLevel - 1].TimeBetween;
        numberOfHits = upgradesManager.IonStreamUpgradesInfo.HitNumUpgrades[ionStreamUpgrades.NumberHitsLevel - 1].NumberOfHits;
        radiusFromPlayer = upgradesManager.IonStreamUpgradesInfo.RangeUpgrades[ionStreamUpgrades.RangeLevel - 1].RangeFromPlayer;
        radiusFromLastHit = upgradesManager.IonStreamUpgradesInfo.RangeUpgrades[ionStreamUpgrades.RangeLevel - 1].RangeFromHit;
    }


    Vector2 placeHolderVector = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
    Vector2 castingOrigin;
    float castingRadius;
    Transform target;
    List<int> hitHashs = new List<int>();

    private void FireIonStream()
    {
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
                        enemyHP.ChangeHP(-Mathf.Abs(damage));

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