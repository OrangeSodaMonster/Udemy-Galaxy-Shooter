using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LaserStatsBoxScript : MonoBehaviour
{
    [SerializeField] LaserType type;
    [SerializeField] TextMeshProUGUI powerText;
    [SerializeField] TextMeshProUGUI intervalText;
    [SerializeField] Color normalColor = Color.white;
    [SerializeField] Color bonusColor = Color.cyan;
    [SerializeField] Color disabledColor = Color.gray;

    [SerializeField] GameObject statsBox;
    [SerializeField] GameObject laserVisual;

    private void OnEnable()
    {
        SetValues(type);
    }

    void SetValues(LaserType type)
    {
        switch (type)
        {
            case LaserType.Frontal:
                SetLaserValues(PlayerStats.Instance.Lasers.FrontLaser);
                break;
            case LaserType.Spread:
                SetLaserValues(PlayerStats.Instance.Lasers.SpreadLaser);
                break;
            case LaserType.Lateral:
                SetLaserValues(PlayerStats.Instance.Lasers.SideLaser);
                break;
            case LaserType.Back:
                SetLaserValues(PlayerStats.Instance.Lasers.BackLaser);
                break;
        }

        void SetLaserValues(PlayerStats.LaserStats stats)
        {
            SetEnabled(stats);
            if(!stats.Unlocked) return;

            powerText.text = $"{stats.CurrentPower}";
            intervalText.text = $"{stats.CurrentInterval}s";

            if (stats.ForceDisable)
            {
                powerText.color = disabledColor;
                intervalText.color = disabledColor;
            }
            else
            {
                if(stats.IsPowerBonus)
                    powerText.color = bonusColor;
                else
                    powerText.color = normalColor;

                if (stats.IsCadencyBonus)
                    intervalText.color = bonusColor;
                else
                    intervalText.color = normalColor;
            }

        }

        void SetEnabled(PlayerStats.LaserStats stats)
        {
            statsBox.SetActive(stats.Unlocked);
            laserVisual.SetActive(stats.Unlocked);
        }
    }
}
