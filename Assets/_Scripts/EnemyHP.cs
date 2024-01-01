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

        if (currentHP <= 0)
        {
            if (GetComponent<AsteroidSplit>() != null)
                GetComponent<AsteroidSplit>().Split();

            if (GetComponent<EnemyDropDealer>() != null)
                GetComponent<EnemyDropDealer>().SpawnDrops();

            Destroy(gameObject);
        }

        lastFrameHP = currentHP;
        lastCollisionHash = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Enemy hit by player
        if (collision.GetComponent<PlayerLaserDamage>() != null & lastCollisionHash != collision.gameObject.GetHashCode())
        {
            ChangeHP (-Mathf.Abs(collision.GetComponent<PlayerLaserDamage>().Damage));
            lastCollisionHash = collision.gameObject.GetHashCode();            
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
        if (CurrentHP > maxHP) currentHP = maxHP;
        else if (CurrentHP <= 0) currentHP = 0;
    }

    public void SetHP(float value)
    {
        currentHP = value;
    }
}