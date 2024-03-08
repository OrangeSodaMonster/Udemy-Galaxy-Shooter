using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class TouchControlsManager : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] GameObject analogTurning;
    [SerializeField] GameObject lrTurning;

    private void OnEnable()
    {
        SetTurningInput();
        SetCanvasAlpha();
    }

    public void SetAlphaValue(float value)
    {        
        GameManager.TouchAlpha = (int)value;
        SetCanvasAlpha();
    }

    public void SetCanvasAlpha()
    {
        canvasGroup.alpha = GameManager.TouchAlpha * 0.1f;
        Debug.Log($"Touch Alpha: {GameManager.TouchAlpha}");
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