using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PreventNullSelectionUI : MonoBehaviour
{
    GameObject lastSelected;

    void Update()
    {
        if(EventSystem.current.currentSelectedGameObject == null)
            EventSystem.current.SetSelectedGameObject(lastSelected);
    }

    private void LateUpdate()
    {
        lastSelected = EventSystem.current.currentSelectedGameObject;
    }
}