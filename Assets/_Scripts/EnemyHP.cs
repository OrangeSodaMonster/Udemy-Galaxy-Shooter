using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyHP : MonoBehaviour
{
    public event Action TookDamage;
    public event Action Healed;

    [SerializeField] bool destroyOnCollision = true;
    [field:SerializeField] public int MaxHP { get; private set; } = 1;

    int currentHP;
    int lastFrameHP;

    int lastCollisionHash = 0;
    public float CurrentHP { get { return currentHP; } }
	

    void Start()
    {
        currentHP = MaxHP;
    }

    void Update()
    {
        if (lastFrameHP > currentHP)
        {
            TookDamage?.Invoke();
        }
        else if (lastFrameHP > currentHP)
        {
            Healed?.Invoke();
        }

        if (currentHP <= 0)
        {
            if (TryGetComponent(out AsteroidSplit split))
            {
                if (transform.parent.GetComponent<ObjectiveSpawnArrow>() != null)
                    split.Split(transform.parent);
                else
                    split.Split();
            }

            if (TryGetComponent(out EnemyDropDealer dropDealer))
                dropDealer.SpawnDrops();

            Destroy(gameObject);
        }

        lastFrameHP = currentHP;
        lastCollisionHash = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Enemy hit by player
        if (collision.TryGetComponent(out PlayerLaserDamage laserDamage) & lastCollisionHash != collision.gameObject.GetHashCode())
        {
            ChangeHP (-Mathf.Abs(laserDamage.Damage));
            lastCollisionHash = collision.gameObject.GetHashCode();            
        }
     }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Deal with enemy collision
        
        if(TryGetComponent(out EnemyDropDealer dropDealer))
            dropDealer.SpawnDrops();

        if(destroyOnCollision)
            Destroy(gameObject);
               
    }

    public void ChangeHP(int value)
    {
        currentHP += value;
        if (CurrentHP > MaxHP) currentHP = MaxHP;
        else if (CurrentHP <= 0) currentHP = 0;
    }

    public void SetHP(int value)
    {
        currentHP = value;
    }
}