using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SurvivalGameoverDealer : MonoBehaviour
{
    [SerializeField, FoldoutGroup("collected")]
    TextMeshProUGUI collectedMetalText;
    [SerializeField, FoldoutGroup("collected")]
    TextMeshProUGUI collectedRareMetalText;
    [SerializeField, FoldoutGroup("collected")]
    TextMeshProUGUI collectedEnergyCrystalText;
    [SerializeField, FoldoutGroup("collected")]
    TextMeshProUGUI collectedCondEnCrystalText;
    [Space]
    [SerializeField, FoldoutGroup("lost")]
    TextMeshProUGUI lostMetalText;
    [SerializeField, FoldoutGroup("lost")]
    TextMeshProUGUI lostRareMetalText;
    [SerializeField, FoldoutGroup("lost")]
    TextMeshProUGUI lostEnergyCrystalText;
    [SerializeField, FoldoutGroup("lost")]
    TextMeshProUGUI lostCondEnCrystalText;
    [Space]
    [SerializeField, FoldoutGroup("damageDone")]
    TextMeshProUGUI frontLasetText;
    [SerializeField, FoldoutGroup("damageDone")]
    TextMeshProUGUI spreadLasetText;
    [SerializeField, FoldoutGroup("damageDone")]
    TextMeshProUGUI lateralLasetText;
    [SerializeField, FoldoutGroup("damageDone")]
    TextMeshProUGUI backLasetText;
    [SerializeField, FoldoutGroup("damageDone")]
    TextMeshProUGUI totalLasetText;
    [SerializeField, FoldoutGroup("damageDone")]
    TextMeshProUGUI bombText;
    [SerializeField, FoldoutGroup("damageDone")]
    TextMeshProUGUI ionStreamText;
    [SerializeField, FoldoutGroup("damageDone")]
    TextMeshProUGUI drone1Text;
    [SerializeField, FoldoutGroup("damageDone")]
    TextMeshProUGUI drone2Text;
    [SerializeField, FoldoutGroup("damageDone")]
    TextMeshProUGUI drone3Text;
    [SerializeField, FoldoutGroup("damageDone")]
    TextMeshProUGUI totalDroneText;
    [SerializeField, FoldoutGroup("damageDone")]
    TextMeshProUGUI totalDamageText;

    [SerializeField, FoldoutGroup("damageBlocked")]
    TextMeshProUGUI frontShieldText;
    [SerializeField, FoldoutGroup("damageBlocked")]
    TextMeshProUGUI LeftShieldText;
    [SerializeField, FoldoutGroup("damageBlocked")]
    TextMeshProUGUI rightShieldText;
    [SerializeField, FoldoutGroup("damageBlocked")]
    TextMeshProUGUI backShieldText;
    [SerializeField, FoldoutGroup("damageBlocked")]
    TextMeshProUGUI totalShieldText;

    [SerializeField, FoldoutGroup("damageTaken")]
    TextMeshProUGUI damageTakenText;
    [SerializeField, FoldoutGroup("damageTaken")]
    TextMeshProUGUI damageHealedText;

    [SerializeField, FoldoutGroup("Scoring")]
    TextMeshProUGUI stageText;
    [SerializeField, FoldoutGroup("Scoring")]
    TextMeshProUGUI timeText;
    [SerializeField, FoldoutGroup("Scoring")]
    TextMeshProUGUI scoreText;

    [SerializeField] SelectedBonusUI selectedBonusUI;
    [Space]
    [SerializeField] Canvas statsCanvas;
    [SerializeField] PauseAndUIManager pauseAndUIManager;

    private void OnEnable()
    {
        CombatLog.Instance.SumValues();
        selectedBonusUI.UpdateIcons();
        SetCollectedTexts();
        SetLostTexts();
        SetDamageDoneTexts();
        SetDamageBlockedTexts();
        SetDamageTakenTexts();
        SetScoreTexts();
    }

    void SetCollectedTexts()
    {
        collectedMetalText.text = PickUpsLog.Instance.PickedDrops.Metal.ToString();
        collectedRareMetalText.text = PickUpsLog.Instance.PickedDrops.RareMetal.ToString();
        collectedEnergyCrystalText.text = PickUpsLog.Instance.PickedDrops.EnergyCrystal.ToString();
        collectedCondEnCrystalText.text = PickUpsLog.Instance.PickedDrops.CondensedEnergyCrystal.ToString();
    }
    void SetLostTexts()
    {
        lostMetalText.text = PickUpsLog.Instance.LostDrops.Metal.ToString();
        lostRareMetalText.text = PickUpsLog.Instance.LostDrops.RareMetal.ToString();
        lostEnergyCrystalText.text = PickUpsLog.Instance.LostDrops.EnergyCrystal.ToString();
        lostCondEnCrystalText.text = PickUpsLog.Instance.LostDrops.CondensedEnergyCrystal.ToString();
    }
    void SetDamageDoneTexts()
    {
        frontLasetText.text = CombatLog.Instance.FrontalLasersTotalDamage.ToString();
        spreadLasetText.text = CombatLog.Instance.SpreadLasersTotalDamage.ToString();
        lateralLasetText.text = CombatLog.Instance.LateralLasersTotalDamage.ToString();
        backLasetText.text = CombatLog.Instance.BackLasersTotalDamage.ToString();
        totalLasetText.text = CombatLog.Instance.LasersTotalDamage.ToString();

        bombText.text = CombatLog.Instance.BombTotalDamage.ToString();

        ionStreamText.text = CombatLog.Instance.IonStreamTotalDamage.ToString();

        drone1Text.text = CombatLog.Instance.Drone1TotalDamage.ToString();
        drone2Text.text = CombatLog.Instance.Drone2TotalDamage.ToString();
        drone3Text.text = CombatLog.Instance.Drone3TotalDamage.ToString();
        totalDroneText.text = CombatLog.Instance.DronesTotalDamage.ToString();

        totalDamageText.text = CombatLog.Instance.TotalDamageDealt.ToString();
    }
    void SetDamageBlockedTexts()
    {
        frontShieldText.text = CombatLog.Instance.FrontShieldTotalBlocked.ToString();
        LeftShieldText.text = CombatLog.Instance.LeftShieldTotalBlocked.ToString();
        rightShieldText.text = CombatLog.Instance.RightShieldTotalBlocked.ToString();
        backShieldText.text = CombatLog.Instance.BackShieldTotalBlocked.ToString();
        totalShieldText.text = CombatLog.Instance.ShieldsTotalBlocked.ToString();
    }

    void SetDamageTakenTexts()
    {
        damageTakenText.text = CombatLog.Instance.TotalDamageTaken.ToString();
        damageHealedText.text = CombatLog.Instance.TotalDamageHealed.ToString();
    }

    void SetScoreTexts()
    {
        ClockDealer();
        stageText.text = SurvivalManager.CurrentSection.ToString();
        scoreText.text = SurvivalManager.Score.ToString();

        void ClockDealer()
        {
            float TotalTime = SurvivalManager.TotalTime;

            int hour = (int)MathF.Floor(TotalTime/3600);
            int min = (int)MathF.Floor((TotalTime - (hour * 3600))/60);
            int sec = (int)MathF.Floor((TotalTime - (hour * 3600))%60);

            if (TotalTime < 3600)
                timeText.text = string.Format("{0:00}:{1:00}", min, sec);
            else
                timeText.text = string.Format("{0:00}:{1:00}:{2:00}", hour, min, sec);
        }
    }
    Canvas thisCanvas;
    void SetStatsCanvas(bool status)
    {
        if(thisCanvas == null)
            thisCanvas = GetComponent<Canvas>();

        thisCanvas.enabled = !status;
        statsCanvas.gameObject.SetActive(status);
    }

    public void EnableStatsCanvas()
    {
        SetStatsCanvas(true);
    }
    public void DisableStatsCanvas()
    {
        if(GameStatus.IsGameover)
            SetStatsCanvas(false);
        else
            pauseAndUIManager.ReturnToPauseCanvas();
    }

}
