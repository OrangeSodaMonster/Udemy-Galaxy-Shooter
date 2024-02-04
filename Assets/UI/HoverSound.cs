using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverSound : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler, ISelectHandler
{
    public void PlayHoverSound(BaseEventData eventData)
	{
        AudioManager.Instance.HoverSound.PlayFeedbacks();

        Debug.Log("Hover");
	}

    private void Start()
    {
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry hoverEvent = new() { eventID = EventTriggerType.PointerEnter };
        hoverEvent.callback.AddListener(PlayHoverSound);
        trigger.triggers.Add(hoverEvent);

        EventTrigger.Entry selectEvent = new() { eventID = EventTriggerType.Select };
        selectEvent.callback.AddListener(PlayHoverSound);
        trigger.triggers.Add(selectEvent);
    }

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    PlayHoverSound();
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
        
    //}

    //public void OnSelect(BaseEventData eventData)
    //{
    //    PlayHoverSound();
    //}
}