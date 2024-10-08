using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAttacksActive : MonoBehaviour
{
    int activeNow;
    int activeLast;

    int childCount = 0;

    private void Start()
    {
        childCount = transform.childCount;
    }
    void Update()
    {
        activeNow = 0;

        for(int i = 0; i < childCount; i++)
        {
            if(transform.GetChild(i).gameObject.activeSelf)
                activeNow++;
        }

        if (activeNow == 0)
        {
            AudioManager.Instance.PauseDrone();

            if (activeNow != activeLast)
                Debug.Log("PauseDronesSound");
        }
        else if (activeNow != activeLast)
        {
            AudioManager.Instance.SetDroneVolume(activeNow);
            AudioManager.Instance.PlayDrone();

            Debug.Log("PlayDronesSound");
        }

    }

    private void LateUpdate()
    {
        activeLast = activeNow;
    }
}