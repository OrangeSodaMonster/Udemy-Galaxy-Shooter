using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHP : MonoBehaviour
{
    public static int MaxHP;
    public static int CurrentHP;
    public static int LastFrameHP;

    public bool isInvencible = false;
    public static bool s_IsInvencible = false;

    [SerializeField] ShieldScript shields;
    [SerializeField] IonStreamScript ionStream;
    [SerializeField] GameObject drones;
    [SerializeField] TractorBeamScript tractorBeam;
    [SerializeField] BombScript bomb;

    [SerializeField] UnityEvent tookDamage;
    [SerializeField] UnityEvent wasHealed;
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
            tookDamage.Invoke();
        }
        else if (LastFrameHP < CurrentHP)
        {
            Debug.Log($"<color=green>Heal: {(CurrentHP - LastFrameHP)}</color>");
            wasHealed.Invoke();
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

    public void PlayerDestructionSequence()
    {
        shields.gameObject.SetActive(false);
        ionStream.gameObject.SetActive(false);
        drones.SetActive(false);
        tractorBeam.gameObject.SetActive(false);
        bomb.gameObject.SetActive(false);

        UIManager.EnableGameoverCanvas();
        Destroy(gameObject);
    }

    static public void ChangePlayerHP(int value)
    {
        MaxHP = PlayerUpgradesManager.Instance.ShipUpgradesInfo.HP_Upgrade[PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.HPLevel - 1].HP;

        if (s_IsInvencible && value < 0) return;

        CurrentHP += value;
        if (CurrentHP > MaxHP) CurrentHP = MaxHP;
        else if (CurrentHP <= 0) CurrentHP = 0;
    }
}