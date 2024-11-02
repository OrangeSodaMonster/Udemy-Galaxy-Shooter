using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TooltipShieldStrenghtLink : TooltipLink
{
    enum Shield
    {        
        FrontShield = 0,
        RightShield = 1,
        BackShield = 2,
        LeftShield = 3,
    }
    [SerializeField, PropertyOrder(-1)] Shield shield;

    public override string GetValue()
    {
        switch (shield)
        {
            case Shield.FrontShield:
                return PlayerUpgradesManager.Instance.ShieldUpgradesInfo.StrenghtUpgrades[
                    PlayerUpgradesManager.Instance.CurrentUpgrades.FrontShieldUpgrades.ResistenceLevel - 1].Strenght.ToString();
            case Shield.RightShield:
                return PlayerUpgradesManager.Instance.ShieldUpgradesInfo.StrenghtUpgrades[
                    PlayerUpgradesManager.Instance.CurrentUpgrades.RightShieldUpgrades.ResistenceLevel - 1].Strenght.ToString();
            case Shield.BackShield:
                return PlayerUpgradesManager.Instance.ShieldUpgradesInfo.StrenghtUpgrades[
                    PlayerUpgradesManager.Instance.CurrentUpgrades.BackShieldUpgrades.ResistenceLevel - 1].Strenght.ToString();
            case Shield.LeftShield:
                return PlayerUpgradesManager.Instance.ShieldUpgradesInfo.StrenghtUpgrades[
                    PlayerUpgradesManager.Instance.CurrentUpgrades.LeftShieldUpgrades.ResistenceLevel - 1].Strenght.ToString();

            default:
                return "--";
        }
    }
}
