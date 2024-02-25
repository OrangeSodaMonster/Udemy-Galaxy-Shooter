using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MoreMountains.Tools;

public class PlayModeUI : MonoBehaviour
{
    [SerializeField] bool startDisabled = true;
    [SerializeField] MMProgressBar hpBar;
    [SerializeField] TMP_Text hpText;
    [SerializeField] TMP_Text numberOfBombs;
    [SerializeField] TMP_Text metalText;
    [SerializeField] TMP_Text alloyText;
    [SerializeField] TMP_Text cristalText;
    [SerializeField] TMP_Text condCristalText;

    HpBarSize hpBarSize;

    private void Start()
    {

        hpBarSize = FindObjectOfType<HpBarSize>();
        OnHPChange();

        if(startDisabled)
            gameObject.SetActive(false);
    }

    void Update()
    {        
        numberOfBombs.text = BombScript.BombAmount > 0 ?  $"{BombScript.BombAmount}" : "";
        metalText.text = $"Metal {PlayerCollectiblesCount.MetalAmount}";
        alloyText.text = $"Alloy {PlayerCollectiblesCount.RareMetalAmount}";
        cristalText.text = $"E. Cristal {PlayerCollectiblesCount.EnergyCristalAmount}";
        condCristalText.text = $"C. E. Cristal {PlayerCollectiblesCount.CondensedEnergyCristalAmount}";        
    }

    public void OnHPChange()
    {
        hpBar.UpdateBar01((float)PlayerHP.Instance.CurrentHP / PlayerHP.Instance.MaxHP);
        hpText.text = $"{PlayerHP.Instance.CurrentHP} / {PlayerHP.Instance.MaxHP}";

        hpBarSize.SetHpBarSize(PlayerHP.Instance.MaxHP);
    }
    public void BumpHPBar()
    {
        hpBar.ForceBump();
    }

    public void EnableUI()
    {
        gameObject.SetActive(true);
    }
}