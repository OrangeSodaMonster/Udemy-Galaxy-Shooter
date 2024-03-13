using DG.Tweening;
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
    [Header("On Damage")]
    [SerializeField] float hitDuration = .3f;
    [SerializeField] float hitOverlayMaxValue = 1f;
    [SerializeField] float hitGlowMaxValue = 3f;
    [SerializeField] Ease hitEase = Ease.OutExpo;
    float defaultGlow;

    public float baseRegenTime = 1;
    public float RegenMod = 1;
    public float CurrentRegenTime;

    public int MaxStr;
    public int CurrentStr;

    int lastFrameStr;

    Collider2D coll;
    ShieldScript shieldScript;
    Material mat;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
        mat = GetComponent<SpriteRenderer>().material;
        shieldScript = transform.parent.GetComponent<ShieldScript>();
        defaultGlow = mat.GetFloat("_Glow");
    }

    private void OnEnable()
    {
        SetShieldStartingValues(shieldSide);
        CurrentRegenTime = baseRegenTime;
        CurrentStr = MaxStr;
    }

    void Update()
    {
        CurrentRegenTime = baseRegenTime * RegenMod;

        if(CurrentStr != lastFrameStr)
        {
            if (CurrentStr == 0)
                coll.enabled = false;
            else
                coll.enabled = true;

            shieldScript.ShieldAlphaSetter(this);

            if(CurrentStr > lastFrameStr)
                AudioManager.Instance.ShieldUpSound.PlayFeedbacks();
        }

        lastFrameStr = CurrentStr;
    }

    void SetShieldStartingValues(ShieldUpgrades shieldUpgrades)
    {
        MaxStr = PlayerUpgradesManager.Instance.ShieldUpgradesInfo.StrenghtUpgrades[shieldUpgrades.ResistenceLevel - 1].Strenght;
        baseRegenTime = PlayerUpgradesManager.Instance.ShieldUpgradesInfo.RecoveryUpgrades[shieldUpgrades.RecoveryLevel - 1].TimeBetween;
        Color defaultColor = GetComponent<SpriteRenderer>().color;
        float alpha = PlayerUpgradesManager.Instance.ShieldUpgradesInfo.StrenghtUpgrades[shieldUpgrades.ResistenceLevel - 1].AlphaAtThisStr;
        GetComponent<SpriteRenderer>().color = new(defaultColor.r, defaultColor.g, defaultColor.b, alpha);
    }
    
    void SetShieldStartingValues(ShieldSide shieldSide)
    {
        if (shieldSide == ShieldSide.Front)
            SetShieldStartingValues(PlayerUpgradesManager.Instance.CurrentUpgrades.FrontShieldUpgrades);
        else if (shieldSide == ShieldSide.Back)
            SetShieldStartingValues(PlayerUpgradesManager.Instance.CurrentUpgrades.BackShieldUpgrades);
        else if (shieldSide == ShieldSide.Right)
            SetShieldStartingValues(PlayerUpgradesManager.Instance.CurrentUpgrades.RightShieldUpgrades);
        else if (shieldSide == ShieldSide.Left)
            SetShieldStartingValues(PlayerUpgradesManager.Instance.CurrentUpgrades.LeftShieldUpgrades);
    }

    int lastCollisionHash = 0;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<CollisionWithPlayer>() != null && lastCollisionHash != collision.gameObject.GetHashCode())
        {
            HitFX();
            CurrentStr -= collision.gameObject.GetComponent<CollisionWithPlayer>().Damage;
            lastCollisionHash = collision.gameObject.GetHashCode();
            StartCoroutine(CleanLastHit());
        }

        if (CurrentStr <= 0)
        {
            PlayerHP.Instance.ChangePlayerHP(-Mathf.Abs(CurrentStr));
            CurrentStr = 0;
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (lastCollisionHash != collision.gameObject.GetHashCode())
        {            
            if (collision.gameObject.TryGetComponent(out EnemyWeaponDamage weaponDamage))
            {
                HitFX();                
                CurrentStr -= weaponDamage.Damage;
                lastCollisionHash = collision.gameObject.GetHashCode();
                StartCoroutine(CleanLastHit());                
            }
            if (CurrentStr <= 0)
            {
                PlayerHP.Instance.ChangePlayerHP(-Mathf.Abs(CurrentStr));
                CurrentStr = 0;
            }       
        } 
    }

    WaitForSeconds waitClean = new WaitForSeconds(.15f);
    IEnumerator CleanLastHit()
    {
        yield return waitClean;
        lastCollisionHash = 0;
    }

    public void DamageStrenght(int value)
    {
        CurrentStr -= Mathf.Abs(value);
        HitFX();

        if (CurrentStr <= 0)
        {
            PlayerHP.Instance.ChangePlayerHP(-Mathf.Abs(CurrentStr));
            CurrentStr = 0;
        }
    }

    void HitFX()
    {
        mat.SetFloat("_ShieldHitColor", 0);
        mat.DOFloat(hitOverlayMaxValue, "_ShieldHitColor", hitDuration * 0.5f).SetEase(hitEase).SetLoops(2, LoopType.Yoyo)
             .OnKill(() => mat.SetFloat("_ShieldHitColor", 0));
        mat.SetFloat("_Glow", defaultGlow);
        mat.DOFloat(hitGlowMaxValue, "_Glow", hitDuration * 0.5f).SetEase(hitEase).SetLoops(2, LoopType.Yoyo)
             .OnKill(() => mat.SetFloat("_Glow", defaultGlow));

        AudioManager.Instance.ShieldHitSound.PlayFeedbacks();
    }

    WaitForSecondsRealtime updateStrWait = new WaitForSecondsRealtime(.1f);
    public void UpgradeStr()
    {
        StartCoroutine(UpgradeDelay());

        IEnumerator UpgradeDelay()
        {
            yield return updateStrWait;

            CurrentStr = MaxStr;
        }
    }
    
}