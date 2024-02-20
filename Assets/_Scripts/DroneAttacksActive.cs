using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAttacksActive : MonoBehaviour
{
    int activeNow;
    int activeLast;

    void Update()
    {
        activeNow = 0;

        foreach(Transform child in transform)
        {
            if (child.gameObject.activeSelf)
                activeNow++;
        }

        if (activeNow == 0)
        {
            AudioManager.Instance.PauseDrone();
        }
        else if (activeNow != activeLast)
        {
            AudioManager.Instance.SetDroneVolume(activeNow);
            AudioManager.Instance.PlayDrone();
        }

    }

    private void LateUpdate()
    {
        activeLast = activeNow;
    }
}