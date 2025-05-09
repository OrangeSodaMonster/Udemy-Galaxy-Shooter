using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BombStatBoxScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI powerText;
    [SerializeField] TextMeshProUGUI rangeText;
    [SerializeField] TextMeshProUGUI chargesText;

    private void OnEnable()
    {
        SetValues(PlayerStats.Instance.Bomb);
    }   

    void SetValues(PlayerStats.BombStats stats)
    {
        powerText.text = $"{stats.CurrentPower}";
        rangeText.text = $"{stats.CurrentRange}";
        chargesText.text = $"{stats.Charges}";
    }    
}
