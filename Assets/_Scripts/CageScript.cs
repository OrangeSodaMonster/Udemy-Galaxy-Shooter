using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageScript : MonoBehaviour
{
    [SerializeField] bool disableValidate = false;
    [SerializeField] float radius = 37;
    [SerializeField] bool manualRotationOfNodes = false;
    [SerializeField] LineRenderer lineRenderer;
    public LineRenderer CageLine {  get => lineRenderer; }
    [SerializeField] List<Transform> nodes;
    public List<Transform> Nodes {  get => nodes; }
    [SerializeField] List<BoxCollider2D> colliders;
    public List<BoxCollider2D> Colliders {  get => colliders; }

    void Start()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>(true);
    }


    private void OnValidate()
    {
        if(disableValidate) return;
        if (lineRenderer == null) return;

        lineRenderer = GetComponentInChildren<LineRenderer>();
        float parentAngle = transform.rotation.eulerAngles.z;

        lineRenderer.positionCount = nodes.Count;        

        for (int i = 0; i < nodes.Count; i++)
        {
            float angle = (360/(nodes.Count)) * (i) + parentAngle;

            if(!manualRotationOfNodes)
                nodes[i].rotation = Quaternion.Euler(0, 0, angle
                    );
            //Debug.Log(angle);
            angle *= Mathf.Deg2Rad;
            nodes[i].position = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius + (Vector2)transform.position;
            //Debug.Log(Mathf.Cos(angle) + " " + Mathf.Sin(angle));

            lineRenderer.SetPosition(i, nodes[i].position - transform.position);
            lineRenderer.transform.rotation = Quaternion.identity;            
        }

        // Colliders
        for (int i = 0; i < nodes.Count; i++)
        {
            //Pos
            Vector2 collPos = new();
            if (i == nodes.Count - 1)
                collPos = (nodes[0].position) - (nodes[i].position);
            else
                collPos = (nodes[i+1].position) - (nodes[i].position);

            float distance = collPos.magnitude;
            collPos = collPos * 0.5f + (Vector2)(nodes[i].position);
            colliders[i].transform.position = collPos;

            //Rot
            float angle = (360/(nodes.Count)) * (i) + parentAngle;
            float halfAngle = (180/(nodes.Count));
            colliders[i].transform.rotation = Quaternion.Euler(0, 0, angle + halfAngle);

            //Size
            Vector2 size = colliders[i].size;
            size.y = distance;
            colliders[i].size = size;
        }
    }
}
