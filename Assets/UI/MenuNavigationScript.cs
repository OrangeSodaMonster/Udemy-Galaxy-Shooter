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

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(firstSelected.GetComponentInChildren<Button>().gameObject);

        InputHolder.Instance.CancelUI += OnCancel;
    }

    private void OnDisable()
    {
        InputHolder.Instance.CancelUI -= OnCancel;        
    }

    private void OnCancel()
    {
        if (shouldSelectOnCancel && EventSystem.current.currentSelectedGameObject != selectOnCancel.GetComponentInChildren<Button>().gameObject)
        {
            EventSystem.current.SetSelectedGameObject(selectOnCancel.GetComponentInChildren<Button>().gameObject);
        }

        if (submitOnBackInput)
        {
            if (selectOnCancel.TryGetComponent(out ButtonScript buttonScript))
            {
                buttonScript.CallClickEvent();
            }
           
            selectOnCancel.GetComponent<Button>().onClick.Invoke();
        }
    }
}