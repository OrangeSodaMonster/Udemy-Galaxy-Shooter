using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class MenuNavigationScript : MonoBehaviour
{
	[SerializeField] GameObject firstSelected;
	[SerializeField] bool shouldSelectOnCancel = true;
	[SerializeField] GameObject selectOnCancel;
    [Space, Tooltip("will 'click' the button if backUI input is called")]
    [SerializeField] bool submitOnBackInput = false;
    [SerializeField] InputSO input;

    bool hasReleasedCancel = true;

    private void Update()
    {
        if (input.IsCancelUI && hasReleasedCancel && shouldSelectOnCancel &&
            EventSystem.current.currentSelectedGameObject != selectOnCancel.GetComponentInChildren<Button>().gameObject)
        {
            EventSystem.current.SetSelectedGameObject(selectOnCancel.GetComponentInChildren<Button>().gameObject);
            hasReleasedCancel = false;
            Debug.Log("Select");
        }

        if (input.IsCancelUI && hasReleasedCancel && submitOnBackInput)
        {
            selectOnCancel.GetComponent<Button>().onClick.Invoke();
            hasReleasedCancel = false;
            Debug.Log("Submit");
        }

        if (!input.IsCancelUI)
            hasReleasedCancel = true;
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(firstSelected.GetComponentInChildren<Button>().gameObject);   
    }
}