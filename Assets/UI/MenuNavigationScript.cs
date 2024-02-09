using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class MenuNavigationScript : MonoBehaviour
{
	[SerializeField] GameObject firstSelected;
	[SerializeField] GameObject selectOnCancel;
    [SerializeField] InputSO input;

    bool hasReleasedCancel = true;

    private void Update()
    {
        if (input.IsCancelUI && hasReleasedCancel)
        {
            EventSystem.current.SetSelectedGameObject(selectOnCancel.GetComponentInChildren<Button>().gameObject);
            hasReleasedCancel = false;
        }
        else if (!input.IsCancelUI)
            hasReleasedCancel = true;
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(firstSelected.GetComponentInChildren<Button>().gameObject);   
    }
}