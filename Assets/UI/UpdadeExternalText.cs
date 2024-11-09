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
        else if (GameManager.CurrentLanguage == Language.Portugu�s)
        {
            textObj.text = textPT;
        }
        else if (GameManager.CurrentLanguage == Language.Espa�ol)
        {
            textObj.text = textESP;
        }
    }

    
}
