using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DroneStatsBoxScript : MonoBehaviour
{
    [SerializeField] DroneNumber type;
    [SerializeField] TextMeshProUGUI powerText;
    [SerializeField] TextMeshProUGUI rangeText;
    [SerializeField] TextMeshProUGUI healText;
    [SerializeField] Color normalColor = Color.white;
    [SerializeField] Color bonusColor = Color.cyan;
    [SerializeField] Color disabledColor = Color.gray;

    [SerializeField] GameObject statsBox;
    [SerializeField] GameObject ShieldVisual;

    private void OnEnable()
    {
        SetValues(type);
    }

    void SetValues(DroneNumber type)
    {
        switch (type)
        {
            case DroneNumber.One:
                SetValues(PlayerStats.Instance.Drones.Drone1);
                break;
            case DroneNumber.Two:
                SetValues(PlayerStats.Instance.Drones.Drone2);
                break;
            case DroneNumber.Three:
                SetValues(PlayerStats.Instance.Drones.Drone3);
                break;
        }

        void SetValues(PlayerStats.DroneStats stats)
        {
            SetEnabled(stats);
            if (!stats.Unlocked) return;

            powerText.text = $"{stats.CurrentPower}/s";
            rangeText.text = $"{stats.CurrentRange}";
            healText.text = $"-{stats.HealIntervalSubtraction}s";

            if (stats.ForceDisable)
            {
                powerText.color = disabledColor;
                rangeText.color = disabledColor;
                healText.color = disabledColor;
            }
            else
            {
                healText.color = normalColor;

                if (stats.IsPowerBonus)
                    powerText.color = bonusColor;
                else
                    powerText.color = normalColor;

                if (stats.IsRangeBonus)
                    rangeText.color = bonusColor;
                else
                    rangeText.color = normalColor;
            }

        }

        void SetEnabled(PlayerStats.DroneStats stats)
        {
            statsBox.SetActive(stats.Unlocked);
            ShieldVisual.SetActive(stats.Unlocked);
        }
    }
}
