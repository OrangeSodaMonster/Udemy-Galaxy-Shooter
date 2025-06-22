using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using TMPro;
using UnityEngine;

public class ScoreHolder : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] float nonAsteroidScoreMultiplier = 5;
    [SerializeField] int ObjectiveCompleteScore = 1000;
    [SerializeField, Tooltip("Times the section complete (e.g. 5000 x 5")]
    int SectionCompleteScore = 5000;
    //[ShowInInspector, ReadOnly]public int Score { get; private set;}

    private void OnEnable()
    {
        UpdateScoreText();
        SurvivalManager.OnSectionChange.AddListener(UpdateScoreSectionChange);
    }

    public void UpdateScoreKilledEnemy(int maxHP, bool isAsteroid)
    {
        if (isAsteroid)
            SurvivalManager.Score += Mathf.Abs(maxHP);
        else
            SurvivalManager.Score += (int)(Mathf.Abs(maxHP) * nonAsteroidScoreMultiplier);

        UpdateScoreText();
    }

    public void UpdateScoreObjectiveDestroid()
    {
        SurvivalManager.Score += ObjectiveCompleteScore;
        UpdateScoreText();
    }

    void UpdateScoreSectionChange()
    {
        StartCoroutine(Waiter());

        IEnumerator Waiter()
        {
            yield return null;

            SurvivalManager.Score += SurvivalManager.CurrentSection * SectionCompleteScore;
            //Debug.Log($"Section Change Score = {SurvivalManager.CurrentSection} * {SectionCompleteScore}");
        }
    }

    void UpdateScoreText()
    {
        scoreText.text = SurvivalManager.Score.ToString();
    }
}
