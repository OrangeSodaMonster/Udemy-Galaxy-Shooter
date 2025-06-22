using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SpawnerScript : MonoBehaviour
{
    [SerializeField] GameObject enemyToSpawn;
    [SerializeField] VisualEffect spawnVFX;
    [SerializeField] float spawnTime = 10;
    [SerializeField] float spawnTimeVar = 2;
    [SerializeField] float minDistanceToSpawn = 25;

    public bool CanSpawn = false;

    float currentInterval = 0;
    float timer;
    float sqrDistance;
    Transform player;

    void Start()
    {
        sqrDistance = minDistanceToSpawn * minDistanceToSpawn;
        player = FindObjectOfType<PlayerMove>().transform;

        currentInterval = Random.Range(1, spawnTime*0.5f);
    }

    void Update()
    {
        if(player == null || GameStatus.IsPortal || GameStatus.IsGameover) return;
        if(CanSpawn && timer > currentInterval && Vector2.SqrMagnitude(player.position - transform.position) < sqrDistance)
        {
            EnemySpawner.Instance.SpawnEnemy(enemyToSpawn, transform.position);
            currentInterval = Random.Range(spawnTime-spawnTimeVar, spawnTime+spawnTimeVar);
            timer = 0;

            AudioManager.Instance.SpawnerSound.PlayFeedbacks();
            spawnVFX.gameObject.SetActive(true);
        }

        if (CanSpawn)
            timer += Time.deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, minDistanceToSpawn);
    }

    public void SetSpawn(bool spawn)
    {
        CanSpawn = spawn;
    }

}
