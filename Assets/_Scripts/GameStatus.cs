using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatus : MonoBehaviour
{
	public static bool IsPaused = false;
	public static bool IsGameover = false;

    private void OnEnable()
    {
        IsPaused = false;
        IsGameover = false;
    }
}