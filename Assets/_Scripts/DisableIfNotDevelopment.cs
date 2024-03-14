using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableIfNotDevelopment : MonoBehaviour
{
    private void Awake()
    {
        if(!Debug.isDebugBuild && !Application.isEditor)
            gameObject.SetActive(false);
    }
}