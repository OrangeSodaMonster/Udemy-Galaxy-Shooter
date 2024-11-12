using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ScoreHolder : MonoBehaviour
{
    [SerializeField] float nonAsteroidScoreMultiplier = 5;
    [SerializeField] int ObjectiveCompleteScore = 1000;
    [ShowInInspector, ReadOnly]public int Score { get; private set;}
    

    public void UpdateScoreKilledEnemy(int maxHP, bool isAsteroid)
    {
        if (isAsteroid)
            Score += Mathf.Abs(maxHP);
        else
            Score += (int)(Mathf.Abs(maxHP) * nonAsteroidScoreMultiplier);
    }

    public void UpdateScoreObjectiveDestroid()
    {
        Score += ObjectiveCompleteScore;
    }
}
