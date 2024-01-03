using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IonStreamScript : MonoBehaviour
{

    [SerializeField] float visualDuration = .3f;
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
    RaycastHit2D[] hits;
    List<Vector2> lineNodes = new List<Vector2>();

    void Start()
    {
        player = FindAnyObjectByType<PlayerMove>().transform;
        upgradesManager = FindAnyObjectByType<PlayerUpgradesManager>();
        UpdateValues();

        lineRenderer.gameObject.SetActive(false);
    }

    void Update()
    {
        UpdateValues();

        if (isIonStreamEnabled && timeSinceFired > timeBetweenActivations && Physics2D.CircleCast(player.position, radiusFromPlayer, Vector2.zero, 0, layersToHit))
        {
            FireIonStream();
            timeSinceFired = 0;
            //Debug.Log("Fire Ion Stream");
        }
        timeSinceFired += Time.deltaTime;
    } 

    void UpdateValues()
    {
        isIonStreamEnabled = upgradesManager.CurrentUpgrades.IonStreamUpgrades.Enabled;

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

        for (int i = 0; i < numberOfHits; i++)
        {
            hits = Physics2D.CircleCastAll(castingOrigin, castingRadius, Vector2.zero, 0, layersToHit);
            lineNodes.Add(placeHolderVector);
            hitHashs.Add(0);

            //Debug.Log("Radius: " + testingRadius + " --- Origin: " + testingOrigin);

            if (hits != null)
            {
                float minDistance = float.MaxValue;
                target = null;
                foreach (RaycastHit2D hit in hits)
                {
                    //Debug.Log(hit.transform.name);
                    //Debug.Log(hit.transform.name + " pos: " + hit.transform.position +  " distance: " + (testingOrigin - (Vector2)hit.transform.position).sqrMagnitude);
                    if (!hitHashs.Contains(hit.transform.GetHashCode()) & hit.transform.GetComponent<EnemyHP>() != null
                        & Vector2.SqrMagnitude((Vector2)hit.transform.position - castingOrigin) < minDistance)
                    {
                        minDistance = Vector2.SqrMagnitude((Vector2)hit.transform.position - castingOrigin);
                        target = hit.transform;
                        lineNodes[i+1]= (Vector2)hit.transform.position;                        
                    }
                }
                castingRadius = radiusFromLastHit;
                if (target != null)
                {
                    if(target.GetComponent<EnemyHP>() != null)
                        target.GetComponent<EnemyHP>().ChangeHP(-Mathf.Abs(damage));

                    castingOrigin = (Vector2)target.transform.position;
                    hitHashs[i]= target.GetHashCode();
                }
            }
        }
        lineNodes.RemoveAll(node => node == placeHolderVector);

        if (lineNodes.Count >= 2)
        {
            if (lineNodes.Count == 2)
            {
                lineNodes.Add(lineNodes[1]);
                lineNodes[1] = lineNodes[0] + (lineNodes[1] - lineNodes[0]) / 2;
            }
            
            lineRenderer.gameObject.SetActive(true);
            lineRenderer.material = material;
            lineRenderer.widthMultiplier = lineWidht;
            lineRenderer.positionCount = lineNodes.Count;
            for (int i = 0; i < lineNodes.Count; i++)
            {
                lineRenderer.SetPosition(i, lineNodes[i]);
            }
            StartCoroutine(DestroyLine(lineRenderer.gameObject, visualDuration));
        }
    }

    IEnumerator DestroyLine(GameObject line, float time)
    {
        yield return new WaitForSeconds(time);

        line.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position, radiusFromPlayer);
        Gizmos.DrawWireSphere(transform.position, radiusFromLastHit);

    }
}