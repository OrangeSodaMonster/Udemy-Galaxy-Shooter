using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;

public class SnakeBossMain : MonoBehaviour
{
    [Tooltip("Quanto menos bodyparts mais rápidos os disparos")]
    [SerializeField] Vector2 minMaxShootTime = new(0.9f,2.1f);
    [SerializeField] float defaultSpeed = 3.5f;
    [SerializeField] float catchUpTime = 1;
    [SerializeField] float deltaOffset = .005f;
    [SerializeField] List<SplineAnimate> parts = new List<SplineAnimate>();
    [Space]
    [SerializeField] LineRenderer lineRenderer1;
    [SerializeField] List<Transform> linePoints1;
    [SerializeField] LineRenderer lineRenderer2;
    [SerializeField] List<Transform> linePoints2;
    [Space]
    [SerializeField] List<GameObject> objsToEnableOnTurnOn;
    [SerializeField] GameObject headShield;

    List<SubShooter> shooters = new();
    Transform player;
    float currentShootTime = float.MaxValue;
    float shootTimer = 0;
    float startingShooters;
    bool hasTail = true;
    float tailExtraOffset;
    List<float> offsets = new List<float>();
    List<float> targetOffsets = new List<float>();
    float catchUpSpeed;
    Vector3[] linePos1;
    Vector3[] linePos2;
    float stopAttacksDistance;
    bool isOn = false;

    private void Start()
    {
        for (int i = 0; i < parts.Count; i++)
        {
            parts[i].MaxSpeed = defaultSpeed;

            offsets.Add(parts[i].StartOffset);
            targetOffsets = new List<float>(offsets);
        }
        StartCoroutine(PauseParts());

        catchUpSpeed = deltaOffset*Time.deltaTime/catchUpTime;
        tailExtraOffset = -(offsets[offsets.Count-2] - offsets[offsets.Count-1])+deltaOffset;

        player = FindObjectOfType<PlayerMove>().transform;

        GetComponentsInChildren<SubShooter>(true, shooters);
        currentShootTime = minMaxShootTime.x;
        startingShooters = shooters.Count;

        stopAttacksDistance = EnemySpawner.Instance.SpawnZoneRadius * 1.5f;
        stopAttacksDistance *= stopAttacksDistance;
    }

    IEnumerator PauseParts()
    {
        yield return null;        

        for (int i = 0;i < parts.Count;i++)
        {
            parts[i].Pause();
        }
    }

    private void Update()
    {
        for (int i = 0; i < parts.Count; i++)
        {
            if (i == 0) continue;

            if (parts[i].StartOffset < targetOffsets[i])
            {
                parts[i].StartOffset += catchUpSpeed;
            }
        }

        currentShootTime = Mathf.Lerp(minMaxShootTime.y, minMaxShootTime.x, shooters.Count/startingShooters);
        shootTimer += Time.deltaTime;
        if (shootTimer >= currentShootTime && isOn)
        {
            PickShooter();
            shootTimer = 0;
        }
    }
    private void LateUpdate()
    {
        UpdateLine();        
    }

    [Button]
    public void PickShooter()
    {
        if (GameStatus.IsGameover || GameStatus.IsStageClear) return;

        if (Vector2.SqrMagnitude(player.position - parts[0].transform.position) > stopAttacksDistance)
            return;

        SubShooter bestShooterSoFar = null;
        float bestDotSoFar = -1;
        for(int i = 0;i < shooters.Count;i++)
        {
            if (!shooters[i].gameObject.activeInHierarchy)
            {
                shooters.Remove(shooters[i]);
                i--;
                continue;
            }
            if (shooters[i].IsShooting) continue;
            if (bestShooterSoFar == null)
            {
                bestShooterSoFar = shooters[i];
                bestDotSoFar = Vector2.Dot(shooters[i].transform.up, (player.position - shooters[i].transform.position).normalized);
                continue;
            }

            float dot = Vector2.Dot(shooters[i].transform.up, (player.position - shooters[i].transform.position).normalized);
            if (dot > bestDotSoFar)
            {
                bestDotSoFar = dot;
                bestShooterSoFar = shooters[i];
            }
        }

        if(bestDotSoFar >= 0.8f)
            bestShooterSoFar?.StartShoot();
    }

    public void RemovePart(SplineAnimate splineAnimate, bool isTail = false)
    {
        int removedIndex = parts.IndexOf(splineAnimate);
        if (isTail) hasTail = false;

        for (int i = removedIndex; i < parts.Count; i++)
        {
            targetOffsets[i] = offsets[i-1];

            if (i == parts.Count-1 && hasTail) targetOffsets[i] += tailExtraOffset;
        }

        parts.Remove(splineAnimate);
        targetOffsets.RemoveAt(removedIndex);   
        linePoints1.RemoveAt(removedIndex);
        linePoints2.RemoveAt(removedIndex);

        if(parts.Count == 1)
        {
            parts[0].GetComponent<PolygonCollider2D>().enabled = true;
            headShield.SetActive(false);
        }    
        
        if(parts.Count == 0)
        {
            this.enabled = false;
        }

    }

    [Button]
    public void UpdateLine()
    {
        lineRenderer1.positionCount = linePoints1.Count;
        linePos1 = new Vector3[linePoints1.Count];
        for(int i = 0; i < linePoints1.Count; i++)
        {
            linePos1[i] = linePoints1[i].position - transform.position;
        }
        lineRenderer1.SetPositions(linePos1);

        lineRenderer2.positionCount = linePoints2.Count;
        linePos2 = new Vector3[linePoints2.Count];
        for (int i = 0; i < linePoints2.Count; i++)
        {
            linePos2[i] = linePoints2[i].position - transform.position;
        }
        lineRenderer2.SetPositions(linePos2);
    }

    [Button]
    public void TurnBossOn()
    {
        for(int i = 0;i < parts.Count;i++)
        {
            parts[i].Play();
            parts[i].GetComponent<ChangeMat>()?.ChangeMaterial();
            parts[i].GetComponent<EnemyHP>().enabled = true;
        }
        parts[0].GetComponent<ObjectiveSpawnArrow>().enabled = true;
        parts[0].GetComponent<SpawnDroneFromShip>().enabled = true;
        parts[0].GetComponent<PolygonCollider2D>().enabled = false;

        for(int i = 0;i < objsToEnableOnTurnOn.Count;i++)
        {
            objsToEnableOnTurnOn[i].SetActive(true);
        }
        headShield.SetActive(true);

        shootTimer = 0;
        isOn = true;
    }


}
