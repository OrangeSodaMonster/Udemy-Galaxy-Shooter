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

    PlayerUpgradesManager upgradesManager;
    Transform player;
    Color defaultColor;

    float powerUpExtraStrPerc = 0;
    float PowerUpAddeAlpha = 0;

    void Start()
    {
        player = FindObjectOfType<PlayerMove>().transform;
        //upgradesManager = FindObjectOfType<PlayerUpgradesManager>();
        upgradesManager = PlayerUpgradesManager.Instance;
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
    }
    
    IEnumerator ShieldRegen(ShieldStrenght shieldStr)
    {
        do
        {
            yield return new WaitForSeconds(shieldStr.CurrentRegenTime);

            shieldStr.CurrentStr += 5;
            if (shieldStr.CurrentStr > (int)Mathf.Ceil(shieldStr.MaxStr + shieldStr.MaxStr * (powerUpExtraStrPerc/100)))
                shieldStr.CurrentStr = (int)Mathf.Ceil(shieldStr.MaxStr + shieldStr.MaxStr * (powerUpExtraStrPerc/100));
        } while (true);
    }    

    public void ShieldAlphaSetter(ShieldStrenght shieldStr)
    {
        float str = shieldStr.CurrentStr;
        float alpha = 0;

        for (int i = 0; i < upgradesManager.ShieldUpgradesInfo.StrenghtUpgrades.Length; i++)
        {
            if (str >= upgradesManager.ShieldUpgradesInfo.StrenghtUpgrades[i].Strenght)
                alpha = upgradesManager.ShieldUpgradesInfo.StrenghtUpgrades[i].AlphaAtThisStr;
        }
        
        alpha += PowerUpAddeAlpha;
        if (alpha > 1) alpha = 1;

        shieldStr.GetComponent<SpriteRenderer>().color = new Color (defaultColor.r, defaultColor.g, defaultColor.b, alpha);
    }

    public void SetShieldValues(ShieldStrenght shield, ShieldUpgrades shieldUpgrades, ShieldSide type)
    {
        GetValues(type, out int maxStr, out float regenTime);

        shield.MaxStr = maxStr;
        shield.baseRegenTime = regenTime;

        void GetValues(ShieldSide type, out int maxStr, out float regenTime)
        {
            maxStr = 0;
            regenTime = 0;
            switch (type)
            {
                case ShieldSide.Front:
                    maxStr = PlayerStats.Instance.Shields.ShieldFront.CurrentMaxStrenght;
                    regenTime = PlayerStats.Instance.Shields.ShieldFront.CurrentMaxStrenght;
                    break;
                case ShieldSide.Right:
                    maxStr = PlayerStats.Instance.Shields.ShieldRight.CurrentMaxStrenght;
                    regenTime = PlayerStats.Instance.Shields.ShieldRight.CurrentMaxStrenght;
                    break;
                case ShieldSide.Left:
                    maxStr = PlayerStats.Instance.Shields.ShieldLeft.CurrentMaxStrenght;
                    regenTime = PlayerStats.Instance.Shields.ShieldLeft.CurrentMaxStrenght;
                    break;
                case ShieldSide.Back:
                    maxStr = PlayerStats.Instance.Shields.ShieldBack.CurrentMaxStrenght;
                    regenTime = PlayerStats.Instance.Shields.ShieldBack.CurrentMaxStrenght;
                    break;
            }
        } 
    }
    void SetShieldsValues()
    {
        SetShieldValues(frontShield, upgradesManager.CurrentUpgrades.FrontShieldUpgrades, ShieldSide.Front);
        SetShieldValues(rightShield, upgradesManager.CurrentUpgrades.RightShieldUpgrades, ShieldSide.Right);
        SetShieldValues(backShield, upgradesManager.CurrentUpgrades.BackShieldUpgrades, ShieldSide.Back);
        SetShieldValues(leftShield, upgradesManager.CurrentUpgrades.LeftShieldUpgrades, ShieldSide.Left);
    }

    private void SetPartsActivateStatus()
    {
        frontShield.gameObject.SetActive(upgradesManager.CurrentUpgrades.FrontShieldUpgrades.Enabled && !upgradesManager.CurrentUpgrades.FrontShieldUpgrades.DisableOverwrite && !GameStatus.IsPortal);
        rightShield.gameObject.SetActive(upgradesManager.CurrentUpgrades.RightShieldUpgrades.Enabled && !upgradesManager.CurrentUpgrades.RightShieldUpgrades.DisableOverwrite && !GameStatus.IsPortal);
        backShield.gameObject.SetActive(upgradesManager.CurrentUpgrades.BackShieldUpgrades.Enabled && !upgradesManager.CurrentUpgrades.BackShieldUpgrades.DisableOverwrite && !GameStatus.IsPortal);
        leftShield.gameObject.SetActive(upgradesManager.CurrentUpgrades.LeftShieldUpgrades.Enabled && !upgradesManager.CurrentUpgrades.LeftShieldUpgrades.DisableOverwrite && !GameStatus.IsPortal);
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