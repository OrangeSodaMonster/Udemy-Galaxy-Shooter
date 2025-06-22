using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSpawnRare : MonoBehaviour
{
    [Tooltip("Asteroids only")]
    [SerializeField] AsteroidMove Spawn;

    [Button]
    public void SpawnRare()
    {
        RareSpawnScript.Instance.SpawnRare(Spawn.gameObject);
    }
}
