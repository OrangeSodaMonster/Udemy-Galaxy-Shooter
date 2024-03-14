using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableIfDevelopment : MonoBehaviour
{
    private void Awake()
    {
        if(Debug.isDebugBuild && !Application.isEditor)
            gameObject.SetActive(false);
    }
}