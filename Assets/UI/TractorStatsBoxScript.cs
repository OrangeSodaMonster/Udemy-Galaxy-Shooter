using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TractorStatsBoxScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI rangeText;
    [SerializeField] TextMeshProUGUI forceText;
    [SerializeField] Color normalColor = Color.white;
    [SerializeField] Color bonusColor = Color.cyan;
    [SerializeField] Color disabledColor = Color.gray;

    [SerializeField] GameObject statsBox;
    [SerializeField] GameObject visual;

    private void OnEnable()
    {
        SetValues(PlayerStats.Instance.Ship.Tractor);
    }

    void SetValues(PlayerStats.TractorStats stats)
    {
        SetEnabled(stats);
        if (!stats.Unlocked) return;

        rangeText.text = $"{stats.CurrentRange}";
        forceText.text = $"{stats.CurrentForce}";

        if (stats.ForceDisable)
        {
            rangeText.color = disabledColor;
            forceText.color = disabledColor;
        }
        else
        {
            if (stats.IsRangeBonus)
                rangeText.color = bonusColor;
            else
                rangeText.color = normalColor;

            if (stats.IsForceBonus)
                forceText.color = bonusColor;
            else
                forceText.color = normalColor;            

    }

        void SetEnabled(PlayerStats.TractorStats stats)
        {
            statsBox.SetActive(stats.Unlocked);
            visual.SetActive(stats.Unlocked);
        }
    }
}
