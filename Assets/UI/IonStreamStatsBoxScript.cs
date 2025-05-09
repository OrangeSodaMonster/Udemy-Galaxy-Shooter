using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IonStreamStatsBoxScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI powerText;
    [SerializeField] TextMeshProUGUI intervalText;
    [SerializeField] TextMeshProUGUI rangeText;
    [SerializeField] TextMeshProUGUI hitNumberText;
    [SerializeField] Color normalColor = Color.white;
    [SerializeField] Color bonusColor = Color.cyan;
    [SerializeField] Color disabledColor = Color.gray;

    [SerializeField] GameObject statsBox;
    [SerializeField] GameObject Visual;

    private void OnEnable()
    {
        SetValues(PlayerStats.Instance.IonStream);
    }
    void SetValues(PlayerStats.IonStreamStats stats)
    {
        SetEnabled(stats);
        if (!stats.Unlocked) return;

        powerText.text = $"{stats.CurrentPower}";
        intervalText.text = $"{stats.CurrentInterval}s";
        rangeText.text = $"{Mathf.Round(stats.CurrentPlayerRange*10f)/10f}/{Mathf.Round(stats.CurrentHitRange*10f)/10f}";
        hitNumberText.text = $"{stats.CurrentHitNumber}";

        if (stats.ForceDisable)
        {
            powerText.color = disabledColor;
            rangeText.color = disabledColor;
            intervalText.color = disabledColor;
            hitNumberText.color = disabledColor;
        }
        else
        {
            if (stats.IsPowerBonus)
                powerText.color = bonusColor;
            else
                powerText.color = normalColor;

            if (stats.IsCadencyBonus)
                intervalText.color = bonusColor;
            else
                intervalText.color = normalColor;

            if (stats.IsRangeBonus)
                rangeText.color = bonusColor;
            else
                rangeText.color = normalColor;
    }

        void SetEnabled(PlayerStats.IonStreamStats stats)
        {
            statsBox.SetActive(stats.Unlocked);
            Visual.SetActive(stats.Unlocked);
        }
    }
}
