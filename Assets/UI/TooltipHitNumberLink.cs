using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipHitNumberLink : TooltipLink
{
    public override string GetValue()
    {
        return PlayerUpgradesManager.Instance.IonStreamUpgradesInfo.HitNumUpgrades[
                    PlayerUpgradesManager.Instance.CurrentUpgrades.IonStreamUpgrades.NumberHitsLevel - 1].NumberOfHits.ToString();
    }
}
