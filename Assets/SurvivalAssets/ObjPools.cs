using MoreMountains.Tools;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Splines.SplineInstantiate;

public class ObjPools : MonoBehaviour
{
    //[HorizontalGroup("0", .75f)]
    [SerializeField] MMSimpleObjectPooler poolerPrefab;
    public List<ObjToPool> poolDescriptions;
    public static Dictionary<GameObject, MMSimpleObjectPooler> ObjPoolers = new();

    private void OnEnable()
    {
        ObjPoolers.Clear();
        PopulatePools();
    }

    //[Button, HorizontalGroup("0")]
    void PopulatePools()
    {
        //foreach (var description in poolDescriptions)
        for(int i = 0; i < poolDescriptions.Count; i++)
        {
            if (!ObjPoolers.ContainsKey(poolDescriptions[i].Obj))
            {
                ObjPoolers.Add(poolDescriptions[i].Obj, Instantiate(poolerPrefab, transform));
                ObjPoolers[poolDescriptions[i].Obj].PoolSize = poolDescriptions[i].Count;
                ObjPoolers[poolDescriptions[i].Obj].GameObjectToPool = poolDescriptions[i].Obj;
            }
        }
    }

    static public GameObject PoolObjective(GameObject obj)
    {
        GameObject pooledObj;
        try
        {
            pooledObj = ObjPoolers[obj].GetPooledGameObject();
        }
        catch (KeyNotFoundException) { return null; }

        pooledObj.GetComponent<ReturnPoolHolder>().ReturnPool = pooledObj.transform.parent;
        return pooledObj;
    }

    private void OnValidate()
    {
        for (int i = 0; i < poolDescriptions.Count; i++)
        {
            if (poolDescriptions[i].Obj != null)
            {
                ObjToPool otp = poolDescriptions[i];
                otp.Name = otp.Obj.name;
                poolDescriptions[i] = otp;
            }
        }
    }

}
