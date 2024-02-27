using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatus : MonoBehaviour
{
	public static bool IsPaused = false;
    public static event Action PausedGame;
    public static event Action UnPausedGame;
	public static bool IsGameover = false;
    public static event Action GameOver;
	public static bool IsStageClear = false;
    public static event Action StageCleared;
	public static bool IsPortal = false;

    [SerializeField] bool saveConfigOnGameOver = true;

    private void OnEnable()
    {
        IsPaused = false;
        IsGameover = false;
        IsStageClear = false;

        GameOver += SaveConfigOnGameOver;
    }

    private void OnDisable()
    {
        GameOver -= SaveConfigOnGameOver;        
    }

    bool pausedLastFrame = false;
    bool gameoverLastFrame = false;
    private void Update()
    {
        if(IsPaused && !pausedLastFrame)
        {
            PausedGame?.Invoke();
        }
        else if (!IsPaused && pausedLastFrame)
            UnPausedGame?.Invoke();

        if(IsGameover && !gameoverLastFrame)
            GameOver?.Invoke();

        pausedLastFrame = IsPaused;
        gameoverLastFrame = IsGameover;
    }

    void SaveConfigOnGameOver()
    {
        if (saveConfigOnGameOver)
        {
            SaveLoad.instance.SaveConfig();
        }
    }

    static public void ClearStage()
    {
        IsStageClear = true;
        StageCleared?.Invoke();
    }
}