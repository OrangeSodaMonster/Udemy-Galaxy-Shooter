using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.AudioSettings;

public class HideElementIfNotMobile : MonoBehaviour
{    	
    void Start()
    {
        #if !UNITY_ANDROID && !UNITY_IPHONE
        gameObject.SetActive(false);
        #endif
    }
}