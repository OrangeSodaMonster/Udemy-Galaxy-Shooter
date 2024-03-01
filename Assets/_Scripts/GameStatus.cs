using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

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
	public static bool IsMobile = false;
	public static bool IsJoystick = false;
    public static UnityEvent DisconectedJoystick = new();

    [SerializeField] bool saveConfigOnGameOver = true;

    int joysticksConnected;

    private void OnEnable()
    {
        IsPaused = false;
        IsGameover = false;
        IsStageClear = false;

        GameOver += SaveConfigOnGameOver;

        joysticksConnected = Input.GetJoystickNames().Length;
        IsJoystick = joysticksConnected > 0;
        //StartCoroutine(CheckJoysticksConnectedCO());
    }

    void Start()
    {
        #if UNITY_ANDROID || UNITY_IPHONE
            IsMobile = true;
        #endif
    }

    IEnumerator CheckJoysticksConnectedCO()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f);

            int joysticksLastFrame = joysticksConnected;
            joysticksConnected = Input.GetJoystickNames().Length;
            IsJoystick = joysticksConnected > 0;

            Debug.Log(Input.GetJoystickNames()[0]);

            if (joysticksConnected < joysticksLastFrame)
                DisconectedJoystick?.Invoke();
        }
    }

    private void OnDisable()
    {
        GameOver -= SaveConfigOnGameOver;   
        StopAllCoroutines();
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