using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopButtonScript : MonoBehaviour
{
    [SerializeField] int stageToUnlock;
    [SerializeField] Color unavaliableColor;
    [SerializeField] Color unavaliableTextColor;
    [SerializeField] Image border;

    Image buttonImage;
    TextMeshProUGUI text;
    Button button;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
    }

    void Start()
    {
        if (stageToUnlock > GameManager.HighestStageCleared)
        {
            buttonImage.color = unavaliableColor;
            text.color = unavaliableTextColor;
            border.enabled = false;
            button.enabled = false;
        }
        else
        {
            button.enabled = true;
        }
    }
}