using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Seguir Player e Rotacionar junto
// Controlar "HP" de cada parte
// Decidir quais partes ficam ativas
// Regenerar escudos gradualmente
// Modificar o alpha de cada parte com base no HP atual (full, > 70, > 35, < 35)
public class ShieldScript : MonoBehaviour
{
    [Header("Which parts are unlocked")]
    public bool IsNorthUnlocked;
    public bool IsEastUnlocked;
    public bool IsSouthUnlocked;
    public bool IsWestUnlocked;

    [Header("Shield Objects")]
    [SerializeField] GameObject northShield;
    [SerializeField] GameObject eastShield;
    [SerializeField] GameObject southShield;
    [SerializeField] GameObject westShield;    

    [Header("Shield Alpha (full, > 70, > 35, < 35)")]
    [SerializeField] Vector4 strAlphas;

    Transform player;
    ShieldStrenght northShieldStr;
    ShieldStrenght eastShieldStr;
    ShieldStrenght southShieldStr;
    ShieldStrenght westShieldStr;

    float powerUpExtraStrPerc = 0;
    float overMaxPowerUpAlpha;

    void Start()
    {
        player = FindObjectOfType<PlayerMove>().transform;
        northShieldStr = northShield.GetComponent<ShieldStrenght>();
        eastShieldStr = eastShield.GetComponent<ShieldStrenght>();
        southShieldStr = southShield.GetComponent<ShieldStrenght>();
        westShieldStr = westShield.GetComponent<ShieldStrenght>();

        overMaxPowerUpAlpha = strAlphas[0];

        StartCoroutine(ShieldRegen(northShieldStr));
        StartCoroutine(ShieldRegen(eastShieldStr));
        StartCoroutine(ShieldRegen(southShieldStr));
        StartCoroutine(ShieldRegen(westShieldStr));
    }

    void Update()
    {
        FollowPlayer();
        SetPartsActivateStatus();

        if (northShieldStr.HasStrChanged) ShieldAlphaSetter(northShieldStr);
        if (eastShieldStr.HasStrChanged) ShieldAlphaSetter(eastShieldStr);
        if (southShieldStr.HasStrChanged) ShieldAlphaSetter(southShieldStr);
        if (westShieldStr.HasStrChanged) ShieldAlphaSetter(westShieldStr);
    }

    
    IEnumerator ShieldRegen(ShieldStrenght shieldStr)
    {
        do
        {
            yield return new WaitForSeconds(shieldStr.CurrentRegenTime);

            shieldStr.CurrentStr++;
            if (shieldStr.CurrentStr > shieldStr.MaxStr + shieldStr.MaxStr * (powerUpExtraStrPerc/100))
                shieldStr.CurrentStr = shieldStr.MaxStr + shieldStr.MaxStr * (powerUpExtraStrPerc/100);

        } while (true);
    }
    
    void ShieldAlphaSetter(ShieldStrenght shieldStr)
    {
        float strPerc = shieldStr.CurrentStr / shieldStr.MaxStr * 100;
        float alpha;

        if (strPerc > 100)
            alpha = overMaxPowerUpAlpha;
        else if (strPerc >= 100)
            alpha = strAlphas[0];
        else if (strPerc >= 70)
            alpha = strAlphas[1];
        else if (strPerc >= 35)
            alpha = strAlphas[2];
        else
            alpha = strAlphas[3];

        shieldStr.GetComponent<SpriteRenderer>().color = new Color (1, 1, 1, alpha);
    }

    private void SetPartsActivateStatus()
    {
        if (IsNorthUnlocked & northShieldStr.CurrentStr > 0)
        {
            northShield.SetActive(true);
        }
        else if (northShieldStr.CurrentStr > 0)
            northShield.SetActive(false);

        if (IsEastUnlocked & eastShieldStr.CurrentStr > 0)
        {
            eastShield.SetActive(true);
        }
        else if (eastShieldStr.CurrentStr > 0)
            eastShield.SetActive(false);

        if (IsSouthUnlocked & southShieldStr.CurrentStr > 0)
        {
            southShield.SetActive(true);
        }
        else if (southShieldStr.CurrentStr > 0)
            southShield.SetActive(false);

        if (IsWestUnlocked & westShieldStr.CurrentStr > 0)
        {
            westShield.SetActive(true);
        }
        else if (westShieldStr.CurrentStr > 0)
            westShield.SetActive(false);
    }

    private void FollowPlayer()
    {
        transform.position = player.position;
        transform.rotation = player.rotation;
    }

    public void PowerUpStart(float regenMod, float extraStrPerc, float overMaxAlpha)
    {
        overMaxPowerUpAlpha = overMaxAlpha;
        powerUpExtraStrPerc = extraStrPerc;    
        
        northShieldStr.RegenMod = regenMod;
        eastShieldStr.RegenMod = regenMod;
        southShieldStr.RegenMod = regenMod;
        westShieldStr.RegenMod = regenMod;

        northShieldStr.CurrentStr = northShieldStr.MaxStr + northShieldStr.MaxStr * (extraStrPerc/100);
        eastShieldStr.CurrentStr = eastShieldStr.MaxStr + eastShieldStr.MaxStr * (extraStrPerc/100);
        southShieldStr.CurrentStr = southShieldStr.MaxStr + southShieldStr.MaxStr * (extraStrPerc/100);
        westShieldStr.CurrentStr = westShieldStr.MaxStr + westShieldStr.MaxStr * (extraStrPerc/100);
    }
    public void PowerUpEnd()
    {
        overMaxPowerUpAlpha = strAlphas[0];
        powerUpExtraStrPerc = 0;

        northShieldStr.RegenMod = 1;
        eastShieldStr.RegenMod = 1;
        southShieldStr.RegenMod = 1;
        westShieldStr.RegenMod = 1;

        if(northShieldStr.CurrentStr > northShieldStr.MaxStr)
            northShieldStr.CurrentStr = northShieldStr.MaxStr;
        if(eastShieldStr.CurrentStr > eastShieldStr.MaxStr)
            eastShieldStr.CurrentStr = eastShieldStr.MaxStr;
        if(southShieldStr.CurrentStr > southShieldStr.MaxStr)
            southShieldStr.CurrentStr = southShieldStr.MaxStr ;
        if(westShieldStr.CurrentStr > westShieldStr.MaxStr)
            westShieldStr.CurrentStr = westShieldStr.MaxStr;
    }
}