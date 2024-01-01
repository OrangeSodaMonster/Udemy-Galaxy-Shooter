using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IonStreamScript : MonoBehaviour
{
    public bool isIonStreamEnabled;

    [SerializeField] float radiusFromPlayer = 2;
    [SerializeField] float RadiusFromLastHit = 1.5f;
    [SerializeField] float numberOfHits = 3;
    [SerializeField] float timeBetweenActivations = 6;
    [SerializeField] float damage = 1;
    [SerializeField] float visualDuration = .3f;
    [SerializeField] LayerMask layersToHit;
    [SerializeField] LineRenderer lineRenderer;

    float timeSinceFired = float.MaxValue;
    Transform player;
    RaycastHit2D[] hits;
    float remainingJumps;
    List<Vector2> lineNodes = new List<Vector2>();

    void Start()
    {
        player = FindAnyObjectByType<PlayerMove>().transform;
    }

    void Update()
    {
        if(isIonStreamEnabled & timeSinceFired > timeBetweenActivations & Physics2D.CircleCast(player.position, radiusFromPlayer, Vector2.zero, 0, layersToHit))
        {
            FireIonStream();
            timeSinceFired = 0;
            Debug.Log("Fire Ion Stream");
        }
        timeSinceFired += Time.deltaTime;
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
                castingRadius = RadiusFromLastHit;
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
            LineRenderer line = Instantiate(lineRenderer, transform.position, Quaternion.identity, transform);
            line.positionCount = lineNodes.Count;
            for (int i = 0; i < lineNodes.Count; i++)
            {
                line.SetPosition(i, lineNodes[i]);
            }
            StartCoroutine(DestroyLine(line.gameObject, visualDuration));
        }
    }

    IEnumerator DestroyLine(GameObject line, float time)
    {
        yield return new WaitForSeconds(time);

        Destroy(line);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position, radiusFromPlayer);
        Gizmos.DrawWireSphere(transform.position, RadiusFromLastHit);

    }
}