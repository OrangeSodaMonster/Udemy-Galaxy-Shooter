using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StageButtonScript : MonoBehaviour
{
    [SerializeField] int stageNum;
    [SerializeField] Color clearedColor;
    [SerializeField] Color clearedTextColor;
    [SerializeField] Color unavaliableColor;
    [SerializeField] Color unavaliableTextColor;
    [Space]
    [SerializeField] UnityEvent stageB_call;

    Image buttonImage;
    TextMeshProUGUI text;
    Button button;
    ButtonScript buttonScript;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
        buttonScript = GetComponent<ButtonScript>();
    }

    void Start()
    {
        if(GameManager.HighestStageCleared >= stageNum)
        {
            buttonImage.color = clearedColor;
            text.color = clearedTextColor;

            if(stageB_call.GetPersistentEventCount() > 0)
                buttonScript.SetAlternativeClickEvents(true, CallStageB, true);
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

    public void CallStageB()
    {
        stageB_call.Invoke();
        Debug.Log("Call StageB");
    }
}