using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnEnableShowOnDisable : MonoBehaviour
{
    [SerializeField] List<GameObject> objectsToHide;

    private void OnEnable()
    {
        for(int i = 0; i < objectsToHide.Count; i++)
        {
            objectsToHide[i].SetActive(false);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < objectsToHide.Count; i++)
        {
            objectsToHide[i].SetActive(true);
        }
    }
}
