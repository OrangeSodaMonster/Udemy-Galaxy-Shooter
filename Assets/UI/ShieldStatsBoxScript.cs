using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShieldStatsBoxScript : MonoBehaviour
{
    [SerializeField] ShieldSide type;
    [SerializeField] TextMeshProUGUI strenghtText;
    [SerializeField] TextMeshProUGUI intervalText;
    [SerializeField] Color normalColor = Color.white;
    [SerializeField] Color bonusColor = Color.cyan;
    [SerializeField] Color disabledColor = Color.gray;

    [SerializeField] GameObject statsBox;
    [SerializeField] GameObject ShieldVisual;

    private void OnEnable()
    {
        SetValues(type);
    }

    void SetValues(ShieldSide type)
    {
        switch (type)
        {
            case ShieldSide.Front:
                SetValues(PlayerStats.Instance.Shields.ShieldFront);
                break;
            case ShieldSide.Right:
                SetValues(PlayerStats.Instance.Shields.ShieldRight);
                break;
            case ShieldSide.Left:
                SetValues(PlayerStats.Instance.Shields.ShieldLeft);
                break;
            case ShieldSide.Back:
                SetValues(PlayerStats.Instance.Shields.ShieldBack);
                break;
        }

        void SetValues(PlayerStats.ShieldStats stats)
        {
            SetEnabled(stats);
            if (!stats.Unlocked) return;

            strenghtText.text = $"{stats.CurrentStrenght}/{stats.CurrentMaxStrenght}";
            intervalText.text = $"{stats.CurrentInterval}s";

            if (stats.ForceDisable)
            {
                strenghtText.color = disabledColor;
                intervalText.color = disabledColor;
            }
            else
            {
                if (stats.IsStrenghtBonus)
                    strenghtText.color = bonusColor;
                else
                    strenghtText.color = normalColor;

                if (stats.IsRecoveryBonus)
                    intervalText.color = bonusColor;
                else
                    intervalText.color = normalColor;
            }

        }

        void SetEnabled(PlayerStats.ShieldStats stats)
        {
            statsBox.SetActive(stats.Unlocked);
            ShieldVisual.SetActive(stats.Unlocked);
        }
    }
}
