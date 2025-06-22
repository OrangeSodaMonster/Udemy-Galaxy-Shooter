using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventAddToSpawnAround : MonoBehaviour
{
    [SerializeField] List<GameObject> objToSpawnAround;

    [Button]
    public void CallAddToSpawnAround()
    {
        for (int i = 0; i < objToSpawnAround.Count; ++i)
        {
            SurvivalObjectiveDealer.ObjectsToSpawnAround.Add(objToSpawnAround[i]);
        }
    }
}
