using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldButtonSlider : MonoBehaviour
{
    [SerializeField] Image Image;
    ButtonScript buttonScript;

    private void OnEnable()
    {
        buttonScript = GetComponent<ButtonScript>();
        Image.rectTransform.localScale = new Vector3(0, 1, 1);
    }

    void Update()
    {
        float sliderValue = buttonScript.HoldingTime/buttonScript.TotalHoldDuration;
        Image.rectTransform.localScale = new Vector3(sliderValue, 1, 1);
    }
}
