using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnObjToPool : MonoBehaviour
{
    public List<ReturnPoolHolder> returnPoolHolders = new List<ReturnPoolHolder>();
   
    public void ReturnToPool()
    {
        returnPoolHolders.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            returnPoolHolders.Add(transform.GetChild(i).GetComponent<ReturnPoolHolder>());
        }

        for (int i = 0; i < returnPoolHolders.Count; i++)
        {
            returnPoolHolders[i].ReturnToPool();
        }
    }
}
