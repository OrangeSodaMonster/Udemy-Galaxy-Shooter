using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage20Cage : MonoBehaviour
{
    [SerializeField] List<GameObject> objectsToTurnOn;
    [SerializeField, Tooltip("Conduits only")] Material offMat;
    [SerializeField, Tooltip("Conduits only")] Color offColor;
    [SerializeField] List<SpriteRenderer> conduits;

    List<ChangeMat> changeMats = new();
    List<SpawnerScript> spawners = new();
    List<LineRenderer> lines = new();

    Stage20Script stageScript;
    EnemySpawner enemySpawner;
    RareSpawnScript rareSpawner;

    private void Awake()
    {
        GetComponentsInChildren<ChangeMat>(true, changeMats);
        GetComponentsInChildren<SpawnerScript>(true, spawners);
        GetComponentsInChildren<LineRenderer>(true, lines);
    }

    private void Start()
    {
        for (int i = 0; i < objectsToTurnOn.Count; i++)
        {
            objectsToTurnOn[i].SetActive(false);
        }

        stageScript = FindObjectOfType<Stage20Script>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        rareSpawner = FindObjectOfType<RareSpawnScript>();
    }

    [Button]
    public void TurnOnCage()
    {
        for (int i = 0; i < objectsToTurnOn.Count; i++)
        {
            objectsToTurnOn[i].SetActive(true);
        }

        for (int i = 0; i < changeMats.Count; i++)
        {
            changeMats[i].ChangeMaterial();
        }

        for (int i = 0; i < spawners.Count; i++)
        {
            spawners[i].CanSpawn = true;
        }

        enemySpawner.enabled = false;
        rareSpawner.enabled = false;
        stageScript.CanSpawnEnemies = false;
    }

    [Button]
    public void TurnOffCage()
    {
        for (int i = 0; i < objectsToTurnOn.Count; i++)
        {
            objectsToTurnOn[i].SetActive(false);
        }

        for (int i = 0; i < lines.Count; i++)
        {
            lines[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < conduits.Count; i++)
        {
            conduits[i].color = offColor;
            conduits[i].material = offMat;
        }

        for (int i = 0; i < changeMats.Count; i++)
        {
            changeMats[i].ReturnOriginalMaterial();
        }

        enemySpawner.enabled = true;
        rareSpawner.enabled = true;
        stageScript.CanSpawnEnemies = true;
    }
}
