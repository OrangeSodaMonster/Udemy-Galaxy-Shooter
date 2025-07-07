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
    [SerializeField] List<RectTransform> scalableObjects;

    private void OnEnable()
    {
        SetTurningInput();
        SetCanvasAlpha();
        SetCanvasScale();
    }

    public void SetAlphaValue(float value)
    {        
        GameManager.TouchAlpha = (int)value;
        SetCanvasAlpha();
    }

    public void SetScaleValue(float value)
    {
        GameManager.TouchScale = (int)value;
        SetCanvasScale();
    }

    public void SetCanvasAlpha()
    {
        canvasGroup.alpha = GameManager.TouchAlpha * 0.1f;
        Debug.Log($"Touch Alpha: {GameManager.TouchAlpha}");
    }

    public void SetCanvasScale()
    {
        for(int i = 0; i < scalableObjects.Count; i++)
        {
            scalableObjects[i].localScale = Vector3.one * GameManager.TouchScale * 0.01f;
        }

        Debug.Log($"Touch Scale: {GameManager.TouchScale}%");
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