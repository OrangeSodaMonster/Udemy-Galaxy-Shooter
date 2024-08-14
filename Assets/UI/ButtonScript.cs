using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Windows;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] bool firstClickSelect = false;
    //[SerializeField] bool disableOnClick = false;
    [FormerlySerializedAs("secondClickEvent")]
	[SerializeField] UnityEvent clickEvents;
	[SerializeField] UnityEvent holdEvents;
	[SerializeField] UnityEvent onPointerDownEvents;
	[SerializeField] UnityEvent onPointerUpEvents;
	[SerializeField] float holdDuration = 2f;
    [SerializeField] bool playHoverSound = true;
    [SerializeField] bool playSelectionSound = true;
    [SerializeField] bool playConfirmationSound = true;
    [SerializeField] bool playBackSound = false;

    public event Action ClickedOnInterface;
    static GameObject lastSelected;

    Button button;
    bool isHoldingButton;
    bool calledHold;
    float holdingTime;

    private void OnEnable()
    {
        ClickedOnInterface += SelectOnClick;
        lastSelected = EventSystem.current.currentSelectedGameObject;
        if(button != null )
            button.enabled = true;
    }

    private void OnDisable()
    {
        ClickedOnInterface -= SelectOnClick;
    }
    public void OnPointerDown(BaseEventData baseData)
    {
        PointerEventData pointerData = baseData as PointerEventData;
        if (pointerData.button == PointerEventData.InputButton.Right)
            return;

        isHoldingButton = true;
        onPointerDownEvents?.Invoke();
    }

    public void OnPointerUp(BaseEventData baseData)    
    {
        PointerEventData pointerData = baseData as PointerEventData;
        if (pointerData.button == PointerEventData.InputButton.Right)
            return;

        onPointerUpEvents?.Invoke();
        isHoldingButton = false;
        calledHold = false;

        if(holdingTime < holdDuration && calledHold == false)
        {
            CallClickEvent();
        }

        holdingTime = 0;
    }

    private void Update()
    {
        if (!isHoldingButton)
            return;

        holdingTime += Time.unscaledDeltaTime;

        if (holdingTime >= holdDuration && calledHold == false)
        {
            holdEvents?.Invoke();
            calledHold = true;
        }
    }

    private void Start()
    {
        button = GetComponentInChildren<Button>();
        //button.onClick.AddListener(CallClickEvent);

        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>()??button.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry moveEvent = new() { eventID = EventTriggerType.Move};
        moveEvent.callback.AddListener((eventData) => SelectOnMove());
        trigger.triggers.Add(moveEvent);

        EventTrigger.Entry hoverEvent = new() { eventID = EventTriggerType.PointerEnter };
        hoverEvent.callback.AddListener((eventData) => PlayHoverSound());
        trigger.triggers.Add(hoverEvent);

        EventTrigger.Entry pointerDownEvent = new() { eventID = EventTriggerType.PointerDown };
        pointerDownEvent.callback.AddListener(OnPointerDown);
        trigger.triggers.Add(pointerDownEvent);

        EventTrigger.Entry pointerUpEvent = new() { eventID = EventTriggerType.PointerUp };
        pointerUpEvent.callback.AddListener(OnPointerUp);
        trigger.triggers.Add(pointerUpEvent);

        EventTrigger.Entry submitEvent = new() { eventID = EventTriggerType.Submit };
        submitEvent.callback.AddListener((eventData) => CallClickEvent());
        trigger.triggers.Add(submitEvent);
    }

    public void CallClickEvent()
    {
        ClickedOnInterface?.Invoke();
    }

    public void SelectOnMove()
    {
        if (EventSystem.current.currentSelectedGameObject != lastSelected)
        {
            AudioManager.Instance.SelectionClickSound.PlayFeedbacks();
        }

        lastSelected = EventSystem.current.currentSelectedGameObject;
    }

    public void SelectOnClick()
	{        
        // Quando o primeiro click seleciona o objeto
        if (firstClickSelect && EventSystem.current.currentSelectedGameObject == button.gameObject)
        {
            if (EventSystem.current.currentSelectedGameObject == lastSelected)
            {
                InvokeClick();
            }
            else
            {
                if(playSelectionSound)
                    AudioManager.Instance.SelectionClickSound.PlayFeedbacks();
            }

            lastSelected = EventSystem.current.currentSelectedGameObject;
        }
        // Quando o primeiro click chama o evento
        else if (EventSystem.current.currentSelectedGameObject == button.gameObject)
        {
            InvokeClick();
        }
    }

    private void InvokeClick()
    {        
        if (playConfirmationSound)
            AudioManager.Instance.ConfirmationClickSound.PlayFeedbacks();
        if (playBackSound)
            AudioManager.Instance.BackSound.PlayFeedbacks();

        clickEvents.Invoke();
    }

    public void PlayHoverSound()
    {
        if (!playHoverSound || !button.enabled) return;

        AudioManager.Instance.HoverSound.PlayFeedbacks();
    }

    public void ChangeClickEvents(UnityAction call, bool wipeEvents = false)
    {
        if (wipeEvents)
        {
            for (int i = 0; i < clickEvents.GetPersistentEventCount(); i++)
            {
                UnityEventTools.RemovePersistentListener(clickEvents, i);
            }
            clickEvents.RemoveAllListeners();
        }            

        clickEvents.AddListener(call);
    }
}