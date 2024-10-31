using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RightClickButton : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent OnRightClick;

    //private void Start()
    //{
    //    #if UNITY_ANDROID || UNITY_IPHONE
    //        this.enabled = false;
    //    #endif
    //}

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick?.Invoke();
            //Debug.Log("RightClick");
        }
    }

    public void CallRightClick()
    {
        OnRightClick?.Invoke();
    }
}