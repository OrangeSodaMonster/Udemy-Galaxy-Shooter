using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    public static int MaxHP;
    public static int CurrentHP;
    public static int LastFrameHP;
    public static bool DamageTaken = false;
    public static bool HealReceived = false;

    public bool isInvencible = false;
    public static bool s_IsInvencible = false;

    PlayerUpgradesManager upgradesManager;

    void Start()
    {
        upgradesManager =FindObjectOfType<PlayerUpgradesManager>();

        MaxHP = upgradesManager.ShipUpgradesInfo.HP_Upgrade[upgradesManager.CurrentUpgrades.ShipUpgrades.HPLevel - 1].HP;

        CurrentHP = MaxHP;
        LastFrameHP = CurrentHP;
    }

    private void Update()
    {
        s_IsInvencible = isInvencible;

        MaxHP = upgradesManager.ShipUpgradesInfo.HP_Upgrade[upgradesManager.CurrentUpgrades.ShipUpgrades.HPLevel - 1].HP;

        if (CurrentHP == 0)
            PlayerDestructionSequence();       
    }

    void LateUpdate()
    {
        if (LastFrameHP > CurrentHP)
        {
            Debug.Log($"<color=orange>Damage: {(LastFrameHP - CurrentHP)}</color>");
            DamageTaken = true;
            HealReceived = false;
        }
        else if (LastFrameHP < CurrentHP)
        {
            Debug.Log($"<color=green>Heal: {(CurrentHP - LastFrameHP)}</color>");
            HealReceived = true;
            DamageTaken = false;
        }
        else
        {
            HealReceived = false;
            DamageTaken = false;
        }

        LastFrameHP = CurrentHP;
        lastCollisionHash = 0;
    }

    int lastCollisionHash;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyWeaponDamage>() != null
            & lastCollisionHash != collision.gameObject.GetHashCode()
            & !isInvencible)
        {
            ChangePlayerHP(-Mathf.Abs(collision.GetComponent<EnemyWeaponDamage>().Damage));
            lastCollisionHash = collision.gameObject.GetHashCode();            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.layer == 3) { return; } // Invisible walls filter

        //if (collision.gameObject.GetComponent<CollisionWithPlayer>() != null
        //    & lastCollisionHash != collision.gameObject.GetHashCode()
        //    & !isInvencible)
        //{
        //    ChangePlayerHP(-Mathf.Abs(collision.gameObject.GetComponent<CollisionWithPlayer>().Damage));
        //    lastCollisionHash = collision.gameObject.GetHashCode();
        //}       
    }

    void PlayerDestructionSequence()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    static public void ChangePlayerHP(int value)
    {
        if (s_IsInvencible && value < 0) return;

        CurrentHP += value;
        if (CurrentHP > MaxHP) CurrentHP = MaxHP;
        else if (CurrentHP <= 0) CurrentHP = 0;
    }
}