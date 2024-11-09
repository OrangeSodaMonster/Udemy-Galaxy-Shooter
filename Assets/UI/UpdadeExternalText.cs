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
        if(GameManager.CurrentLanguage == Language.English)
        {
            textObj.text = text;
        }
        else if (GameManager.CurrentLanguage == Language.Português)
        {
            textObj.text = textPT;
        }
        else if (GameManager.CurrentLanguage == Language.Español)
        {
            textObj.text = textESP;
        }
    }

    
}
