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
    public bool HasStrChanged;

    int lastFrameStr;

    Collider2D coll;
    PlayerUpgradesManager upgradesManager;

    private IEnumerator Start()
    {
        coll = GetComponent<Collider2D>();
        upgradesManager = FindObjectOfType<PlayerUpgradesManager>();
        SetShieldValues(this, shieldSide);
        CurrentRegenTime = baseRegenTime;
        CurrentStr = MaxStr;

        yield return new WaitForEndOfFrame();       

        FindObjectOfType<ShieldScript>().ShieldAlphaSetter(this);
    }

    void LateUpdate()
    {
        CurrentRegenTime = baseRegenTime * RegenMod;

        HasStrChanged = (CurrentStr != lastFrameStr) ? true : false;

        if(HasStrChanged)
        {
            if(CurrentStr == 0 )
            {
                coll.enabled = false;
            }
            else
            {
                coll.enabled = true;
            }
        }

        lastFrameStr = CurrentStr;
    }

    void SetShieldValues(ShieldStrenght shield, ShieldUpgrades shieldUpgrades)
    {
        shield.MaxStr = upgradesManager.ShieldUpgradesInfo.StrenghtUpgrades[shieldUpgrades.ResistenceLevel - 1].Strenght;
        shield.baseRegenTime = upgradesManager.ShieldUpgradesInfo.RecoveryUpgrades[shieldUpgrades.RecoveryLevel - 1].TimeBetween;
    }
    
    public void SetShieldValues(ShieldStrenght shield, ShieldSide shieldSide)
    {
        if (shieldSide == ShieldSide.Front)
            SetShieldValues(shield, upgradesManager.CurrentUpgrades.FrontShieldUpgrades);
        else if (shieldSide == ShieldSide.Back)
            SetShieldValues(shield, upgradesManager.CurrentUpgrades.BackShieldUpgrades);
        else if (shieldSide == ShieldSide.Right)
            SetShieldValues(shield, upgradesManager.CurrentUpgrades.RightShieldUpgrades);
        else if (shieldSide == ShieldSide.Left)
            SetShieldValues(shield, upgradesManager.CurrentUpgrades.LeftShieldUpgrades);
    }

    int lastCollisionHash = 0;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<CollisionDamage>() != null & lastCollisionHash != collision.gameObject.GetHashCode())
        {
            CurrentStr -= collision.gameObject.GetComponent<CollisionDamage>().Damage;
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