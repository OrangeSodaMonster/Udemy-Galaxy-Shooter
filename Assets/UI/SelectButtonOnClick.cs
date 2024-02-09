using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectButtonOnClick : MonoBehaviour
{
	[SerializeField] UnityEvent secondClickEvent;
    [SerializeField] bool playConfirmationSound = true;
    [SerializeField] bool playBackSound = false;

    public static event Action ClickedOnInterface;

    Button button;
    static GameObject lastSelected;   

    private void OnDestroy()
    {
        SelectButtonOnClick.ClickedOnInterface -= SelectOnClick;
    }
     
    private void Start()
    {
        SelectButtonOnClick.ClickedOnInterface += SelectOnClick;

        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(CallClickEvent);

        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>()??button.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry moveEvent = new() { eventID = EventTriggerType.Move};
        moveEvent.callback.AddListener((eventData) => SelectOnMove());
        trigger.triggers.Add(moveEvent);        
    }

    private void OnEnable()
    {
        lastSelected = EventSystem.current.currentSelectedGameObject;
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

        //if(EventSystem.current.currentSelectedGameObject != null)
            lastSelected = EventSystem.current.currentSelectedGameObject;
    }

    public void SelectOnClick()
	{        
        if (EventSystem.current.currentSelectedGameObject == button.gameObject)
        {
            if (EventSystem.current.currentSelectedGameObject == lastSelected)
            {
                secondClickEvent.Invoke();

                if (playConfirmationSound)
                    AudioManager.Instance.ConfirmationClickSound.PlayFeedbacks();
                if (playBackSound)
                    AudioManager.Instance.BackSound.PlayFeedbacks();
            }
            else
                AudioManager.Instance.SelectionClickSound.PlayFeedbacks();

            lastSelected = EventSystem.current.currentSelectedGameObject;
        }        
    } 
}