using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuadrantDealer : MonoBehaviour
{
    public List<QuadrantDealer> Neighbors = new List<QuadrantDealer>();

    public List<ObjSpotScript> Spots;

    private void Awake()
    {
        PopulateList();
    }

    private void OnValidate()
    {
        PopulateList();
    }


    public void PopulateList()
    {
        transform.GetComponentsInChildren(Spots);
    }

    public QuadrantDealer GetNeighbor()
    {
        List<QuadrantDealer> validNeighbors = Neighbors;

        for(int i = 0; i < SurvivalManager.RecentQuadrants.Count; i++)
        {
            if (validNeighbors.Contains(SurvivalManager.RecentQuadrants[i]))
                validNeighbors.Remove(SurvivalManager.RecentQuadrants[i]);
        }

        QuadrantDealer newQuadrant = validNeighbors[Random.Range(0, Neighbors.Count)];
        return newQuadrant;
    }
}
