using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(SaveButtonScript))]
[RequireComponent(typeof(ButtonScript))]
public class HoldToEraseSave : MonoBehaviour
{
    const float holdTimeToShow = 1f;
    [ShowInInspector, Tooltip("includes time to show")] const float necessaryHoldTime = 4f;
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI sliderText;    

    ButtonScript btScript;
    Button button;
    string defaultSliderText;
    int saveSlot;
    bool hasErased = false;

    private void Awake()
    {
        button = GetComponent<Button>();
        btScript = GetComponent<ButtonScript>();
        btScript.TotalHoldDuration = necessaryHoldTime;

        defaultSliderText = sliderText.text;

        saveSlot = GetComponent<SaveButtonScript>().saveSlot;
    }

    void Update()
    {
        if(EventSystem.current.currentSelectedGameObject != button.gameObject) return;

        if(btScript.HoldingTime >= holdTimeToShow)
        {
            sliderText.text = defaultSliderText + " " + saveSlot;
            slider.gameObject.SetActive(true);
        }
        else
        {
            hasErased = false;
            slider.gameObject.SetActive(false);
        }        

        slider.value = (btScript.HoldingTime - holdTimeToShow) / (btScript.TotalHoldDuration - holdTimeToShow);

        if (btScript.HoldingTime >= btScript.TotalHoldDuration && !hasErased)
        {
            SaveLoad.instance.EraseSave(saveSlot);
            slider.gameObject.SetActive(false);
            hasErased = true;
        } 
    }

}