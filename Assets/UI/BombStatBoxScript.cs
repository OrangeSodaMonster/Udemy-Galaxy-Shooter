using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BombStatBoxScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI powerText;
    [SerializeField] TextMeshProUGUI rangeText;
    [SerializeField] TextMeshProUGUI chargesText;

    [SerializeField] Color normalColor = Color.white;
    [SerializeField] Color bonusColor = Color.cyan;

    private void OnEnable()
    {
        SetValues(PlayerStats.Instance.Bomb);
    }   

    void SetValues(PlayerStats.BombStats stats)
    {
        powerText.text = $"{stats.CurrentPower}";
        rangeText.text = $"{stats.CurrentRange}";
        chargesText.text = $"{stats.Charges}";

        powerText.color = normalColor;
        rangeText.color = normalColor;

        if(BonusSelection.Instance != null && BonusPowersDealer.Instance != null)
        {
            if (BonusSelection.Instance.ActivePowerBonuses.Contains(BonusSelection.BonusType.BombPower) || BonusPowersDealer.Instance.IsSuperBomb)
                powerText.color = bonusColor;
            if (BonusSelection.Instance.ActiveUtilityBonuses.Contains(BonusSelection.BonusType.DroneIonBombRange) || BonusPowersDealer.Instance.IsSuperBomb)
                rangeText.color = bonusColor;
        }
    }    
}
