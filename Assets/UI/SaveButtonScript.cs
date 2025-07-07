using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveButtonScript : MonoBehaviour
{
    [Range(1,4)] public int saveSlot;
    [SerializeField] Color hasSaveColor = Color.white;
    [SerializeField] Color hasSaveTextColor = Color.cyan;
    [SerializeField] Color activeSaveColor = Color.white;
    [SerializeField] Color activeSaveTextColor = Color.green;

    Image image;
    Color defaultColor;
    TextMeshProUGUI text;
    Color defaultTextColor;
    string defaultText = string.Empty;


    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        defaultText = text.text;
        defaultTextColor = text.color;

        image = GetComponent<Image>();
        defaultColor = image.color;
    }

    void OnEnable()
    {
        SaveLoad.instance.ChangedSaveSlot += UpdateVisual;

        UpdateVisual();
    }

    private void OnDisable()
    {
        SaveLoad.instance.ChangedSaveSlot -= UpdateVisual;        
    }

    private void UpdateVisual()
    {
        image.color = defaultColor;
        text.color = defaultTextColor;
        text.text = defaultText;

        if (SaveLoad.instance.TryGetSlot(saveSlot))
        {
            text.text = $"{defaultText}\n{SaveLoad.instance.GetCreationDate(saveSlot)}";

            image.color = hasSaveColor;
            text.color = hasSaveTextColor;
        }

        if (SaveLoad.instance.CurrentSaveSlot == saveSlot)
        {
            text.color = activeSaveTextColor;
        }
    }
}