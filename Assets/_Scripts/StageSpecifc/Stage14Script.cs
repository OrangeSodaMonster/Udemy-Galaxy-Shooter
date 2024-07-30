using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage14Script : MonoBehaviour
{
    [SerializeField] List<Transform> cageNodes;
    [SerializeField] LineRenderer cageLine;


    void Start()
    {
        cageLine.positionCount = cageNodes.Count;
        Vector3[] nodesPos = new Vector3[cageNodes.Count];
        for (int i = 0; i < cageNodes.Count; i++)
        {
            nodesPos[i] = cageNodes[i].position - cageLine.transform.position;
        }
        cageLine.SetPositions(nodesPos);
    }

    [Button]
    public void TurnCageOn()
    {
        for(int i = 0;i < cageNodes.Count; i++)
        {
            cageNodes[i].GetComponent<Stage14_Nodes>().TurnOn();
        }
        cageLine.gameObject.SetActive(true);
    }
}
