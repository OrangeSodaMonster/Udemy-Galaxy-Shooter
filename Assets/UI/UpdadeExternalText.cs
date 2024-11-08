using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdadeExternalText : MonoBehaviour
{
    [SerializeField] string text;
    [SerializeField] string textPT;
    [SerializeField] string textESP;
    [SerializeField] TextMeshProUGUI textObj;

    public void UpdateText()
    {
        Debug.Log("Stage Name Update");
        textObj.text = text;
    }

    
}
