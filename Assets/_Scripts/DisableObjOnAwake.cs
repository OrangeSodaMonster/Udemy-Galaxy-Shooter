using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObjOnAwake : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(false);
    }
}