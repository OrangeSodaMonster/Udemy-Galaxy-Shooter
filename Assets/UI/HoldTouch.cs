using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HoldTouch : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float holdDuration = 2.5f;

    private bool isHoldingButton;
    private float holdingTime = 0;

    RightClickButton rClick;
    bool calledHold = false;

    private void Start()
    {
        rClick = GetComponent<RightClickButton>();

        if(rClick == null )
            this.enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHoldingButton = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHoldingButton = false;
        calledHold = false;
        holdingTime = 0;
    }

    private void Update()
    {
        if (!isHoldingButton)
            return;

        holdingTime += Time.unscaledDeltaTime;

        if (holdingTime > holdDuration && calledHold == false)
        {
            rClick.CallRightClick();
            calledHold = true;
        }
    }
}