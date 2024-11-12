using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

[SelectionBase]
public class PlayerHP : MonoBehaviour
{
    public int MaxHP;
    public int CurrentHP;
    public int LastFrameHP;

    public bool isInvencible = false;
    public static bool s_IsInvencible = false;

    [SerializeField] ShieldScript shields;
    [SerializeField] IonStreamScript ionStream;
    [SerializeField] GameObject drones;
    [SerializeField] TractorBeamScript tractorBeam;
    [SerializeField] BombScript bomb;

    [SerializeField] UnityEvent tookDamage;
    [SerializeField] UnityEvent wasHealed;
    [SerializeField] VisualEffect deathVFX;

    [Header("Shields")]
    [SerializeField] ShieldStrenght frontShield;
    [SerializeField] ShieldStrenght backShield;
    [SerializeField] ShieldStrenght leftShield;
    [SerializeField] ShieldStrenght rightShield;

    PlayerUpgradesManager upgradesManager;

    public static PlayerHP Instance;

    int lastFrameMaxHP;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void OnEnable()
    {
        upgradesManager =FindObjectOfType<PlayerUpgradesManager>();

        MaxHP = upgradesManager.ShipUpgradesInfo.HP_Upgrade[upgradesManager.CurrentUpgrades.ShipUpgrades.HPLevel - 1].HP;
        CurrentHP = MaxHP;
        LastFrameHP = CurrentHP;
        lastFrameMaxHP = MaxHP;

    }

    private void Update()
    {
        s_IsInvencible = isInvencible;

        MaxHP = upgradesManager.ShipUpgradesInfo.HP_Upgrade[upgradesManager.CurrentUpgrades.ShipUpgrades.HPLevel - 1].HP;

        if (CurrentHP == 0)
            StartCoroutine(PlayerDestructionSequence());
    }

    void LateUpdate()
    {
        if (LastFrameHP > CurrentHP)
        {
            Debug.Log($"<color=orange>Damage: {(LastFrameHP - CurrentHP)}</color>");
            tookDamage.Invoke();
        }
        else if (LastFrameHP < CurrentHP)
        {
            Debug.Log($"<color=green>Heal: {(CurrentHP - LastFrameHP)}</color>");
            wasHealed.Invoke();
        }

        LastFrameHP = CurrentHP;
        lastCollisionHash = 0;
        lastFrameMaxHP = MaxHP;
    }

    int lastCollisionHash;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyWeaponDamage>() != null
            & lastCollisionHash != collision.gameObject.GetHashCode()
            & !isInvencible)
        {
            //ChangePlayerHP(-Mathf.Abs(collision.GetComponent<EnemyWeaponDamage>().Damage));
            lastCollisionHash = collision.gameObject.GetHashCode();   
            OnPlayerHit(collision.transform.position, collision.GetComponent<EnemyWeaponDamage>().Damage);
        }
    }

    public void OnPlayerHit(Vector3 hitPos, int damage, bool playSound = false)
    {
        damage = Mathf.Abs(damage);
        float dot = Vector2.Dot(transform.up, (hitPos-transform.position).normalized);
        // 1 na frente
        // -1 atrás
        if (dot > 0.707) // Front
        {
            if (frontShield.gameObject.activeSelf)
                frontShield.OnShieldHit(damage);
            else
                ChangePlayerHP(-damage);
        }
        else if (dot < -0.707) // Back
        {
            if (backShield.gameObject.activeSelf)
                backShield.OnShieldHit(damage);
            else
                ChangePlayerHP(-damage);
        }
        else
        {
            Vector3 cross = Vector3.Cross(transform.up, (hitPos-transform.position).normalized);
            // cross.z > 0 na esquerda
            // cross.z < 0 na direita
            if(cross.z > 0)
            {
                if (leftShield.gameObject.activeSelf)
                    leftShield.OnShieldHit(damage);
                else
                    ChangePlayerHP(-damage);
            }
            else if (cross.z < 0)
            {
                if (rightShield.gameObject.activeSelf)
                    rightShield.OnShieldHit(damage);
                else
                    ChangePlayerHP(-damage);
            }
        }

        if(playSound) AudioManager.Instance.PlayerHitSound.PlayFeedbacks();
    }

    public IEnumerator PlayerDestructionSequence()
    {
        yield return null;

        shields.gameObject.SetActive(false);
        ionStream.gameObject.SetActive(false);
        drones.SetActive(false);
        tractorBeam.gameObject.SetActive(false);
        bomb.gameObject.SetActive(false);

        if(CurrentHP <= 0)
        {
            deathVFX.transform.position = transform.position;
            deathVFX.gameObject.SetActive(true);

            AudioManager.Instance.PlayerDestructionSound.PlayFeedbacks();
        }

        PauseAndUIManager.EnableGameoverCanvas();
        GameStatus.IsGameover = true;
        Destroy(gameObject);
    }

    public void ApplyHPUpgrade()
    {
        int missingHP = MaxHP - CurrentHP;
        MaxHP = PlayerUpgradesManager.Instance.ShipUpgradesInfo.HP_Upgrade[PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.HPLevel - 1].HP;
        CurrentHP = MaxHP - missingHP;
    }

    public void ChangePlayerHP(int value, bool ignoreInvencibility = false, bool playHitSound = false)
    {
        if(value < 0 && GameManager.CombatLog != null)
            GameManager.CombatLog.TotalDamageTaken += (int)MathF.Min(Mathf.Abs(value), CurrentHP);

        MaxHP = PlayerUpgradesManager.Instance.ShipUpgradesInfo.HP_Upgrade[PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.HPLevel - 1].HP;

        if (s_IsInvencible && !ignoreInvencibility && value < 0) return;

        CurrentHP += value;
        if (CurrentHP > MaxHP) CurrentHP = MaxHP;
        else if (CurrentHP <= 0) CurrentHP = 0;

        if (playHitSound)
        {
            AudioManager.Instance.PlayerHitSound.PlayFeedbacks();
        }
    }
}