using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableTouchControls : MonoBehaviour
{
	[SerializeField] Canvas touchCanvas;

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
}