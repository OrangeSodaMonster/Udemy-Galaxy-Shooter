using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnAround : MonoBehaviour
{
    [SerializeField] Transform originTransform;
    [SerializeField] float rangeToSpawn = 0;
    [SerializeField] List<GameObject> objectsToSpawn = new();

    PoolRefs poolRef;

    private void Start()
    {
        poolRef = EnemySpawner.Instance.gameObject.GetComponent<PoolRefs>();
    }

    [Button("SpawnAround")]
    public void Spawn(Vector3? origin = null, List<GameObject> objects = null, float range = 0)
    {
        if(origin == null) origin = originTransform.position;
        if (origin == null)
        {
            Debug.Log("<color=red>NO ORIGIN TO SPAWN AROUND </color>");
            return;
        }

        if(objects == null) objects = objectsToSpawn;
        if(objects.Count == 0)
        {
            Debug.Log("<color=red>NO OBJECTS TO SPAWN AROUND </color>");
            return;
        }

        if (range == 0) range = rangeToSpawn;
        if (range == 0)
        {
            Debug.Log("<color=red>NO RANGE TO SPAWN AROUND </color>");
            return;
        }

        float degrees = 360 / objects.Count;
        float degreesVar = degrees * 0.2f; // Only Sum (one direction var)
        float lastDegree = UnityEngine.Random.Range(0f, 360f);

        for (int i = 0; i < objects.Count; i++)
        {
            float degreeRad = lastDegree * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Cos(degreeRad), Mathf.Sin(degreeRad), 0);
            Vector3 pos = origin.Value + direction * range;

            lastDegree += UnityEngine.Random.Range(0f, degreesVar) + degrees;
            GameObject objToPlace = null;
            try
            {
                objToPlace = poolRef.Poolers[objects[i]].GetPooledGameObject();
            }
            catch(Exception) { }

            if(objToPlace == null)
            {                
                try
                {
                    objToPlace = ObjPools.PoolObjective(objects[i]);
                }
                catch (Exception) { }
            }

            if(objToPlace == null)
            {
                //Instantiate(objects[i], pos, Quaternion.identity);
                Debug.Log("<color=yellow>Could not pool object to place around</color>");
                return;
            }
            else
            {
                objToPlace.transform.position = pos;
                objToPlace.SetActive(true);
            }
        }
    }
}
