using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideIfNotMobileNorJoystick : MonoBehaviour
{
    bool isMobile = true;

    void Start()
    {
        #if !UNITY_ANDROID && !UNITY_IPHONE
            isMobile = false;
        #endif

        if(!isMobile && ! GameStatus.IsJoystick)
            gameObject.SetActive(false);
    }
}