using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DisableTouchControls : MonoBehaviour
{
	[SerializeField] Canvas touchCanvas;

    public static UnityEvent EnableTouchControls = new();

    private void Awake()
    {
        EnableTouchControls.AddListener(EnableTouch);
    }

    private void Update()
    {
        if(GameStatus.IsPaused || GameStatus.IsGameover || GameStatus.IsStageClear)
        {
            touchCanvas.enabled = false;
        }
        else
        {
            touchCanvas.enabled = true;
        }

    }

    void EnableTouch()
    {
        touchCanvas.enabled = true;
    }
}