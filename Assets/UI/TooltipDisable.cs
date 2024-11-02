using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipDisable : MonoBehaviour
{   
    void Update()
    {
        if (!EventSystem.current.currentSelectedGameObject.GetComponentInParent<TooltipLink>())
        {
            gameObject.SetActive(false);
        }

        //TooltipLink vai reativar a tooltip
    }
}
