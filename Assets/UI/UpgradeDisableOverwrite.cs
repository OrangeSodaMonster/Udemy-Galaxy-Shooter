using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeDisableOverwrite : MonoBehaviour
{    

	PlayerUpgradesManager upgradesManager;

    static public event Action OnUpdateShip;
    static public event Action OnUpdateLasers;
    static public event Action OnUpdateDrones;
    static public event Action OnUpdateShield;
    static public event Action OnUpdateIonStream;

    private void OnEnable()
    {
        upgradesManager = FindAnyObjectByType<PlayerUpgradesManager>();
    }

    public void DisableTractorBeam()
    {
        if(!upgradesManager.CurrentUpgrades.ShipUpgrades.TractorBeamEnabled) return;

        upgradesManager.CurrentUpgrades.ShipUpgrades.TractorBeamDisableOverwrite = !upgradesManager.CurrentUpgrades.ShipUpgrades.TractorBeamDisableOverwrite;
        OnUpdateShip?.Invoke();
    }

    public void DisableFrontLaser()
	{
        if (!upgradesManager.CurrentUpgrades.FrontLaserUpgrades.Enabled) return;
        upgradesManager.CurrentUpgrades.FrontLaserUpgrades.DisableOverwrite = !upgradesManager.CurrentUpgrades.FrontLaserUpgrades.DisableOverwrite;
        OnUpdateLasers?.Invoke();
    }

    public void DisableSpreadLaser()
    {
        if (!upgradesManager.CurrentUpgrades.SpreadLaserUpgrades.Enabled) return;
        upgradesManager.CurrentUpgrades.SpreadLaserUpgrades.DisableOverwrite = !upgradesManager.CurrentUpgrades.SpreadLaserUpgrades.DisableOverwrite;
        OnUpdateLasers?.Invoke();
    }

    public void DisableLateralLaser()
    {
        if (!upgradesManager.CurrentUpgrades.SideLaserUpgrades.Enabled) return;
        upgradesManager.CurrentUpgrades.SideLaserUpgrades.DisableOverwrite = !upgradesManager.CurrentUpgrades.SideLaserUpgrades.DisableOverwrite;
        OnUpdateLasers?.Invoke();
    }

    public void DisableBackLaser()
    {
        if (!upgradesManager.CurrentUpgrades.BackLaserUpgrades.Enabled) return;
        upgradesManager.CurrentUpgrades.BackLaserUpgrades.DisableOverwrite = !upgradesManager.CurrentUpgrades.BackLaserUpgrades.DisableOverwrite;
        OnUpdateLasers?.Invoke();
    }

    public void DisableFrontShield()
    {
        if (!upgradesManager.CurrentUpgrades.FrontShieldUpgrades.Enabled) return;
        upgradesManager.CurrentUpgrades.FrontShieldUpgrades.DisableOverwrite = !upgradesManager.CurrentUpgrades.FrontShieldUpgrades.DisableOverwrite;
        OnUpdateShield?.Invoke();
    }

    public void DisableLeftShield()
    {
        if (!upgradesManager.CurrentUpgrades.LeftShieldUpgrades.Enabled) return;
        upgradesManager.CurrentUpgrades.LeftShieldUpgrades.DisableOverwrite = !upgradesManager.CurrentUpgrades.LeftShieldUpgrades.DisableOverwrite;
        OnUpdateShield?.Invoke();
    }

    public void DisableRightShield()
    {
        if (!upgradesManager.CurrentUpgrades.RightShieldUpgrades.Enabled) return;
        upgradesManager.CurrentUpgrades.RightShieldUpgrades.DisableOverwrite = !upgradesManager.CurrentUpgrades.RightShieldUpgrades.DisableOverwrite;
        OnUpdateShield?.Invoke();
    }

    public void DisableBackShield()
    {
        if (!upgradesManager.CurrentUpgrades.BackShieldUpgrades.Enabled) return;
        upgradesManager.CurrentUpgrades.BackShieldUpgrades.DisableOverwrite = !upgradesManager.CurrentUpgrades.BackShieldUpgrades.DisableOverwrite;
        OnUpdateShield?.Invoke();
    }
    public void DisableIonStream()
    {
        if (!upgradesManager.CurrentUpgrades.IonStreamUpgrades.Enabled) return;
        upgradesManager.CurrentUpgrades.IonStreamUpgrades.DisableOverwrite = !upgradesManager.CurrentUpgrades.IonStreamUpgrades.DisableOverwrite;
        OnUpdateIonStream?.Invoke();
    }

    public void DisableDrone1()
    {
        if (!upgradesManager.CurrentUpgrades.Drone_1_Upgrades.Enabled) return;
        upgradesManager.CurrentUpgrades.Drone_1_Upgrades.DisableOverwrite = !upgradesManager.CurrentUpgrades.Drone_1_Upgrades.DisableOverwrite;
        OnUpdateDrones?.Invoke();
    }

    public void DisableDrone2()
    {
        if (!upgradesManager.CurrentUpgrades.Drone_2_Upgrades.Enabled) return;
        upgradesManager.CurrentUpgrades.Drone_2_Upgrades.DisableOverwrite = !upgradesManager.CurrentUpgrades.Drone_2_Upgrades.DisableOverwrite;
        OnUpdateDrones?.Invoke();
    }

    public void DisableDrone3()
    {
        if (!upgradesManager.CurrentUpgrades.Drone_3_Upgrades.Enabled) return;
        upgradesManager.CurrentUpgrades.Drone_3_Upgrades.DisableOverwrite = !upgradesManager.CurrentUpgrades.Drone_3_Upgrades.DisableOverwrite;
        OnUpdateDrones?.Invoke();
    }
}