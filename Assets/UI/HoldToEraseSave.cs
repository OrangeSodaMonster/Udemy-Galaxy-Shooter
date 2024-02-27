using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(SaveButtonScript))]
public class HoldToEraseSave : MonoBehaviour
{
    [SerializeField] float necessaryHoldTime = 5f;
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI sliderText;
    
    bool isHolding;
    float currentHoldTime = 0;
    Button button;
    string defaultSliderText;
    int saveSlot;

    bool hasErased = false;

    private void Awake()
    {
        button = GetComponent<Button>();
        defaultSliderText = sliderText.text;

        saveSlot = GetComponent<SaveButtonScript>().saveSlot;
    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != button.gameObject || !SaveLoad.instance.TryGetConfig(saveSlot)) return;

        if (InputHolder.Instance.IsDisableUI)
            isHolding = true; 
        else
            isHolding = false;

        if (isHolding)
        {
            currentHoldTime += Time.deltaTime;
        }
        else
        {
            currentHoldTime = 0;
            hasErased = false;
        }

        slider.value = currentHoldTime / necessaryHoldTime;

        if(slider.gameObject.activeInHierarchy && slider.value <= 0.05f)
            slider.gameObject.SetActive(false);
        else if (!slider.gameObject.activeInHierarchy && slider.value > 0.05f)
        {
            sliderText.text = defaultSliderText + " " + saveSlot;
            slider.gameObject.SetActive(true);
        }

        if(currentHoldTime >= necessaryHoldTime && !hasErased)
        {
            SaveLoad.instance.EraseSave(saveSlot);
            slider.gameObject.SetActive(false);
            hasErased = true;
        }
    }
}