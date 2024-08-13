using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AutoSetLinePositions : MonoBehaviour
{
    LineRenderer lineRenderer;

    [SerializeField] List<Transform> positions;

    static UnityEvent updateLines = new UnityEvent();    

    private void OnValidate()
    {
        updateLines.AddListener(UpdateLine);
        UpdateLine();
    }

    [Button]
    public void UpdateLine()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = positions.Count;

        for (int i = 0; i < positions.Count; i++)
        {
            lineRenderer.SetPosition(i, positions[i].position - transform.position);
        }

        lineRenderer.transform.rotation = Quaternion.identity;       
    }

    [Button]
    public void UpdateAllLines()
    {
        updateLines.Invoke();
    }
}
