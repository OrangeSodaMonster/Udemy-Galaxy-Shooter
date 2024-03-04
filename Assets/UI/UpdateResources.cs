using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateResources : MonoBehaviour
{
    [SerializeField] TMP_Text metalText;
    [SerializeField] TMP_Text alloyText;
    [SerializeField] TMP_Text cristalText;
    [SerializeField] TMP_Text condCristalText;

    [SerializeField] InterfaceDataHolder InterfaceDataHolder;

    private void Start()
    {
        PlayerCollectiblesCount.OnPickCollectible.AddListener(UpdateText);
        UpdateText();
    }

    public void UpdateText()
    {
        metalText.text = PlayerCollectiblesCount.MetalAmount.ToString();
        metalText.color = InterfaceDataHolder.metalColor;

        alloyText.text = PlayerCollectiblesCount.RareMetalAmount.ToString();
        alloyText.color = InterfaceDataHolder.alloyColor;

        cristalText.text = PlayerCollectiblesCount.EnergyCristalAmount.ToString();
        cristalText.color = InterfaceDataHolder.energyCristalColor;

        condCristalText.text = PlayerCollectiblesCount.CondensedEnergyCristalAmount.ToString();
        condCristalText.color = InterfaceDataHolder.condensedCristalColor;
    }
}