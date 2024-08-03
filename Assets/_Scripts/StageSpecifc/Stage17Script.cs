using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage17Script : MonoBehaviour
{
    [SerializeField] GameObject redShipPref;
    [SerializeField] int firstRedShipSpawnsNum = 2;
    [SerializeField] int secondRedShipSpawnsNum = 3;
    [SerializeField] GameObject redDronePref;
    [SerializeField] int firstRedDroneSpawnsNum = 1;
    [SerializeField] int secondRedDroneSpawnsNum = 2;
    [SerializeField] int thirdRedDroneSpawnsNum = 3;

    int proxTriggers = 0;
    int objCleared = 0;

    public void ProximityTrigger()
    {
        proxTriggers++;
        if(proxTriggers == 1)
        {
            for(int i = 0; i < firstRedShipSpawnsNum; i++)
            {
                EnemySpawner.Instance.SpawnEnemy(redShipPref);
            }
        }
        else
        {
            for (int i = 0; i < secondRedShipSpawnsNum; i++)
            {
                EnemySpawner.Instance.SpawnEnemy(redShipPref);
            }
        }
    }

    public void ObjCleared()
    {
        objCleared++;
        if (objCleared == 1)
        {
            for (int i = 0; i < firstRedDroneSpawnsNum; i++)
            {
                EnemySpawner.Instance.SpawnEnemy(redDronePref);
            }
        }
        else if (objCleared == 2)
        {
            for (int i = 0; i < secondRedDroneSpawnsNum; i++)
            {
                EnemySpawner.Instance.SpawnEnemy(redDronePref);
            }
        }
        else
        {
            for (int i = 0; i < thirdRedDroneSpawnsNum; i++)
            {
                EnemySpawner.Instance.SpawnEnemy(redDronePref);
            }
        }
    }





}
