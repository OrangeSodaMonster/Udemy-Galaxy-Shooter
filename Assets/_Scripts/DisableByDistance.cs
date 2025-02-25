using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableByDistance : MonoBehaviour
{
    [Tooltip("Base: SpawnZoneRadius * 1.5f")]
    [SerializeField, Range(1,10)] float distMultiplier = 1;
    public float DistMultiplier => distMultiplier;
    [SerializeField] bool autoCheck = true;

    Transform player;
    float deSpawnZoneRadius;
    public float DeSpawnRadius => deSpawnZoneRadius;
    WaitForSeconds wait = new WaitForSeconds(.8f);
    Vector3 playerPos = new();

    private void Awake()
    {
        player = FindAnyObjectByType<PlayerMove>()?.transform;
    }

    void OnEnable()
    {
        if (GameStatus.IsGameover || GameStatus.IsStageClear) player = null;
        deSpawnZoneRadius = EnemySpawner.Instance.SpawnZoneRadius * 1.5f;
        deSpawnZoneRadius *= deSpawnZoneRadius;
        deSpawnZoneRadius *= distMultiplier;

        if(autoCheck)
            StartCoroutine(DistanceCheckFrequency());
    }

    IEnumerator DistanceCheckFrequency()
    {
        while (true)
        {
            yield return wait;

            if(CheckIfTooFar())
                gameObject.SetActive(false);
        }
    }

    public bool CheckIfTooFar()
    {
        playerPos = player != null ? player.position : EnemySpawner.Instance.PlayerLastPos;
        if (Vector2.SqrMagnitude(transform.position - playerPos) > deSpawnZoneRadius)
        {
            return true;            
        }
        return false;
    }
    public bool CheckIfTooFar(float distance)
    {
        playerPos = player != null ? player.position : EnemySpawner.Instance.PlayerLastPos;
        if (Vector2.SqrMagnitude(transform.position - playerPos) > distance)
        {
            return true;
        }
        return false;
    }

    [SerializeField] bool drawGizmos = false;
    private void OnDrawGizmosSelected()
    {
        if(!drawGizmos) return;

        Gizmos.color = Color.red;
        deSpawnZoneRadius = FindObjectOfType<EnemySpawner>().SpawnZoneRadius * 1.5f * distMultiplier;
        Debug.Log(deSpawnZoneRadius);
        Gizmos.DrawWireSphere(transform.position, deSpawnZoneRadius);
    }
}