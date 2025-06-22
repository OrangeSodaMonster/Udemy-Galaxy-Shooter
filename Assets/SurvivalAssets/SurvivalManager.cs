using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.CoreUtils;

[Serializable]
public enum SurvivalState
{
    Spawning,
    Bonus,
    Boss,
}

public class SurvivalManager : MonoBehaviour
{
    public static SurvivalState SurvivalState = SurvivalState.Spawning;
    public static CombatLog CombatLog;
    public static ScoreHolder ScoreHolder;
    public static int CurrentSection = 0;
    public static int Score = 0;
    public static float TotalTime = 0;
    public static bool IsInEndEventTime;
    public static QuadrantDealer CurrentQuadrant;
    public static List<QuadrantDealer> RecentQuadrants = new();
    public static UnityEvent OnSectionChange = new();
    public static bool IsWaitingEndEvent = false;
    public static bool IsBonusPickUpEnabled = false;
    //public static bool IsNextSectionReady = false;
    public static UnityEvent OnBonusAsteroidSpawn = new();
    public static UnityEvent OnBonusAsteroidDestroyed = new();
    public static UnityEvent OnBossDestroyed = new();
    public static float ExtraMetalDropPerc = 30;
    public static float ExtraRareMetalDropPerc = 20;

    void OnEnable()
    {
        CombatLog = FindObjectOfType<CombatLog>();
        ScoreHolder = FindObjectOfType<ScoreHolder>();

        for (int i = 0; i < 7; i++)
        {
            RecentQuadrants.Add(null);
        }        
    }

    private void LateUpdate()
    {
        if (SurvivalState == SurvivalState.Boss && !IsWaitingEndEvent && !IsBonusPickUpEnabled)
        {
            ChangeSection();
        }

        IsBonusPickUpEnabled = false ;
    }

    void ChangeSection()
    {
        StartCoroutine(Wait());

        IEnumerator Wait()
        {
            yield return null;

            SurvivalManager.CurrentSection++;
            //SurvivalManager.IsNextSectionReady = false;
            OnSectionChange?.Invoke();
            SurvivalManager.ChangeQuadrant();
            SurvivalObjectiveDealer.EraseLastObj();
            SurvivalState = SurvivalState.Spawning;
            Debug.Log("<color=cyan>STATE: Spawning</color>");
            Debug.Log($"CHANGE SECTION >>> {CurrentSection + 1}");
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
