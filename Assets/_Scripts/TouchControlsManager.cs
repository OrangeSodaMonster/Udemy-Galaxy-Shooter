using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class TouchControlsManager : MonoBehaviour
{
    //public static bool isTurnToDirection = true;
    //public static int buttonsAlpha = 5;

    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] GameObject analogTurning;
    [SerializeField] GameObject lrTurning;

    //public static void LoadValues(int alpha, bool turnToDirection)
    //{
    //    isTurnToDirection = turnToDirection;
    //    buttonsAlpha = alpha;
    //}

    private void Start()
    {
        SetTurningInput();
        SetAlpha();
    }

    public void SetAlpha()
    {
        canvasGroup.alpha = GameManager.TouchAlpha * 0.1f;
        //buttonsAlpha = (int)value;
    }

    public void SetTurningMode()
    {
        GameManager.IsTouchTurnToDirection = !GameManager.IsTouchTurnToDirection;
        SetTurningInput();
    }

    void SetTurningInput()
    {
        analogTurning.SetActive(GameManager.IsTouchTurnToDirection);
        lrTurning.SetActive(!GameManager.IsTouchTurnToDirection);
    }


}