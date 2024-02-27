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

    void Update()
    {
        metalText.text = $"{PlayerCollectiblesCount.MetalAmount}";
        metalText.color = InterfaceDataHolder.metalColor;

        alloyText.text = $"{PlayerCollectiblesCount.RareMetalAmount}";
        alloyText.color = InterfaceDataHolder.alloyColor;

        cristalText.text = $"{PlayerCollectiblesCount.EnergyCristalAmount}";
        cristalText.color = InterfaceDataHolder.energyCristalColor;

        condCristalText.text = $"{PlayerCollectiblesCount.CondensedEnergyCristalAmount}";
        condCristalText.color = InterfaceDataHolder.condensedCristalColor;
    }
}