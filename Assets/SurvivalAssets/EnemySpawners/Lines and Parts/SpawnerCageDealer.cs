using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnerCageDealer : MonoBehaviour
{
    [SerializeField, Range(1, 10)] int stageToActivate;
    [SerializeField, Range(0,4)] int amountToActivate = 2;
    [SerializeField] List<GameObject> spawnerCages = new List<GameObject>();         
    [SerializeField, ReadOnly] List<GameObject> spawnerCagesAvaliable = new List<GameObject>(); 
    
    static List<SpawnerPointScript> _usedSpots = new List<SpawnerPointScript>();
    List<SpawnerPointScript> avaliableSpots = new();

    void Start()
    {
        SurvivalManager.OnSectionChange.AddListener(CallSetSpawners);
    }

    void CallSetSpawners()
    {
        StartCoroutine(GetSpawnerWaiter());

        IEnumerator GetSpawnerWaiter()
        {
            yield return null;
            SetSpawners();
        }
    }    

    void SetSpawners()
    {
        if (SurvivalManager.CurrentSection + 1 == stageToActivate)
        {
            for (int i = 0; i < amountToActivate; i++)
            {
                Vector3 pos = GetSpotPos();
                if(pos == Vector3.zero) return;

                GameObject spawn = GetSpawner();
                spawn.transform.position = pos;
                spawn.transform.rotation = GetRotation();
                spawn.SetActive(true);
            }
        }
    }

    GameObject GetSpawner()
    {
        spawnerCagesAvaliable.Clear();
        for(int i = 0;i < spawnerCages.Count; i++)
        {
            if(!spawnerCages[i].activeSelf)
                spawnerCagesAvaliable.Add(spawnerCages[i]);
        }

        int index = Random.Range(0, spawnerCagesAvaliable.Count);
        return spawnerCagesAvaliable[index];
    }

    Vector3 GetSpotPos()
    {
        avaliableSpots.Clear();
        avaliableSpots = new(SurvivalManager.CurrentQuadrant.SpawnerSpots);
        for(int i = 0; i < _usedSpots.Count; i++)
        {
            avaliableSpots.Remove( _usedSpots[i]);
        }

        if(avaliableSpots.Count == 0) return Vector3.zero;

        int index = Random.Range(0, avaliableSpots.Count);
        Transform spot = avaliableSpots[index].transform;

        _usedSpots.Add(avaliableSpots[index]);

        return spot.position;
    }

    Quaternion GetRotation()
    {
        float zRot = 22.5f * Random.Range(0, 4);

        return Quaternion.Euler(0, 0, zRot);
    }

    [Button]
    public void PopulateList()
    {
        spawnerCages.Clear();
        foreach (Transform t in transform)
        {
            spawnerCages.Add(t.gameObject);
        }
    }
}
