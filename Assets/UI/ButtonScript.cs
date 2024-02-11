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

public class ButtonScript : MonoBehaviour
{
    [SerializeField] bool firstClickSelect = false;
    //[SerializeField] bool disableOnClick = false;
    [FormerlySerializedAs("secondClickEvent")]
	[SerializeField] UnityEvent clickEvents;
    [SerializeField] bool playHoverSound = true;
    [SerializeField] bool playSelectionSound = true;
    [SerializeField] bool playConfirmationSound = true;
    [SerializeField] bool playBackSound = false;

    public event Action ClickedOnInterface;

    Button button;
    static GameObject lastSelected;

    private void OnEnable()
    {
        ClickedOnInterface += SelectOnClick;
        lastSelected = EventSystem.current.currentSelectedGameObject;
        if(button != null )
            button.enabled = true;
    }

    private void OnDisable()
    {
        //ButtonScript.ClickedOnInterface -= SelectOnClick;
        ClickedOnInterface -= SelectOnClick;
    }
     
    private void Start()
    {
        //ButtonScript.ClickedOnInterface += SelectOnClick;       

        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(CallClickEvent);

        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>()??button.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry moveEvent = new() { eventID = EventTriggerType.Move};
        moveEvent.callback.AddListener((eventData) => SelectOnMove());
        trigger.triggers.Add(moveEvent);

        EventTrigger.Entry hoverEvent = new() { eventID = EventTriggerType.PointerEnter };
        hoverEvent.callback.AddListener((eventData) => PlayHoverSound());
        trigger.triggers.Add(hoverEvent);
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
        //if (disableOnClick)
            //button.enabled = false;

        clickEvents.Invoke();
    }

    public void PlayHoverSound()
    {
        if (!playHoverSound || !button.enabled) return;

        AudioManager.Instance.HoverSound.PlayFeedbacks();
    }
}