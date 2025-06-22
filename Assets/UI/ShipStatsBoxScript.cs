using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShipStatsBoxScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI turningText;
    [SerializeField] TextMeshProUGUI linInertiaText;
    [SerializeField] TextMeshProUGUI angInertiaText;
    [SerializeField] TextMeshProUGUI healIntervalText;
    [SerializeField] Color normalColor = Color.white;
    [SerializeField] Color bonusColor = Color.cyan;

    private void OnEnable()
    {
        SetValues(PlayerStats.Instance.Ship);
    }
    void SetValues(PlayerStats.ShipStats stats)
    {
        hpText.text = $"{stats.CurrentHP}/{stats.CurrentMaxHP}";
        speedText.text = $"{stats.CurrentMaxSpeed}";
        turningText.text = $"{stats.CurrentMaxTurningSpeed}º";
        linInertiaText.text = $"{stats.CurrentLinearInertia}s";
        angInertiaText.text = $"{stats.CurrentAngularInertia}s";
        healIntervalText.text = $"{PlayerStats.Instance.Drones.CurrentHealInterval}s";

        if (stats.IsSpeedBonus || stats.IsTurningSpeedBonus)
        {
            speedText.color = bonusColor;
            turningText.color = bonusColor;
            linInertiaText.color = bonusColor;
            angInertiaText.color = bonusColor;
        }
        else
        {
            speedText.color = normalColor;
            turningText.color = normalColor;
            linInertiaText.color = normalColor;
            angInertiaText.color = normalColor;
        }

        if (PlayerStats.Instance.Drones.IsHealBonus)
            healIntervalText.color = bonusColor;
        else
            healIntervalText.color = normalColor;
        
    }
}
