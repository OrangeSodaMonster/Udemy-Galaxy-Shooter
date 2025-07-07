using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombIconPositioner : MonoBehaviour
{
    [SerializeField] RectTransform bombIcon;
    [SerializeField] RectTransform bombIconCanvas;
    [SerializeField] RectTransform bombTouchButton;
    [SerializeField] Camera touchCamera;

    [ShowInInspector] Vector2 screenPos;
    [ShowInInspector] Vector2 screenPosDiference;

    Vector2 defaultIconScale;
    float defaultButtonScale;

    private void Start()
    {
        defaultIconScale = bombIcon.localScale;
        defaultButtonScale = bombTouchButton.lossyScale.x;

        GetScreenDiference();
        SetBombIconPosition();

        GameStatus.UnPausedGame += SetBombIconPosition;
    }

    private void OnDestroy()
    {
        GameStatus.UnPausedGame -= SetBombIconPosition;
    }

    void GetScreenDiference()
    {
        Vector2 screenPosTouch = RectTransformUtility.WorldToScreenPoint(touchCamera, bombTouchButton.position);
        Vector2 screenPosIcon = RectTransformUtility.WorldToScreenPoint(null, bombIcon.position);

        screenPosDiference = screenPosTouch - screenPosIcon;
    }

    void SetBombIconPosition()
    {
        if(!GameStatus.IsMobile) return;

        screenPos = RectTransformUtility.WorldToScreenPoint(touchCamera, bombTouchButton.position);

        Vector2 screenPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(bombIconCanvas, screenPos, null, out screenPoint);

        float scaleFactor = bombTouchButton.lossyScale.x / defaultButtonScale;

        bombIcon.localPosition = screenPoint - screenPosDiference * scaleFactor;

        bombIcon.localScale = defaultIconScale * scaleFactor;
    }
}
