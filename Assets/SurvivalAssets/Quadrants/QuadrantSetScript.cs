using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadrantSetScript : MonoBehaviour
{
    [SerializeField] List<QuadrantDealer> edgeQuadrantsPos = new();
    [SerializeField] List<QuadrantDealer> cornerQuadrantsPos = new();
    [Space]
    [SerializeField] List<GameObject> possibleEdgeQuadrants = new();
    [SerializeField] List<GameObject> possibleCornerQuadrants = new();

    void Start()
    {
        SetQuadrants(edgeQuadrantsPos, possibleEdgeQuadrants);
        SetQuadrants(cornerQuadrantsPos, possibleCornerQuadrants);
    }

    void SetQuadrants(List<QuadrantDealer> positions, List<GameObject> possibleQuadrants)
    {
        for(int i = 0; i < 4; i++)
        {
            int index = Random.Range(0, possibleQuadrants.Count);
            int rotation = Random.Range(0, 4);

            Instantiate(possibleQuadrants[index], -positions[i].transform.position, Quaternion.Euler(0, 0, 90*rotation), positions[i].transform);
            possibleQuadrants.RemoveAt(index);
        }
    }
}
