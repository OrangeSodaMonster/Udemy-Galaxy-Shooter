using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalManager : MonoBehaviour
{
    public static CombatLog CombatLog;
    public static ScoreHolder ScoreHolder;
    public static int CurrentSection = 0;
    public static bool IsInEndEventTime;
    public static QuadrantDealer CurrentQuadrant;
    public static List<QuadrantDealer> RecentQuadrants = new();

    void OnEnable()
    {
        CombatLog = FindObjectOfType<CombatLog>();
        ScoreHolder = FindObjectOfType<ScoreHolder>();

        for (int i = 0; i < 7; i++)
        {
            RecentQuadrants.Add(null);
        }        
    }

    static public void ChangeQuadrant()
    {
        QuadrantDealer newQuadrant = CurrentQuadrant.GetNeighbor();
        List<QuadrantDealer> oldQuadrants = RecentQuadrants;
        CurrentQuadrant = newQuadrant;
        Debug.Log($"NewQuadrant: {newQuadrant.transform.name}");

        RecentQuadrants[0] = newQuadrant;
        RecentQuadrants[1] = oldQuadrants[0];
        RecentQuadrants[2] = oldQuadrants[1];
        RecentQuadrants[3] = oldQuadrants[2];
        RecentQuadrants[4] = oldQuadrants[3];
        RecentQuadrants[5] = oldQuadrants[4];
        RecentQuadrants[6] = oldQuadrants[5];
    }

}
