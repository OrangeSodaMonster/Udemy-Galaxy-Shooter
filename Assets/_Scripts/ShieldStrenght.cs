using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShieldStrenght : MonoBehaviour
{
    [Header("Base Strenght")]
    [SerializeField] float shieldBaseStr = 4;

    [Header("Seconds to regenerate 1 str")]
    [SerializeField] float baseRegenerateTime = 8;

    public float RegenMod = 1;

    public float CurrentStr;
    public float MaxStr;
    public float CurrentRegenTime;
    public bool HasStrChanged;

    float lastFrameStr;

    void Start()
    {
        CurrentStr = shieldBaseStr;
        MaxStr = shieldBaseStr;
        CurrentRegenTime = baseRegenerateTime;
    }

    void LateUpdate()
    {
        CurrentRegenTime = baseRegenerateTime * RegenMod;

        HasStrChanged = (CurrentStr != lastFrameStr) ? true : false;

        lastFrameStr = CurrentStr;
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
            PlayerHP.ChangePlayerHP(CurrentStr);
            CurrentStr = 0;
            gameObject.SetActive(false);
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
                PlayerHP.ChangePlayerHP(CurrentStr);
                CurrentStr = 0;
                gameObject.SetActive(false);
            }       
        } 
    }
}