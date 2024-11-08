using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public enum Language
{
    English = 0,
    Português = 1,
    Español = 2,
}

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject defaultBG;
    [SerializeField] GameObject lightBG;
    [Space]
    [SerializeField] MMF_Player enableVib;
    static MMF_Player s_EnableVib;
    [SerializeField] MMF_Player disableVib;
    static MMF_Player s_DisableVib;
    [Space]  
    [SerializeField] bool saveConfigOnGameOver = true;

    public static bool IsVibration = true;
    public static bool IsAutoFire = true;
    public static bool IsLightWeightBG;
    public static int QualityLevel = -1;

    public static bool IsTouchTurnToDirection = true;
    public static int TouchAlpha = 5;
    public static Language CurrentLanguage = Language.English;

    public static int MasterVolume = 5;
    public static int EffectsVolume = 5;
    public static int MusicVolume = 5;
    public static int UiVolume = 5;

    public static int HighestStageCleared = 0;

    public static UnityEvent OnChangeBG = new();
    public static UnityEvent OnLoadedConfig = new();

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void OnEnable()
    {
        s_EnableVib = enableVib;
        s_DisableVib = disableVib;

        GameStatus.GameOver += SaveConfigOnGameOver;
        OnChangeBG.AddListener(SetBG);
        OnLoadedConfig.AddListener(SetVibration);        
        OnLoadedConfig.AddListener(SetBG);
        // Touch alpha set on TouchControlsManager
    }

    private void OnDisable()
    {
        GameStatus.GameOver -= SaveConfigOnGameOver;
    }

    static public void LoadValues(SaveConfigObj save)
    {
        //Debug.Log($"Quality: {QualitySettings.GetQualityLevel()}");
        IsVibration = save.IsVibration;
        IsAutoFire = save.IsAutoFire;
        IsLightWeightBG = save.IsLightWeightBG;
        QualityLevel = save.QualityLevel == -1 ? QualitySettings.GetQualityLevel() : save.QualityLevel;
        CurrentLanguage = save.Language;

        IsTouchTurnToDirection = save.IsTouchTurnToDirection;
        TouchAlpha = save.TouchAlpha;

        MasterVolume = save.MasterVolume;
        EffectsVolume = save.EffectsVolume;
        MusicVolume = save.MusicVolume;
        UiVolume = save.UiVolume;

        OnLoadedConfig?.Invoke();
    }

    static public void DisableVibration()
    {
        IsVibration = false;
        s_DisableVib.PlayFeedbacks();
    }

    static public void EnableVibration()
    {
        IsVibration = true;
        s_EnableVib.PlayFeedbacks();
    }

    void SetVibration()
    {
        if (IsVibration)
            s_EnableVib.PlayFeedbacks();
        else
            s_DisableVib.PlayFeedbacks();
    }

    void SaveConfigOnGameOver()
    {
        if (saveConfigOnGameOver)
        {
            SaveLoad.instance.SaveConfig();
        }
    }   

    //public void ChangeBG()
    //{
    //    IsLightWeightBG = !IsLightWeightBG;

    //    SetBG();
    //}

    void SetBG()
    {
        if (IsLightWeightBG)
        {
            lightBG.SetActive(true);
            defaultBG.SetActive(false);
        }
        else
        {
            lightBG.SetActive(false);
            defaultBG.SetActive(true);
        }
    }

    public void SetHighestStageCleared(int stage)
    {
        if(stage > HighestStageCleared)
            HighestStageCleared = stage;
    }

    public static UnityEvent OnLanguageChange = new();
    public void ChangeLanguage()
    {
        if(CurrentLanguage == Language.English)
            CurrentLanguage = Language.Español;
        else if (CurrentLanguage == Language.Español)
            CurrentLanguage = Language.Português;
        else if (CurrentLanguage == Language.Português)
            CurrentLanguage = Language.English;

        OnLanguageChange?.Invoke();
        Debug.Log(CurrentLanguage);
    }
}