using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ShieldSide
{
    Front, Back, Right, Left,
}
public class ShieldStrenght : MonoBehaviour
{
    [SerializeField] ShieldSide shieldSide;

    public float baseRegenTime = 1;
    public float RegenMod = 1;
    public float CurrentRegenTime;

    public int MaxStr;
    public int CurrentStr;

    int lastFrameStr;

    Collider2D coll;
    ShieldScript shieldScript;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
        shieldScript = transform.parent.GetComponent<ShieldScript>();
    }

    private void OnEnable()
    {
        SetShieldStartingValues(shieldSide);
        CurrentRegenTime = baseRegenTime;
        CurrentStr = MaxStr;
    }

    void Update()
    {
        CurrentRegenTime = baseRegenTime * RegenMod;

        if(CurrentStr != lastFrameStr)
        {
            if (CurrentStr == 0)
                coll.enabled = false;
            else
                coll.enabled = true;

            shieldScript.ShieldAlphaSetter(this);
        }

        lastFrameStr = CurrentStr;
    }

    void SetShieldStartingValues(ShieldUpgrades shieldUpgrades)
    {
        MaxStr = PlayerUpgradesManager.Instance.ShieldUpgradesInfo.StrenghtUpgrades[shieldUpgrades.ResistenceLevel - 1].Strenght;
        baseRegenTime = PlayerUpgradesManager.Instance.ShieldUpgradesInfo.RecoveryUpgrades[shieldUpgrades.RecoveryLevel - 1].TimeBetween;
        Color defaultColor = GetComponent<SpriteRenderer>().color;
        float alpha = PlayerUpgradesManager.Instance.ShieldUpgradesInfo.StrenghtUpgrades[shieldUpgrades.ResistenceLevel - 1].AlphaAtThisStr;
        GetComponent<SpriteRenderer>().color = new(defaultColor.r, defaultColor.g, defaultColor.b, alpha);
    }
    
    void SetShieldStartingValues(ShieldSide shieldSide)
    {
        if (shieldSide == ShieldSide.Front)
            SetShieldStartingValues(PlayerUpgradesManager.Instance.CurrentUpgrades.FrontShieldUpgrades);
        else if (shieldSide == ShieldSide.Back)
            SetShieldStartingValues(PlayerUpgradesManager.Instance.CurrentUpgrades.BackShieldUpgrades);
        else if (shieldSide == ShieldSide.Right)
            SetShieldStartingValues(PlayerUpgradesManager.Instance.CurrentUpgrades.RightShieldUpgrades);
        else if (shieldSide == ShieldSide.Left)
            SetShieldStartingValues(PlayerUpgradesManager.Instance.CurrentUpgrades.LeftShieldUpgrades);
    }

    int lastCollisionHash = 0;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<CollisionWithPlayer>() != null && lastCollisionHash != collision.gameObject.GetHashCode())
        {
            CurrentStr -= collision.gameObject.GetComponent<CollisionWithPlayer>().Damage;
            lastCollisionHash = collision.gameObject.GetHashCode();
        }

        if (CurrentStr <= 0)
        {
            PlayerHP.ChangePlayerHP(-Mathf.Abs(CurrentStr));
            CurrentStr = 0;
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (lastCollisionHash != collision.gameObject.GetHashCode())
        {   
            CurrentStr -= collision.GetComponent<EnemyWeaponDamage>().Damage;
            lastCollisionHash = collision.gameObject.GetHashCode();
            if (CurrentStr <= 0)
            {
                PlayerHP.ChangePlayerHP(-Mathf.Abs(CurrentStr));
                CurrentStr = 0;
            }       
        } 
    }
}