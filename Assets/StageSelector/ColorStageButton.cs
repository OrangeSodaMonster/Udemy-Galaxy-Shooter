using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorStageButton : MonoBehaviour
{
    [SerializeField] int stageNum;
    [SerializeField] Color clearedColor;
    [SerializeField] Color clearedTextColor;
    [SerializeField] Color unavaliableColor;
    [SerializeField] Color unavaliableTextColor;

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
        if(GameManager.HighestStageCleared >= stageNum)
        {
            buttonImage.color = clearedColor;
            text.color = clearedTextColor;
        }

        if(stageNum > GameManager.HighestStageCleared + 1)
        {
            buttonImage.color = unavaliableColor;
            text.color = unavaliableTextColor;
            button.enabled = false;
        }
        else
        {
            button.enabled = true;
        }
    }
}