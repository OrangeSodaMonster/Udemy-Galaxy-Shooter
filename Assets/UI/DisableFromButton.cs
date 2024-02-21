using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RightClickButton))]
public class DisableFromButton : MonoBehaviour
{    
	RightClickButton rClick;

    private void Awake()
    {
        rClick = GetComponent<RightClickButton>();
    }

    private void OnEnable()
    {
        InputHolder.Instance.DisableUI += DisableButton;
    }

    private void OnDisable()
    {
        InputHolder.Instance.DisableUI += DisableButton;        
    }

    void DisableButton()
    {
        if(EventSystem.current.currentSelectedGameObject == gameObject)
            rClick.OnRightClick.Invoke();
    }

}