using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage10Script : MonoBehaviour
{
	[SerializeField] GameObject firstWave;
	[SerializeField] GameObject greenDrone;
	[SerializeField] float greenCD = 9;
	[SerializeField] int greenSpawnsNumber;
	[Space]
    [SerializeField] GameObject secondWave;
    [SerializeField] GameObject yellowDrone;
	[SerializeField] float yellowCD = 13;
    [SerializeField] int yellowSpawnsNumber;
    [Space]
    [SerializeField] GameObject thirdWave;
    [SerializeField] GameObject orangeDrone;
	[SerializeField] float orangeCD = 26;
    [SerializeField] int orangeSpawnsNumber;
    [Space]
    [SerializeField] GameObject fourthWave;
    [SerializeField] GameObject redDrone;
    [SerializeField] GameObject bossPrefab;
    [Space]
    [SerializeField] int objsComplete;

    public Vector3 spawnPos;

    EnemySpawner enemySpawner;
    PoolRefs poolRefs;

    private void Start()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
        poolRefs = FindObjectOfType<PoolRefs>();

        poolRefs.CreatePoolsForObject(greenDrone, 13);
        poolRefs.CreatePoolsForObject(yellowDrone, 7);
        poolRefs.CreatePoolsForObject(orangeDrone, 4);
        poolRefs.CreatePoolsForObject(redDrone, 2);
        poolRefs.CreatePoolsForObject(bossPrefab, 1);
    }

    bool spawnDrones = false;
    float greenTimer = 0;
    float yellowTimer = 0;
    float OrangeTimer = 0;
    private void Update()
    {
        if (greenTimer >= greenCD)
        {
            SpawnDrone(greenDrone, greenSpawnsNumber);
            greenTimer = 0;
        }
        if (yellowTimer >= yellowCD)
        {
            SpawnDrone(yellowDrone, yellowSpawnsNumber);
            yellowTimer = 0;
        }
        if (OrangeTimer >= orangeCD)
        {
            SpawnDrone(orangeDrone, orangeSpawnsNumber);
            OrangeTimer = 0;
        }

        if (spawnDrones)
        {
            greenTimer += Time.deltaTime;
            yellowTimer += Time.deltaTime;
            OrangeTimer += Time.deltaTime;
        }
    }

    public void IncreaseObjCount()
    {
        objsComplete++;

        switch (objsComplete)
        {
            case 1:
                CallFirstSpawn(); break;
            case 2:
                CallSecondSpawn(); break;
            case 3:
                CallThirdSpawn(); break;
            case 4:
                CallFourthSpawn(); break;
            case 5:
                spawnDrones = true; break;
            default: break;
        }
    }

    void SpawnDrone(GameObject enemy, int number)
    {
        for (int i = 0; i < number; i++)
        {
            GameObject spawn = poolRefs.Poolers[enemy].GetPooledGameObject();
            spawn.transform.position = enemySpawner.GetSpawnPoint360();
            spawn.SetActive(true);
        }
    }

    void CallFirstSpawn()
    {
        //SpawnWave(firstSpawns, firstSpawnsNumber);
        firstWave.transform.position = spawnPos;
        firstWave.SetActive(true);
    }

    void CallSecondSpawn()
    {
        //SpawnWave(secondSpawns, secondSpawnsNumber);
        secondWave.transform.position = spawnPos;
        secondWave.SetActive(true);
    }

    void CallThirdSpawn()
    {
        //SpawnWave(thirdSpawns, thirdSpawnsNumber);
        thirdWave.transform.position = spawnPos;
        thirdWave.SetActive(true);
    }

    void CallFourthSpawn()
    {
        //SpawnWave(fourthSpawns, fourthSpawnsNumber);

        //GameObject boss = poolRefs.Poolers[bossPrefab].GetPooledGameObject();
        //boss.transform.position = enemySpawner.GetSpawnPoint360();
        //boss.SetActive(true);

        fourthWave.transform.position = spawnPos;
        fourthWave.SetActive(true);
    }


}