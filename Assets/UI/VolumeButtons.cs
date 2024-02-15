using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeButtons : MonoBehaviour
{
    Slider volumeSlider;

    private void Start()
    {
        volumeSlider = GetComponent<Slider>();
    }

    public void IncreaseVolume()
    {
        volumeSlider.value++;
    }
    public void DecreaseVolume()
    {
        volumeSlider.value--;
    }
}