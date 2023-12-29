using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyHP : MonoBehaviour
{
    [SerializeField] float maxHP = 1.0f;

    float currentHP;
    float lastFrameHP;
    int lastCollisionHash = 0;

    public float CurrentHP { get { return currentHP; } }
	

    void Start()
    {
        currentHP = maxHP;
    }

    void LateUpdate()
    {
        //if (lastFrameHP != currentHP & type == Type.Player)
        //{
        //    Debug.Log("HP Difference: " + (currentHP - lastFrameHP));
        //}

        lastFrameHP = currentHP;
        lastCollisionHash = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Enemy hit by player
        if (collision.GetComponent<PlayerWeaponDamage>() != null & lastCollisionHash != collision.gameObject.GetHashCode())
        {
            currentHP -= collision.GetComponent<PlayerWeaponDamage>().Damage;
            lastCollisionHash = collision.gameObject.GetHashCode();
            if (currentHP <= 0)
            {
                if (GetComponent<AsteroidSplit>() != null)
                    GetComponent<AsteroidSplit>().Split();

                if (GetComponent<EnemyDropDealer>() != null)
                    GetComponent<EnemyDropDealer>().SpawnDrops();

                Destroy(gameObject);
            }
        }
     }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Deal with enemy collision
        
        if(GetComponent<EnemyDropDealer>() != null)
            GetComponent<EnemyDropDealer>().SpawnDrops();
        Destroy(gameObject);
               
    }

    public void ChangeHP(float value)
    {
        currentHP += value;
    }
}