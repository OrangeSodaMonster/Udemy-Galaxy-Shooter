using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class EnemyHP : MonoBehaviour
{
    public event Action TookDamage;
    public event Action Healed;
    public event Action Died;
    //public int OnBirthDamage;

    [field:SerializeField] public bool IsAsteroid { get; private set; } = false;
    [SerializeField] float asteroidVolumeMultiplier = 1;
    [SerializeField] bool destroyOnCollision = true;
    public int MaxHP;
    [SerializeField, Range(0, 1)] float startingHpPerc = 1;

    int currentHP;
    int lastFrameHP;

    int lastCollisionHash = 0;
    public float CurrentHP { get { return currentHP; } }
    Vector3 defaultScale = new();

    private void Awake()
    {
        defaultScale = transform.localScale;
    }

    void OnEnable()
    {
        //currentHP = MaxHP - OnBirthDamage;
        currentHP = (int)Mathf.Ceil(MaxHP * startingHpPerc);
        //Debug.Log("Birth Damage= " + OnBirthDamage);
        lastFrameHP = MaxHP;
        transform.localScale = defaultScale;
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
                //if (transform.parent != null && transform.parent.GetComponent<ObjectiveSpawnArrow>() != null)
                //    split.SplitParented(currentHP, transform.parent);
                //else
                split.Split(currentHP);
            }

            if (TryGetComponent(out EnemyDropDealer dropDealer))
                dropDealer.SpawnDrops();

            Died?.Invoke();
            DestroySequence();
        }

        lastFrameHP = currentHP;
        lastCollisionHash = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Enemy hit by player
        if (collision.TryGetComponent(out PlayerLaserDamage laserDamage) && lastCollisionHash != collision.gameObject.GetHashCode())
        {
            if (GameManager.CombatLog != null)
            {
                UpdateCombatLog(laserDamage);
            }

            ChangeHP (-Mathf.Abs(laserDamage.Damage));
            lastCollisionHash = collision.gameObject.GetHashCode(); 
        }
        //Enemy hit by enemy
        else if (IsAsteroid && collision.TryGetComponent(out EnemyWeaponDamage projectileDamage) && collision.GetComponent<LaserMove>().SourceHash != gameObject.GetHashCode() &&
            lastCollisionHash != collision.gameObject.GetHashCode())
        {
            ChangeHP(-Mathf.Abs(projectileDamage.Damage));
            lastCollisionHash = collision.gameObject.GetHashCode();
        }
     }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Deal with enemy collision     

        if (destroyOnCollision)
        {
            if (TryGetComponent(out EnemyDropDealer dropDealer))
                dropDealer.SpawnDrops();

            Died?.Invoke();
            DestroySequence();
        }
               
    }

    public void DestroySequence()
    {
        if(GameManager.ScoreHolder != null && CurrentHP <= 0)
        {
            GameManager.ScoreHolder.UpdateScoreKilledEnemy(MaxHP, IsAsteroid);
        }

        if (!IsAsteroid && CurrentHP <= 0)
            AudioManager.Instance.EnemyDestructionSound.PlayFeedbacks();
        else if (IsAsteroid && CurrentHP <= 0)
            AudioManager.Instance.PlayAsteroidSound(asteroidVolumeMultiplier);

        gameObject.SetActive(false);
    }

    public void ChangeHP(int value)
    {
        currentHP += value;
        if (CurrentHP > MaxHP) currentHP = MaxHP;
        //else if (CurrentHP <= 0) currentHP = 0;
    }

    public void SetHP(int value)
    {
        currentHP = value;
    }

    void UpdateCombatLog(PlayerLaserDamage laserDamage)
    {
        switch (laserDamage.LaserType)
        {
            case LaserType.Frontal:
                GameManager.CombatLog.FrontalLasersTotalDamage += (int)MathF.Min(Mathf.Abs(laserDamage.Damage), CurrentHP);
                break;
            case LaserType.Spread:
                GameManager.CombatLog.SpreadLasersTotalDamage += (int)MathF.Min(Mathf.Abs(laserDamage.Damage), CurrentHP);
                break;
            case LaserType.Lateral:
                GameManager.CombatLog.LateralLasersTotalDamage += (int)MathF.Min(Mathf.Abs(laserDamage.Damage), CurrentHP);
                break;
            case LaserType.Back:
                GameManager.CombatLog.BackLasersTotalDamage += (int)MathF.Min(Mathf.Abs(laserDamage.Damage), CurrentHP);
                break;
        }
    }
}