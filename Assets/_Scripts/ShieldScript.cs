using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

// Seguir Player e Rotacionar junto
// Controlar "HP" de cada parte
// Decidir quais partes ficam ativas
// Regenerar escudos gradualmente
// Modificar o alpha de cada parte com base no HP atual (full, > 70, > 35, < 35)
public class ShieldScript : MonoBehaviour
{  
    [Header("Shield Objects")]
    [SerializeField] ShieldStrenght frontShield;
    [SerializeField] ShieldStrenght rightShield;
    [SerializeField] ShieldStrenght backShield;
    [SerializeField] ShieldStrenght leftShield;

    [Header("Shield Alpha (1, 2, 3, 4, 5)")]
    [SerializeField] float[] strAlphas;

    PlayerUpgradesManager upgradesManager;
    Transform player;
    Color defaultColor;

    float powerUpExtraStrPerc = 0;
    float PowerUpAddeAlpha = 0;

    void Start()
    {
        player = FindObjectOfType<PlayerMove>().transform;
        upgradesManager = FindObjectOfType<PlayerUpgradesManager>();
        defaultColor = frontShield.GetComponent<SpriteRenderer>().color;

        frontShield = frontShield.GetComponent<ShieldStrenght>();
        rightShield = rightShield.GetComponent<ShieldStrenght>();
        backShield = backShield.GetComponent<ShieldStrenght>();
        leftShield = leftShield.GetComponent<ShieldStrenght>();

        PowerUpAddeAlpha = 0;

        StartCoroutine(ShieldRegen(frontShield));
        StartCoroutine(ShieldRegen(rightShield));
        StartCoroutine(ShieldRegen(backShield));
        StartCoroutine(ShieldRegen(leftShield));
    }

    void Update()
    {
        FollowPlayer();
        SetShieldsValues();
        SetPartsActivateStatus();
        SetAlphas();
    }
    
    IEnumerator ShieldRegen(ShieldStrenght shieldStr)
    {
        do
        {
            yield return new WaitForSeconds(shieldStr.CurrentRegenTime);

            shieldStr.CurrentStr++;
            if (shieldStr.CurrentStr > (int)Mathf.Ceil(shieldStr.MaxStr + shieldStr.MaxStr * (powerUpExtraStrPerc/100)))
                shieldStr.CurrentStr = (int)Mathf.Ceil(shieldStr.MaxStr + shieldStr.MaxStr * (powerUpExtraStrPerc/100));
        } while (true);
    }    

    public void ShieldAlphaSetter(ShieldStrenght shieldStr)
    {
        float str = shieldStr.CurrentStr;
        float alpha;

        if (str <= 0)
            alpha = 0;
        else if (str <= upgradesManager.ShieldUpgradesInfo.StrenghtUpgrades[0].Strenght)
            alpha = strAlphas[0];
        else if (str <= upgradesManager.ShieldUpgradesInfo.StrenghtUpgrades[1].Strenght)
            alpha = strAlphas[1];
        else if (str <= upgradesManager.ShieldUpgradesInfo.StrenghtUpgrades[2].Strenght)
            alpha = strAlphas[2];
        else if (str <= upgradesManager.ShieldUpgradesInfo.StrenghtUpgrades[3].Strenght)
            alpha = strAlphas[3];
        else if (str <= upgradesManager.ShieldUpgradesInfo.StrenghtUpgrades[4].Strenght)
            alpha = strAlphas[4];
        else 
            alpha = strAlphas[5];
        
        alpha += PowerUpAddeAlpha;
        if (alpha > 1) alpha = 1;

        shieldStr.GetComponent<SpriteRenderer>().color = new Color (defaultColor.r, defaultColor.g, defaultColor.b, alpha);
    }

    void SetAlphas()
    {
        if (frontShield.HasStrChanged) ShieldAlphaSetter(frontShield);
        if (rightShield.HasStrChanged) ShieldAlphaSetter(rightShield);
        if (backShield.HasStrChanged) ShieldAlphaSetter(backShield);
        if (leftShield.HasStrChanged) ShieldAlphaSetter(leftShield);
    }


    void SetShieldValues(ShieldStrenght shield, ShieldUpgrades shieldUpgrades)
    {
        shield.MaxStr = upgradesManager.ShieldUpgradesInfo.StrenghtUpgrades[shieldUpgrades.ResistenceLevel - 1].Strenght;
        shield.baseRegenTime = upgradesManager.ShieldUpgradesInfo.RecoveryUpgrades[shieldUpgrades.RecoveryLevel - 1].TimeBetween;
    }
    void SetShieldsValues()
    {
        SetShieldValues(frontShield, upgradesManager.CurrentUpgrades.FrontShieldUpgrades);
        SetShieldValues(rightShield, upgradesManager.CurrentUpgrades.RightShieldUpgrades);
        SetShieldValues(backShield, upgradesManager.CurrentUpgrades.BackShieldUpgrades);
        SetShieldValues(leftShield, upgradesManager.CurrentUpgrades.LeftShieldUpgrades);
    }

    private void SetPartsActivateStatus()
    {
        frontShield.gameObject.SetActive(upgradesManager.CurrentUpgrades.FrontShieldUpgrades.Enabled);
        rightShield.gameObject.SetActive(upgradesManager.CurrentUpgrades.RightShieldUpgrades.Enabled);
        backShield.gameObject.SetActive(upgradesManager.CurrentUpgrades.BackShieldUpgrades.Enabled);
        leftShield.gameObject.SetActive(upgradesManager.CurrentUpgrades.LeftShieldUpgrades.Enabled);
    }

    private void FollowPlayer()
    {
        transform.position = player.position;
        transform.rotation = player.rotation;
    }

    public void PowerUpStart(float regenMod, float extraStrPerc, float addAlpha)
    {
        PowerUpAddeAlpha = addAlpha;
        powerUpExtraStrPerc = extraStrPerc;    
        
        frontShield.RegenMod = regenMod;
        rightShield.RegenMod = regenMod;
        backShield.RegenMod = regenMod;
        leftShield.RegenMod = regenMod;

        frontShield.CurrentStr = (int)Mathf.Ceil(frontShield.MaxStr + frontShield.MaxStr * (extraStrPerc/100));
        rightShield.CurrentStr = (int)Mathf.Ceil(rightShield.MaxStr + rightShield.MaxStr * (extraStrPerc/100));
        backShield.CurrentStr = (int)Mathf.Ceil(backShield.MaxStr + backShield.MaxStr * (extraStrPerc/100));
        leftShield.CurrentStr = (int)Mathf.Ceil(leftShield.MaxStr + leftShield.MaxStr * (extraStrPerc/100));
    }
    public void PowerUpEnd()
    {
        PowerUpAddeAlpha = 0;
        powerUpExtraStrPerc = 0;

        frontShield.RegenMod = 1;
        rightShield.RegenMod = 1;
        backShield.RegenMod = 1;
        leftShield.RegenMod = 1;

        if(frontShield.CurrentStr > frontShield.MaxStr)
            frontShield.CurrentStr = frontShield.MaxStr;
        if(rightShield.CurrentStr > rightShield.MaxStr)
            rightShield.CurrentStr = rightShield.MaxStr;
        if(backShield.CurrentStr > backShield.MaxStr)
            backShield.CurrentStr = backShield.MaxStr ;
        if(leftShield.CurrentStr > leftShield.MaxStr)
            leftShield.CurrentStr = leftShield.MaxStr;
    }
}