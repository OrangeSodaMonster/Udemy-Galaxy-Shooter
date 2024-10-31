using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class RememberLastSelectionScript : MonoBehaviour
{
    enum ReturnDirection
    {
        up = 0, down = 1, left = 2, right = 3,
    }

    [SerializeField] ReturnDirection returnDirection;
    [SerializeField] List<Button> acceptableReturns;
    [SerializeField] Button defaltButton;
    [Tooltip("Can be left null if not needed")]
    [SerializeField] MenuNavigationScript firstSelectedCanvasButton;

    Navigation buttonNav;
    GameObject lastAcceptableSelection;
    GameObject lastSelection;
    Button lastAcceptableButton;
    Button thisButton;

    void Start()
    {
        thisButton = GetComponent<Button>();
        buttonNav = thisButton.navigation;        
    }   

    void Update()
    {
        if(EventSystem.current.currentSelectedGameObject == this.gameObject && lastSelection != this.gameObject)
        {
            Debug.Log("Set return button");
            Debug.Log(lastAcceptableSelection.name);
            SetReturnSelection();
        }

        if(EventSystem.current.currentSelectedGameObject.TryGetComponent(out Button currentButton) &&
            acceptableReturns.Contains(currentButton))
        {
            lastAcceptableSelection = EventSystem.current.currentSelectedGameObject;
        }

        lastSelection = EventSystem.current.currentSelectedGameObject;
    }

    void SetReturnSelection()
    {
        lastAcceptableButton = lastAcceptableSelection.GetComponent<Button>();
        if (lastAcceptableButton == null) 
        {
            Debug.Log("No acceptable Selection");
            buttonNav.selectOnUp = defaltButton;
            lastAcceptableSelection = null;
            return;
        }

        switch (returnDirection)
        {
            case ReturnDirection.up:
                buttonNav.selectOnUp = lastAcceptableButton;                
                break;
            case ReturnDirection.down:
                buttonNav.selectOnDown = lastAcceptableButton;
                break;
            case ReturnDirection.left:
                buttonNav.selectOnLeft = lastAcceptableButton;
                break;
            case ReturnDirection.right:
                buttonNav.selectOnRight = lastAcceptableButton;
                break;
        }
        if(firstSelectedCanvasButton != null)
        {
            Debug.Log("Set first select on canvas");
            firstSelectedCanvasButton.SetFirstSelected(lastAcceptableButton.gameObject);
        }

        thisButton.navigation = buttonNav;
        lastAcceptableSelection = null;
    }
}
