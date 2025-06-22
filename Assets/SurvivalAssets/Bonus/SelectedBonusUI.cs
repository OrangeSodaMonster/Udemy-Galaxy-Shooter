using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static BonusSelection;

public class SelectedBonusUI : SerializedMonoBehaviour
{
    public Dictionary<BonusType, Sprite> BonusSprites = new();
    [SerializeField] bool isGameOverUI = false;
    [SerializeField] private Image power_1;
    [SerializeField] private Image power_2;
    [SerializeField] private Image utility_1;
    [SerializeField] private Image utility_2;
    [SerializeField] private Image power_Utility;
    [SerializeField] private Image super;
    [Space]
    [SerializeField] Color powerColor;
    [SerializeField] Color utilityColor;

    enum BonusCategory
    {
        Power = 0,
        Utility = 1,
    }

    static public SelectedBonusUI Instance;

    private void Awake()
    {
        if(Instance == null && !isGameOverUI)
            Instance = this;
    }

    public void UpdateIcons()
    {
        BonusSelection selection = BonusSelection.Instance;
        if(selection == null) return;

        if(selection.ActivePowerBonuses.Count >= 1)
        {
            power_1.sprite = BonusSprites[selection.ActivePowerBonuses[0]];
            SetLevelText(power_1, selection.ActivePowerBonuses[0]);
        }
        if(selection.ActivePowerBonuses.Count >= 2)
        {
            power_2.sprite = BonusSprites[selection.ActivePowerBonuses[1]];
            SetLevelText(power_2, selection.ActivePowerBonuses[1]);
        }
        if(selection.ActiveUtilityBonuses.Count >= 1)
        {
            utility_1.sprite = BonusSprites[selection.ActiveUtilityBonuses[0]];
            SetLevelText(utility_1, selection.ActiveUtilityBonuses[0]);
        }
        if(selection.ActiveUtilityBonuses.Count >= 2)
        {
            utility_2.sprite = BonusSprites[selection.ActiveUtilityBonuses[1]];
            SetLevelText(utility_2, selection.ActiveUtilityBonuses[1]);
        }
        if(selection.ActiveSuperBonus.Count >= 1)
        {
            super.sprite = BonusSprites[selection.ActiveSuperBonus[0]];
        }

        if (selection.ActivePowerBonuses.Count >= 3)
        {
            power_Utility.sprite = BonusSprites[selection.ActivePowerBonuses[2]];
            SetLevelText(power_Utility, selection.ActivePowerBonuses[2]);
        }
        else if (selection.ActiveUtilityBonuses.Count >= 3)
        {
            power_Utility.sprite = BonusSprites[selection.ActiveUtilityBonuses[2]];
            SetLevelText(power_Utility, selection.ActiveUtilityBonuses[2]);
        }
    }

    void SetLevelText(Image image, BonusSelection.BonusType type)
    {
        TextMeshProUGUI text = image.transform.GetComponentInChildren<TextMeshProUGUI>();
        text.text = GetLevelText(type);

        if(BonusSelection.Instance.PowerBonuses.Contains(type))
            text.color = powerColor;
        else if (BonusSelection.Instance.UtilityBonuses.Contains(type))
            text.color = utilityColor;

        string GetLevelText(BonusSelection.BonusType type)
        {
            Dictionary<BonusType, int> CurrentBonusLevels = BonusPowersDealer.Instance.GetBonusLevels();

            if (CurrentBonusLevels[type] == 1) return "I";
            else if (CurrentBonusLevels[type] == 2) return "II";
            else return "III";
        }
    }    

    //[Button]
    public void PopulateBonusSprires()
    {
        int numBonus = Enum.GetNames(typeof(BonusType)).Length;
        for (int i = 0; i < numBonus; i++)
        {
            BonusSprites.Add((BonusType)i, null);
        }
    }
}
