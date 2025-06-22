using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuadrantDealer : MonoBehaviour
{
    public List<QuadrantDealer> Neighbors = new List<QuadrantDealer>();
    public List<ObjSpotScript> Spots;
    public List<SpawnerPointScript> SpawnerSpots;
    public Transform BossPoint;

    private IEnumerator Start()
    {
        yield return null;
        PopulateList();
    }

    private void OnValidate()
    {
        PopulateList();
    }

    private void OnEnable()
    {
        StartCoroutine(Waiter());

        IEnumerator Waiter()
        {
            yield return null;

            BossPoint = GetComponentInChildren<BossPoint>().transform;
        }
    }

    [Button]
    public void PopulateList()
    {
        transform.GetComponentsInChildren(Spots);
        transform.GetComponentsInChildren(SpawnerSpots);
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
