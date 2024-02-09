using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverSound : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler, ISelectHandler
{
    Button button;

    private void Awake()
    {
        button = GetComponentInChildren<Button>();
    }

    private void Start()
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>()??button.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry hoverEvent = new() { eventID = EventTriggerType.PointerEnter };
        hoverEvent.callback.AddListener(PlayHoverSound);
        trigger.triggers.Add(hoverEvent);

        //EventTrigger.Entry selectEvent = new() { eventID = EventTriggerType.Select };
        //selectEvent.callback.AddListener(PlayHoverSound);
        //trigger.triggers.Add(selectEvent);
    }

    public void PlayHoverSound(BaseEventData eventData)
	{
        AudioManager.Instance.HoverSound.PlayFeedbacks();
	}
}