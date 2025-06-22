using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Windows;
using System.ComponentModel;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] bool firstClickSelect = false;
    //[SerializeField] bool disableOnClick = false;
    [FormerlySerializedAs("secondClickEvent")]
	[SerializeField] UnityEvent clickEvents;
    [HideInInspector] public UnityEvent OnClick = new();
	[SerializeField] UnityEvent holdEvents;
	[SerializeField] UnityEvent onSelectionEvents;
	[SerializeField] UnityEvent onPointerDownEvents;
	[SerializeField] UnityEvent onPointerUpEvents;
	[SerializeField] float holdDuration = 2f;
    public float TotalHoldDuration { get => holdDuration; set { holdDuration = value; } }
    float holdingTime;
    public float HoldingTime { get => holdingTime; }

    [SerializeField] bool playHoverSound = true;
    public bool PlayHoverSound { get => playHoverSound; set { playHoverSound = value; } }
    [SerializeField] bool playSelectionSound = true;
    public bool PlaySelectionSound { get => playSelectionSound; set { playSelectionSound = value; } }
    [SerializeField] bool playConfirmationSound = true;
    public bool PlayConfirmationSound { get => playConfirmationSound; set { playConfirmationSound = value; } }
    [SerializeField] bool playBackSound = false;
    public bool PlayBackSound { get => playBackSound; set { playBackSound = value; } }
    [SerializeField] UnityEvent alternativeClickEvents;

    public event Action ClickedOnInterface;
    static GameObject lastSelected;

    public bool UseAlternativeClickEvents { get; private set; }
    Button button;
    bool isHoldingButton;
    bool calledHold;

    private void OnEnable()
    {
        ClickedOnInterface += SelectOnClick;
        lastSelected = EventSystem.current.currentSelectedGameObject;
        if(button != null )
            button.enabled = true;

        isHoldingButton = false;
        holdingTime = 0;
        calledHold = false;

        InputHolder.Instance.OnSubmitUI.AddListener(OnSubmitDown);
        InputHolder.Instance.OnReleaseSubmitUI.AddListener(OnSubmitUp);
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
    void OnSubmitDown()
    {
        if(EventSystem.current.currentSelectedGameObject != button.gameObject) return;
        isHoldingButton = true;
    }
    void OnSubmitUp()
    {
        if(EventSystem.current.currentSelectedGameObject != button.gameObject) return;
        isHoldingButton = false;
        calledHold = false;

        if (holdingTime < holdDuration && calledHold == false)
        {
            CallClickEvent();
        }

        holdingTime = 0;
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
        clickEvents.AddListener(CallPublicOnClick);
        //button.onClick.AddListener(CallClickEvent);

        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>()??button.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry moveEvent = new() { eventID = EventTriggerType.Move};
        moveEvent.callback.AddListener((eventData) => SelectOnMove());
        trigger.triggers.Add(moveEvent);

        EventTrigger.Entry hoverEvent = new() { eventID = EventTriggerType.PointerEnter };
        hoverEvent.callback.AddListener((eventData) => CallPlayHoverSound());
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

        EventTrigger.Entry selectEvent = new() { eventID = EventTriggerType.Select };
        selectEvent.callback.AddListener((eventData) => OnSelect());
        trigger.triggers.Add(selectEvent);
    }

    public void CallClickEvent()
    {
        ClickedOnInterface?.Invoke();
    }

    public void OnSelect()
    {
        if (!button.enabled) return;

        if (playSelectionSound && button.gameObject != lastSelected)
            AudioManager.Instance.SelectionClickSound.PlayFeedbacks();

        onSelectionEvents?.Invoke();
    }

    public void SelectOnMove()
    {
        //if (EventSystem.current.currentSelectedGameObject != lastSelected)
        //{
        //    AudioManager.Instance.SelectionClickSound.PlayFeedbacks();
        //}

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
            //else
            //{
            //    if (playSelectionSound)
            //    {
            //        AudioManager.Instance.SelectionClickSound.PlayFeedbacks();
            //    }
            //}

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

        if(!UseAlternativeClickEvents)
            clickEvents.Invoke();
        else
            alternativeClickEvents.Invoke();
    }

    public void CallPlayHoverSound()
    {
        if (!playHoverSound || !button.enabled) return;

        AudioManager.Instance.HoverSound.PlayFeedbacks();
    }

    [Description("Can't wipe persistent events (set in editor)")]
    public void SetAlternativeClickEvents(bool useAlternativeClickEvents, UnityAction call = null, bool wipeEvents = false)
    {
        if (wipeEvents)
            alternativeClickEvents.RemoveAllListeners();

        if(call != null)
            alternativeClickEvents.AddListener(call);

        UseAlternativeClickEvents = useAlternativeClickEvents;
    }

    void CallPublicOnClick()
    {
        OnClick.Invoke();
    }

    public void SelectButton()
    {
        EventSystem.current.SetSelectedGameObject(button.gameObject);
    }

    
}