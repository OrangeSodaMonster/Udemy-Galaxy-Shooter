using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayModeUI : MonoBehaviour
{
    [SerializeField] Slider hpSlider;
    [SerializeField] TMP_Text hpText;
    [SerializeField] TMP_Text metalText;
    [SerializeField] TMP_Text alloyText;
    [SerializeField] TMP_Text cristalText;
    [SerializeField] TMP_Text condCristalText;    	

    void Update()
    {
        hpSlider.value = PlayerHP.CurrentHP / PlayerHP.MaxHP;
        hpText.text = $"{Mathf.Ceil(PlayerHP.CurrentHP)} / {Mathf.Ceil(PlayerHP.MaxHP)}";
        metalText.text = $"Metal {PlayerCollectiblesCount.MetalAmount}";
        alloyText.text = $"Alloy {PlayerCollectiblesCount.AlloyAmount}";
        cristalText.text = $"E. Cristal {PlayerCollectiblesCount.EnergyCristalAmount}";
        condCristalText.text = $"C. E. Cristal {PlayerCollectiblesCount.CondensedEnergyCristalAmount}";
    }
}