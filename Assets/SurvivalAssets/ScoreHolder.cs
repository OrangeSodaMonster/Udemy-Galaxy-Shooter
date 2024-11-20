using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class ScoreHolder : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] float nonAsteroidScoreMultiplier = 5;
    [SerializeField] int ObjectiveCompleteScore = 1000;
    [ShowInInspector, ReadOnly]public int Score { get; private set;}

    private void OnEnable()
    {
        UpdateScoreText();
    }

    public void UpdateScoreKilledEnemy(int maxHP, bool isAsteroid)
    {
        if (isAsteroid)
            Score += Mathf.Abs(maxHP);
        else
            Score += (int)(Mathf.Abs(maxHP) * nonAsteroidScoreMultiplier);

        UpdateScoreText();
    }

    public void UpdateScoreObjectiveDestroid()
    {
        Score += ObjectiveCompleteScore;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = Score.ToString();
    }
}
