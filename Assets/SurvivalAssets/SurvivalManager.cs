using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalManager : MonoBehaviour
{
    public static CombatLog CombatLog;
    public static ScoreHolder ScoreHolder;
    public static int CurrentSection = 0;
    public static bool IsInEndEventTime;


    void OnEnable()
    {
        CombatLog = FindObjectOfType<CombatLog>();
        ScoreHolder = FindObjectOfType<ScoreHolder>();
    }

}
