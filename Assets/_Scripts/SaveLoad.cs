using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SaveLoad : MonoBehaviour
{
    //TO SAVE    
        //Fases Completas
        //Melhor Score    

    [Range(1,4)] public int CurrentSaveSlot = 1;
    [SerializeField] UpgradesSO startUpgrades;
    public event Action ChangedSaveSlot;
    static public SaveLoad instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        LoadSlot();        
    }

    private void OnEnable()
    {
        LoadOrCreateSave();
    }

    public void SetSlot(int slot)
    {
        CurrentSaveSlot = slot;

        SaveGeneral();
        LoadOrCreateSave();

        ChangedSaveSlot?.Invoke();
    }

    // Save Slot
    void SaveGeneral()
    {
        MMSaveLoadManager.SaveLoadMethod =  new MMSaveLoadManagerMethodJson();

        SlotObj saveSlot = new SlotObj();
        saveSlot.slot = CurrentSaveSlot;

        MMSaveLoadManager.Save(saveSlot, "lastSaveSlotUsed.saveFile", "SaveSlot");
    }

    void LoadSlot()
    {
        MMSaveLoadManager.SaveLoadMethod =  new MMSaveLoadManagerMethodJson();

        SlotObj loadedData = (SlotObj)MMSaveLoadManager.Load(typeof(SlotObj), "lastSaveSlotUsed.saveFile", "SaveSlot");
        if (loadedData != null)
            CurrentSaveSlot = loadedData.slot;
    }

    //Configs
    [ContextMenu("SaveConfig")]
    public void SaveConfig()
    {
        MMSaveLoadManager.SaveLoadMethod =  new MMSaveLoadManagerMethodJson();

        SaveConfigObj saveConfig = new SaveConfigObj();           

        saveConfig.IsVibration = GameManager.IsVibration;
        saveConfig.IsAutoFire = GameManager.IsAutoFire;  
        saveConfig.IsLightWeightBG = GameManager.IsLightWeightBG;
        saveConfig.QualityLevel = GameManager.QualityLevel;
        saveConfig.TouchAlpha = GameManager.TouchAlpha;
        saveConfig.IsTouchTurnToDirection = GameManager.IsTouchTurnToDirection;

        saveConfig.MasterVolume = GameManager.MasterVolume;
        saveConfig.MusicVolume = GameManager.MusicVolume;
        saveConfig.EffectsVolume = GameManager.EffectsVolume;
        saveConfig.UiVolume = GameManager.UiVolume;
        
        //saveConfig.CreationDate = currentSlotDate;

        saveConfig.CreationDate = GetCreationDate(CurrentSaveSlot);

        MMSaveLoadManager.Save(saveConfig, "Config.saveFile", "Save" + CurrentSaveSlot);

        Debug.Log("Saved Config");
    }

    public void SaveVolumes()
    {
        MMSaveLoadManager.SaveLoadMethod =  new MMSaveLoadManagerMethodJson();
        SaveConfigObj data = (SaveConfigObj)MMSaveLoadManager.Load(typeof(SaveConfigObj), "Config.saveFile", "Save" + CurrentSaveSlot);

        data.MasterVolume = GameManager.MasterVolume;
        data.MusicVolume = GameManager.MusicVolume;
        data.EffectsVolume = GameManager.EffectsVolume;
        data.UiVolume = GameManager.UiVolume;

        data.IsAutoFire = GameManager.IsAutoFire;

        MMSaveLoadManager.Save(data, "Config.saveFile", "Save" + CurrentSaveSlot);

        Debug.Log($"Saved Volumes: {data.MasterVolume}, {data.MusicVolume}, {data.EffectsVolume}, {data.UiVolume}");
    }

    public void SaveConfigInfo()
    {
        MMSaveLoadManager.SaveLoadMethod =  new MMSaveLoadManagerMethodJson();
        SaveConfigObj data = (SaveConfigObj)MMSaveLoadManager.Load(typeof(SaveConfigObj), "Config.saveFile", "Save" + CurrentSaveSlot);

        data.TouchAlpha = GameManager.TouchAlpha;
        data.IsTouchTurnToDirection = GameManager.IsTouchTurnToDirection;
        data.IsVibration = GameManager.IsVibration;
        data.IsLightWeightBG = GameManager.IsLightWeightBG;
        data.QualityLevel = GameManager.QualityLevel;
        data.Language = GameManager.CurrentLanguage;

        data.IsAutoFire = GameManager.IsAutoFire;

        MMSaveLoadManager.Save(data, "Config.saveFile", "Save" + CurrentSaveSlot);

        Debug.Log($"Saved Config");
    }

    [ContextMenu("LoadConfig")]
    public void LoadConfig()
    {
        MMSaveLoadManager.SaveLoadMethod =  new MMSaveLoadManagerMethodJson();

        SaveConfigObj loadedData = (SaveConfigObj)MMSaveLoadManager.Load(typeof(SaveConfigObj), "Config.saveFile", "Save" + CurrentSaveSlot);

        GameManager.LoadValues(loadedData);
        //GameManager.IsAutoFire = loadedData.IsAutoFire;
        //AudioTrackConfig.Instance.LoadVolume(loadedData.MasterVolume, loadedData.MusicVolume, loadedData.EffectsVolume, loadedData.UiVolume);

        //TouchControlsManager.LoadValues(loadedData.touchAlpha, loadedData.touchTurnToDirection);

        Debug.Log("Loaded Config");
    }

    public void CreateConfig()
    {
        MMSaveLoadManager.SaveLoadMethod =  new MMSaveLoadManagerMethodJson();

        SaveConfigObj saveConfig = new SaveConfigObj();
        saveConfig.CreationDate = System.DateTime.Now.ToString("yyyy/MM/dd");

        MMSaveLoadManager.Save(saveConfig, "Config.saveFile", "Save" + CurrentSaveSlot);

        Debug.Log($"<color=red>CONFIG CREATED</color>");
    }

    public string GetCreationDate(int slot)
    {
        MMSaveLoadManager.SaveLoadMethod =  new MMSaveLoadManagerMethodJson();

        SaveConfigObj loadedData = (SaveConfigObj)MMSaveLoadManager.Load(typeof(SaveConfigObj), "Config.saveFile", "Save" + slot);

        if(loadedData != null && loadedData.CreationDate != null)
            return loadedData.CreationDate;
        else
            return "----/--/--";
    }

    public bool TryGetConfig(int slot)
    {
        MMSaveLoadManager.SaveLoadMethod =  new MMSaveLoadManagerMethodJson();

        SaveConfigObj loadedData = (SaveConfigObj)MMSaveLoadManager.Load(typeof(SaveConfigObj), "Config.saveFile", "Save" + slot);

        if (loadedData != null)
            return true;
        else 
            return false;
    } 

    // GameState
    [ContextMenu("SaveState")]
    public void SaveState()
    {
        MMSaveLoadManager.SaveLoadMethod =  new MMSaveLoadManagerMethodJson();

        SaveUpgradesObj saveUpgrades = SetUpgradesObject(PlayerUpgradesManager.Instance.CurrentUpgrades);
        MMSaveLoadManager.Save(saveUpgrades, "Upgrades.saveFile", "Save" + CurrentSaveSlot);

        SaveResourcesObj saveResources = new();
        saveResources.MetalCount = PlayerCollectiblesCount.MetalAmount;
        saveResources.RareMetalCount = PlayerCollectiblesCount.RareMetalAmount;
        saveResources.EnergyCristalCount = PlayerCollectiblesCount.EnergyCristalAmount;
        saveResources.CondensedEnergyCristalCount = PlayerCollectiblesCount.CondensedEnergyCristalAmount;
        MMSaveLoadManager.Save(saveResources, "Resources.saveFile", "Save" + CurrentSaveSlot);

        SaveStageAndTimeObj SaveStages = new();
        SaveStages.HighestStageCleared = GameManager.HighestStageCleared;
        MMSaveLoadManager.Save(SaveStages, "Stages.saveFile", "Save" + CurrentSaveSlot);

        Debug.Log("Saved State");
    }

    [ContextMenu("LoadState")]
    public void LoadState()
    {
        MMSaveLoadManager.SaveLoadMethod =  new MMSaveLoadManagerMethodJson();

        SaveUpgradesObj loadedUpgrades = (SaveUpgradesObj)MMSaveLoadManager.Load(typeof(SaveUpgradesObj), "Upgrades.saveFile", "Save" + CurrentSaveSlot);
        SetUpgradesSO(loadedUpgrades, PlayerUpgradesManager.Instance.CurrentUpgrades);

        SaveResourcesObj loadedResources = (SaveResourcesObj)MMSaveLoadManager.Load(typeof(SaveResourcesObj), "Resources.saveFile", "Save" + CurrentSaveSlot);
        PlayerCollectiblesCount.LoadResources(loadedResources.MetalCount, loadedResources.RareMetalCount, loadedResources.EnergyCristalCount, loadedResources.CondensedEnergyCristalCount);

        SaveStageAndTimeObj loadedStages = (SaveStageAndTimeObj)MMSaveLoadManager.Load(typeof(SaveStageAndTimeObj), "Stages.saveFile", "Save" + CurrentSaveSlot);
        GameManager.HighestStageCleared = loadedStages.HighestStageCleared;

        Debug.Log("Loaded State");
    }

    void CreateSaveState()
    {
        MMSaveLoadManager.SaveLoadMethod =  new MMSaveLoadManagerMethodJson();

        SaveUpgradesObj saveUpgrades = SetUpgradesObject(startUpgrades);
        MMSaveLoadManager.Save(saveUpgrades, "Upgrades.saveFile", "Save" + CurrentSaveSlot);

        SaveResourcesObj saveResources = new();
        MMSaveLoadManager.Save(saveResources, "Resources.saveFile", "Save" + CurrentSaveSlot);

        SaveStageAndTimeObj SaveStages = new();
        MMSaveLoadManager.Save(SaveStages, "Stages.saveFile", "Save" + CurrentSaveSlot);
    }

    public bool TryGetState(int slot)
    {
        MMSaveLoadManager.SaveLoadMethod =  new MMSaveLoadManagerMethodJson();

        SaveUpgradesObj loadedUpgrades = (SaveUpgradesObj)MMSaveLoadManager.Load(typeof(SaveUpgradesObj), "Upgrades.saveFile", "Save" + slot);

        if (loadedUpgrades != null)
            return true;
        else
            return false;
    }

    // Erase Save

    public void EraseSave(int slot)
    {
        if (CurrentSaveSlot == slot)
        {
            CurrentSaveSlot = 1;
            SaveGeneral();
        }

        MMSaveLoadManager.DeleteSaveFolder("Save" + slot);

        LoadOrCreateSave();

        ChangedSaveSlot?.Invoke();

        Debug.Log("Erased Slot " + slot);
    }

    private void LoadOrCreateSave()
    {
        // Criar Save se não existir nesse slot

        if (TryGetConfig(CurrentSaveSlot))
        {
            LoadConfig();
        }
        else
        {
            CreateConfig();
            LoadConfig();
        }

        if (TryGetState(CurrentSaveSlot))
        {
            LoadState();
        }
        else
        {            
            CreateSaveState();
            LoadState();
        }
    }

    public void SaveDisables()
    {
        SaveUpgradesObj upgrades = (SaveUpgradesObj)MMSaveLoadManager.Load(typeof(SaveUpgradesObj), "Upgrades.saveFile", "Save" + CurrentSaveSlot);

        upgrades.FL_DisableOverwrite = PlayerUpgradesManager.Instance.CurrentUpgrades.FrontLaserUpgrades.DisableOverwrite;
        upgrades.SL_DisableOverwrite = PlayerUpgradesManager.Instance.CurrentUpgrades.SpreadLaserUpgrades.DisableOverwrite;
        upgrades.LL_DisableOverwrite = PlayerUpgradesManager.Instance.CurrentUpgrades.SideLaserUpgrades.DisableOverwrite;
        upgrades.BL_DisableOverwrite = PlayerUpgradesManager.Instance.CurrentUpgrades.BackLaserUpgrades.DisableOverwrite;

        upgrades.FS_DisableOverwrite = PlayerUpgradesManager.Instance.CurrentUpgrades.FrontShieldUpgrades.DisableOverwrite;
        upgrades.RS_DisableOverwrite = PlayerUpgradesManager.Instance.CurrentUpgrades.RightShieldUpgrades.DisableOverwrite;
        upgrades.LS_DisableOverwrite = PlayerUpgradesManager.Instance.CurrentUpgrades.LeftShieldUpgrades.DisableOverwrite;
        upgrades.BS_DisableOverwrite = PlayerUpgradesManager.Instance.CurrentUpgrades.BackShieldUpgrades.DisableOverwrite;

        upgrades.D1_DisableOverwrite = PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_1_Upgrades.DisableOverwrite;
        upgrades.D2_DisableOverwrite = PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_2_Upgrades.DisableOverwrite;
        upgrades.D3_DisableOverwrite = PlayerUpgradesManager.Instance.CurrentUpgrades.Drone_3_Upgrades.DisableOverwrite;

        upgrades.IS_DisableOverwrite = PlayerUpgradesManager.Instance.CurrentUpgrades.IonStreamUpgrades.DisableOverwrite;

        upgrades.SH_TractorBeamDisableOverwrite = PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.TractorBeamDisableOverwrite;

        MMSaveLoadManager.Save(upgrades, "Upgrades.saveFile", "Save" + CurrentSaveSlot);

        Debug.Log("SAVED DISABLES");
    }

    // Save Objects       
    
    class SaveResourcesObj
    {
        public int MetalCount = 0;
        public int RareMetalCount = 0;
        public int EnergyCristalCount = 0;
        public int CondensedEnergyCristalCount = 0;
    }

    class SaveStageAndTimeObj
    {
        public int HighestStageCleared = 0;
    }

    class SaveUpgradesObj
    {
        // Lasers
        public bool FL_Enabled;
        public bool FL_DisableOverwrite;
        public int FL_DamageLevel;
        public int FL_CadencyLevel;

        public bool SL_Enabled;
        public bool SL_DisableOverwrite;
        public int SL_DamageLevel;
        public int SL_CadencyLevel;

        public bool LL_Enabled;
        public bool LL_DisableOverwrite;
        public int LL_DamageLevel;
        public int LL_CadencyLevel;

        public bool BL_Enabled;
        public bool BL_DisableOverwrite;
        public int BL_DamageLevel;
        public int BL_CadencyLevel;

        // Shields
        public bool FS_Enabled;
        public bool FS_DisableOverwrite;
        public int FS_ResistenceLevel;
        public int FS_RecoveryLevel;

        public bool RS_Enabled;
        public bool RS_DisableOverwrite;
        public int RS_ResistenceLevel;
        public int RS_RecoveryLevel;

        public bool LS_Enabled;
        public bool LS_DisableOverwrite;
        public int LS_ResistenceLevel;
        public int LS_RecoveryLevel;

        public bool BS_Enabled;
        public bool BS_DisableOverwrite;
        public int BS_ResistenceLevel;
        public int BS_RecoveryLevel;

        // Drones
        public bool D1_Enabled;
        public bool D1_DisableOverwrite;
        public int D1_DamageLevel;
        public int D1_RangeLevel;
        public int D1_HealingLevel;

        public bool D2_Enabled;
        public bool D2_DisableOverwrite;
        public int D2_DamageLevel;
        public int D2_RangeLevel;
        public int D2_HealingLevel;

        public bool D3_Enabled;
        public bool D3_DisableOverwrite;
        public int D3_DamageLevel;
        public int D3_RangeLevel;
        public int D3_HealingLevel;

        // IonStream
        public bool IS_Enabled;
        public bool IS_DisableOverwrite;
        public int IS_DamageLevel;
        public int IS_CadencyLevel;
        public int IS_RangeLevel;
        public int IS_NumberHitsLevel;

        // ShipUpgrades
        public int SH_HPLevel;
        public int SH_SpeedLevel;
        public int SH_ManobrabilityLevel;
        public bool SH_TractorBeamEnabled;
        public bool SH_TractorBeamDisableOverwrite;
        public int SH_TractorBeamLevel;
    }

    SaveUpgradesObj SetUpgradesObject(UpgradesSO so)
    {
        SaveUpgradesObj upgrades = new SaveUpgradesObj();

        // Lasers
        upgrades.FL_Enabled = so.FrontLaserUpgrades.Enabled;
        upgrades.FL_DisableOverwrite = so.FrontLaserUpgrades.DisableOverwrite;
        upgrades.FL_DamageLevel = so.FrontLaserUpgrades.DamageLevel;
        upgrades.FL_CadencyLevel = so.FrontLaserUpgrades.CadencyLevel;

        upgrades.SL_Enabled = so.SpreadLaserUpgrades.Enabled;
        upgrades.SL_DisableOverwrite = so.SpreadLaserUpgrades.DisableOverwrite;
        upgrades.SL_DamageLevel = so.SpreadLaserUpgrades.DamageLevel;
        upgrades.SL_CadencyLevel = so.SpreadLaserUpgrades.CadencyLevel;

        upgrades.LL_Enabled = so.SideLaserUpgrades.Enabled;
        upgrades.LL_DisableOverwrite = so.SideLaserUpgrades.DisableOverwrite;
        upgrades.LL_DamageLevel = so.SideLaserUpgrades.DamageLevel;
        upgrades.LL_CadencyLevel = so.SideLaserUpgrades.CadencyLevel;

        upgrades.BL_Enabled = so.BackLaserUpgrades.Enabled;
        upgrades.BL_DisableOverwrite = so.BackLaserUpgrades.DisableOverwrite;
        upgrades.BL_DamageLevel = so.BackLaserUpgrades.DamageLevel;
        upgrades.BL_CadencyLevel = so.BackLaserUpgrades.CadencyLevel;

        // Shields
        upgrades.FS_Enabled = so.FrontShieldUpgrades.Enabled;
        upgrades.FS_DisableOverwrite = so.FrontShieldUpgrades.DisableOverwrite;
        upgrades.FS_ResistenceLevel = so.FrontShieldUpgrades.ResistenceLevel;
        upgrades.FS_RecoveryLevel = so.FrontShieldUpgrades.RecoveryLevel;

        upgrades.RS_Enabled = so.RightShieldUpgrades.Enabled;
        upgrades.RS_DisableOverwrite = so.RightShieldUpgrades.DisableOverwrite;
        upgrades.RS_ResistenceLevel = so.RightShieldUpgrades.ResistenceLevel;
        upgrades.RS_RecoveryLevel = so.RightShieldUpgrades.RecoveryLevel;

        upgrades.LS_Enabled = so.LeftShieldUpgrades.Enabled;
        upgrades.LS_DisableOverwrite = so.LeftShieldUpgrades.DisableOverwrite;
        upgrades.LS_ResistenceLevel = so.LeftShieldUpgrades.ResistenceLevel;
        upgrades.LS_RecoveryLevel = so.LeftShieldUpgrades.RecoveryLevel;

        upgrades.BS_Enabled = so.BackShieldUpgrades.Enabled;
        upgrades.BS_DisableOverwrite = so.BackShieldUpgrades.DisableOverwrite;
        upgrades.BS_ResistenceLevel = so.BackShieldUpgrades.ResistenceLevel;
        upgrades.BS_RecoveryLevel = so.BackShieldUpgrades.RecoveryLevel;

        // Drones
        upgrades.D1_Enabled = so.Drone_1_Upgrades.Enabled;
        upgrades.D1_DisableOverwrite = so.Drone_1_Upgrades.DisableOverwrite;
        upgrades.D1_DamageLevel = so.Drone_1_Upgrades.DamageLevel;
        upgrades.D1_RangeLevel = so.Drone_1_Upgrades.RangeLevel;
        upgrades.D1_HealingLevel = so.Drone_1_Upgrades.HealingLevel;

        upgrades.D2_Enabled = so.Drone_2_Upgrades.Enabled;
        upgrades.D2_DisableOverwrite = so.Drone_2_Upgrades.DisableOverwrite;
        upgrades.D2_DamageLevel = so.Drone_2_Upgrades.DamageLevel;
        upgrades.D2_RangeLevel = so.Drone_2_Upgrades.RangeLevel;
        upgrades.D2_HealingLevel = so.Drone_2_Upgrades.HealingLevel;

        upgrades.D3_Enabled = so.Drone_3_Upgrades.Enabled;
        upgrades.D3_DisableOverwrite = so.Drone_3_Upgrades.DisableOverwrite;
        upgrades.D3_DamageLevel = so.Drone_3_Upgrades.DamageLevel;
        upgrades.D3_RangeLevel = so.Drone_3_Upgrades.RangeLevel;
        upgrades.D3_HealingLevel = so.Drone_3_Upgrades.HealingLevel;

        // IonStream
        upgrades.IS_Enabled = so.IonStreamUpgrades.Enabled;
        upgrades.IS_DisableOverwrite = so.IonStreamUpgrades.DisableOverwrite;
        upgrades.IS_DamageLevel = so.IonStreamUpgrades.DamageLevel;
        upgrades.IS_CadencyLevel = so.IonStreamUpgrades.CadencyLevel;
        upgrades.IS_RangeLevel = so.IonStreamUpgrades.RangeLevel;
        upgrades.IS_NumberHitsLevel = so.IonStreamUpgrades.NumberHitsLevel;

        // ShipUpgrades
        upgrades.SH_HPLevel = so.ShipUpgrades.HPLevel;
        upgrades.SH_SpeedLevel = so.ShipUpgrades.SpeedLevel;
        upgrades.SH_ManobrabilityLevel = so.ShipUpgrades.ManobrabilityLevel;
        upgrades.SH_TractorBeamEnabled = so.ShipUpgrades.TractorBeamEnabled;
        upgrades.SH_TractorBeamDisableOverwrite = so.ShipUpgrades.TractorBeamDisableOverwrite;
        upgrades.SH_TractorBeamLevel = so.ShipUpgrades.TractorBeamLevel;

        return upgrades;
    }

    static void SetUpgradesSO(SaveUpgradesObj save, UpgradesSO so)
    {
        // Lasers
        so.FrontLaserUpgrades.Enabled = save.FL_Enabled;
        so.FrontLaserUpgrades.DisableOverwrite = save.FL_DisableOverwrite;
        so.FrontLaserUpgrades.DamageLevel = save.FL_DamageLevel;
        so.FrontLaserUpgrades.CadencyLevel = save.FL_CadencyLevel;

        so.SpreadLaserUpgrades.Enabled = save.SL_Enabled;
        so.SpreadLaserUpgrades.DisableOverwrite = save.SL_DisableOverwrite;
        so.SpreadLaserUpgrades.DamageLevel = save.SL_DamageLevel;
        so.SpreadLaserUpgrades.CadencyLevel = save.SL_CadencyLevel;

        so.SideLaserUpgrades.Enabled = save.LL_Enabled;
        so.SideLaserUpgrades.DisableOverwrite = save.LL_DisableOverwrite;
        so.SideLaserUpgrades.DamageLevel = save.LL_DamageLevel;
        so.SideLaserUpgrades.CadencyLevel = save.LL_CadencyLevel;

        so.BackLaserUpgrades.Enabled = save.BL_Enabled;
        so.BackLaserUpgrades.DisableOverwrite = save.BL_DisableOverwrite;
        so.BackLaserUpgrades.DamageLevel = save.BL_DamageLevel;
        so.BackLaserUpgrades.CadencyLevel = save.BL_CadencyLevel;

        // Shields
        so.FrontShieldUpgrades.Enabled = save.FS_Enabled;
        so.FrontShieldUpgrades.DisableOverwrite = save.FS_DisableOverwrite;
        so.FrontShieldUpgrades.ResistenceLevel = save.FS_ResistenceLevel;
        so.FrontShieldUpgrades.RecoveryLevel = save.FS_RecoveryLevel;

        so.RightShieldUpgrades.Enabled = save.RS_Enabled;
        so.RightShieldUpgrades.DisableOverwrite = save.RS_DisableOverwrite;
        so.RightShieldUpgrades.ResistenceLevel = save.RS_ResistenceLevel;
        so.RightShieldUpgrades.RecoveryLevel = save.RS_RecoveryLevel;

        so.LeftShieldUpgrades.Enabled = save.LS_Enabled;
        so.LeftShieldUpgrades.DisableOverwrite = save.LS_DisableOverwrite;
        so.LeftShieldUpgrades.ResistenceLevel = save.LS_ResistenceLevel;
        so.LeftShieldUpgrades.RecoveryLevel = save.LS_RecoveryLevel;

        so.BackShieldUpgrades.Enabled = save.BS_Enabled;
        so.BackShieldUpgrades.DisableOverwrite = save.BS_DisableOverwrite;
        so.BackShieldUpgrades.ResistenceLevel = save.BS_ResistenceLevel;
        so.BackShieldUpgrades.RecoveryLevel = save.BS_RecoveryLevel;

        // Drones
        so.Drone_1_Upgrades.Enabled = save.D1_Enabled;
        so.Drone_1_Upgrades.DisableOverwrite = save.D1_DisableOverwrite;
        so.Drone_1_Upgrades.DamageLevel = save.D1_DamageLevel;
        so.Drone_1_Upgrades.RangeLevel = save.D1_RangeLevel;
        so.Drone_1_Upgrades.HealingLevel = save.D1_HealingLevel;

        so.Drone_2_Upgrades.Enabled = save.D2_Enabled;
        so.Drone_2_Upgrades.DisableOverwrite = save.D2_DisableOverwrite;
        so.Drone_2_Upgrades.DamageLevel = save.D2_DamageLevel;
        so.Drone_2_Upgrades.RangeLevel = save.D2_RangeLevel;
        so.Drone_2_Upgrades.HealingLevel = save.D2_HealingLevel;

        so.Drone_3_Upgrades.Enabled = save.D3_Enabled;
        so.Drone_3_Upgrades.DisableOverwrite = save.D3_DisableOverwrite;
        so.Drone_3_Upgrades.DamageLevel = save.D3_DamageLevel;
        so.Drone_3_Upgrades.RangeLevel = save.D3_RangeLevel;
        so.Drone_3_Upgrades.HealingLevel = save.D3_HealingLevel;

        // IonStream
        so.IonStreamUpgrades.Enabled = save.IS_Enabled;
        so.IonStreamUpgrades.DisableOverwrite = save.IS_DisableOverwrite;
        so.IonStreamUpgrades.DamageLevel = save.IS_DamageLevel;
        so.IonStreamUpgrades.CadencyLevel = save.IS_CadencyLevel;
        so.IonStreamUpgrades.RangeLevel = save.IS_RangeLevel;
        so.IonStreamUpgrades.NumberHitsLevel = save.IS_NumberHitsLevel;

        // ShipUpgrades
        so.ShipUpgrades.HPLevel = save.SH_HPLevel;
        so.ShipUpgrades.SpeedLevel = save.SH_SpeedLevel;
        so.ShipUpgrades.ManobrabilityLevel = save.SH_ManobrabilityLevel;
        so.ShipUpgrades.TractorBeamEnabled = save.SH_TractorBeamEnabled;
        so.ShipUpgrades.TractorBeamDisableOverwrite = save.SH_TractorBeamDisableOverwrite;
        so.ShipUpgrades.TractorBeamLevel = save.SH_TractorBeamLevel;

    }

    class SlotObj
    {
        public int slot;
    }
}

public class SaveConfigObj
{
    public bool IsVibration = true;
    public bool IsAutoFire = true;
    public bool IsLightWeightBG = false;
    public int QualityLevel = -1;
    public Language Language = Language.English;

    public int TouchAlpha = 5;
    public bool IsTouchTurnToDirection = true;

    public int MasterVolume = 5;
    public int EffectsVolume = 5;
    public int MusicVolume = 5;
    public int UiVolume = 5;

    public string CreationDate;
}