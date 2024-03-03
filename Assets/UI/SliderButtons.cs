using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderButtons : MonoBehaviour
{
    Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void IncreaseValue()
    {
        slider.value++;
    }
    public void DecreaseValue()
    {
        slider.value--;
    }
}