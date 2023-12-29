using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] float playerMaxHP;

    public static float MaxHP;
    public static float CurrentHP;
    public static float LastFrameHP;

    public bool isInvencible = false;


    void Start()
    {
        MaxHP = playerMaxHP;
        CurrentHP = MaxHP;
        LastFrameHP = CurrentHP;
    }

    private void Update()
    {
        if (CurrentHP == 0)
            PlayerDestructionSequence();       
    }

    void LateUpdate()
    {
        if (LastFrameHP > CurrentHP)
            Debug.Log($"<color=orange>Damage: {(LastFrameHP - CurrentHP)}</color>");
        else if (LastFrameHP < CurrentHP)
            Debug.Log($"<color=green>Heal: {(CurrentHP - LastFrameHP)}</color>");

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
        if (collision.gameObject.layer == 3) { return; } // Invisible walls filter

        if (collision.gameObject.GetComponent<CollisionDamage>() != null
            & lastCollisionHash != collision.gameObject.GetHashCode()
            & !isInvencible)
        {
            ChangePlayerHP(-Mathf.Abs(collision.gameObject.GetComponent<CollisionDamage>().Damage));
            lastCollisionHash = collision.gameObject.GetHashCode();
        }       
    }

    void PlayerDestructionSequence()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    static public void ChangePlayerHP(float value)
    {
        CurrentHP += value;
        if (CurrentHP > MaxHP) CurrentHP = MaxHP;
        else if (CurrentHP <= 0) CurrentHP = 0;
    }
}