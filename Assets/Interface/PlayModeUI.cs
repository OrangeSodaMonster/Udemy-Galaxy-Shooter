using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MoreMountains.Tools;

public class PlayModeUI : MonoBehaviour
{
    [SerializeField] MMProgressBar hpBar;
    [SerializeField] TMP_Text hpText;
    [SerializeField] TMP_Text numberOfBombs;
    [SerializeField] TMP_Text metalText;
    [SerializeField] TMP_Text alloyText;
    [SerializeField] TMP_Text cristalText;
    [SerializeField] TMP_Text condCristalText;

    private void Start()
    {
        OnHPChange();
    }

    void Update()
    {        
        numberOfBombs.text = BombScript.BombAmount > 0 ?  $"{BombScript.BombAmount}" : "";
        metalText.text = $"Metal {PlayerCollectiblesCount.MetalAmount}";
        alloyText.text = $"Alloy {PlayerCollectiblesCount.AlloyAmount}";
        cristalText.text = $"E. Cristal {PlayerCollectiblesCount.EnergyCristalAmount}";
        condCristalText.text = $"C. E. Cristal {PlayerCollectiblesCount.CondensedEnergyCristalAmount}";
    }

    public void OnHPChange()
    {
        hpBar.UpdateBar01((float)PlayerHP.CurrentHP / PlayerHP.MaxHP);

        hpText.text = $"{Mathf.Ceil(PlayerHP.CurrentHP)} / {Mathf.Ceil(PlayerHP.MaxHP)}";
    }
    public void BumpHPBar()
    {
        hpBar.ForceBump();
    }
}