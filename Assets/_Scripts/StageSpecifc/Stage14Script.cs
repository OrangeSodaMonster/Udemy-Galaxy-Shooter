using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage14Script : MonoBehaviour
{
    [SerializeField] List<Transform> cageNodes;
    [SerializeField] LineRenderer cageLine;

    EnemySpawner enemySpawner;

    void Start()
    {
        cageLine.positionCount = cageNodes.Count;
        Vector3[] nodesPos = new Vector3[cageNodes.Count];
        for (int i = 0; i < cageNodes.Count; i++)
        {
            nodesPos[i] = cageNodes[i].position - cageLine.transform.position;
        }
        cageLine.SetPositions(nodesPos);

        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    [Button]
    public void TurnCageOn()
    {
        for(int i = 0;i < cageNodes.Count; i++)
        {
            cageNodes[i].GetComponent<ChangeMat>().ChangeMaterial();
        }
        cageLine.gameObject.SetActive(true);
    }

    public void TurnCageOff()
    {
        for (int i = 0; i < cageNodes.Count; i++)
        {
            cageNodes[i].GetComponent<ChangeMat>().ReturnOriginalMaterial();
        }
        cageLine.gameObject.SetActive(false);
    }

    public void DisableSpawn()
    {
        enemySpawner.GetComponent<RareSpawnScript>().enabled = false;
        enemySpawner.enabled = false;
    }

    public void ReEnableSpawn()
    {        
        enemySpawner.enabled = true;
        enemySpawner.GetComponent<RareSpawnScript>().enabled = true;
    }
}
