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
    [SerializeField] Image bombIcon;
    

    HpBarSize hpBarSize;

    private void Start()
    {

        hpBarSize = FindObjectOfType<HpBarSize>();
        OnHPChange();

        if(startDisabled)
            gameObject.SetActive(false);

        BombScript.OnChangeBombs.AddListener(SetBombNumText);
        SetBombNumText();
    }

    void Update()
    {      
        if(BombScript.BombAmount <= 0)
        {
            bombIcon.gameObject.SetActive(false);
            numberOfBombs.gameObject.SetActive(false);
        }
        else if (!GameStatus.IsPaused && !bombIcon.gameObject.activeInHierarchy)
        {
            bombIcon.gameObject.SetActive(true);
            numberOfBombs.gameObject.SetActive(true);
        }
    }

    public void SetBombNumText()
    {
        numberOfBombs.text = $"{BombScript.BombAmount}";
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