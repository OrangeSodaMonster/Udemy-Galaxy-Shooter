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
    public static List<QuadrantDealer> RecentQuadrants = new(3);

    void OnEnable()
    {
        CombatLog = FindObjectOfType<CombatLog>();
        ScoreHolder = FindObjectOfType<ScoreHolder>();
        RecentQuadrants.Add(null);
        RecentQuadrants.Add(null);
        RecentQuadrants.Add(null);
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
    }

}
