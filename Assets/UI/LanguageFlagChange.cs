using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class LanguageFlagChange : MonoBehaviour
{
    [SerializeField] Image flagImage;

    [SerializeField] List<Sprite> EnglishFlags;
    [SerializeField] List<Sprite> EspanolFlags;
    [SerializeField] List<Sprite> PortuguesFlags;

    private void Awake()
    {
        GameManager.OnLanguageChange.AddListener(ChangeFlag);
    }
    private void OnDestroy()
    {
        GameManager.OnLanguageChange.RemoveListener(ChangeFlag);
    }
    private void OnEnable()
    {
        ChangeFlag();
    }

    void ChangeFlag()
    {
        if(GameManager.CurrentLanguage == Language.English)
        {
            flagImage.sprite = EnglishFlags[Random.Range(0, EnglishFlags.Count)];
        }
        else if (GameManager.CurrentLanguage == Language.Español)
        {
            flagImage.sprite = EspanolFlags[Random.Range(0, EspanolFlags.Count)];
        }
        else if (GameManager.CurrentLanguage == Language.Português)
        {
            flagImage.sprite = PortuguesFlags[Random.Range(0, PortuguesFlags.Count)];
        }
    }
}
