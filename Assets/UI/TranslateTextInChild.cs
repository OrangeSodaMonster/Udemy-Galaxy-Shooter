using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TranslateTextInChild : MonoBehaviour
{
    [SerializeField, TextArea(2, 3)] string English = "";
    [SerializeField, TextArea(2, 3)] string Portugues = "";
    [SerializeField, TextArea(2, 3)] string Espanol = "";

    TextMeshProUGUI textObj;

    private void Awake()
    {
        textObj = GetComponentInChildren<TextMeshProUGUI>();

        GameManager.OnLanguageChange.AddListener(ChangeText);
    }
    private void OnDestroy()
    {
        GameManager.OnLanguageChange.RemoveListener(ChangeText);
    }
    private void OnEnable()
    {
        ChangeText();
    }

    void ChangeText()
    {
        if(GameManager.CurrentLanguage == Language.English)
        {
            textObj.text = English;
        }
        else if (GameManager.CurrentLanguage == Language.Español)
        {
            textObj.text = Espanol;
        }
        else if (GameManager.CurrentLanguage == Language.Português)
        {
            textObj.text = Portugues;
        }
    }

    private void OnValidate()
    {
        textObj = GetComponentInChildren<TextMeshProUGUI>();

        string largestText = English;
        if (Portugues != null && largestText.Length < Portugues.Length) largestText = Portugues;
        if (Espanol != null && largestText.Length < Espanol.Length) largestText = Espanol; 

        if(largestText != null && textObj != null)
            textObj.text = largestText;
    }

}
