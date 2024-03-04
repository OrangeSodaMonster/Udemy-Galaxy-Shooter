using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] MMF_Player enableVib;
    static MMF_Player s_EnableVib;
    [SerializeField] MMF_Player disableVib;
    static MMF_Player s_DisableVib;

    [SerializeField] bool saveConfigOnGameOver = true;

    public static bool IsVibration = true;
    public static bool IsAutoFire = true;

    public static bool IsTouchTurnToDirection = true;
    public static int TouchAlpha = 5;

    public static int MasterVolume = 5;
    public static int EffectsVolume = 5;
    public static int MusicVolume = 5;
    public static int UiVolume = 5;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void OnEnable()
    {

        s_EnableVib = enableVib;
        s_DisableVib = disableVib;

        GameStatus.GameOver += SaveConfigOnGameOver;
    }

    private void OnDisable()
    {
        GameStatus.GameOver -= SaveConfigOnGameOver;
    }

    static public void LoadValues(SaveConfigObj save)
    {
        IsVibration = save.IsVibration;
        IsAutoFire = save.IsAutoFire;

        IsTouchTurnToDirection = save.IsTouchTurnToDirection;
        TouchAlpha = save.TouchAlpha;

        MasterVolume = save.MasterVolume;
        EffectsVolume = save.EffectsVolume;
        MusicVolume = save.MusicVolume;
        UiVolume = save.UiVolume;

        Debug.Log("Manager Loaded Preferences");
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

    void SaveConfigOnGameOver()
    {
        if (saveConfigOnGameOver)
        {
            SaveLoad.instance.SaveConfig();
        }
    }

}