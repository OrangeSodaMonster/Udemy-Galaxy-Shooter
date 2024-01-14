using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOverlayScript : MonoBehaviour
{
    [Header("On Damage")]
    [SerializeField] float damageDuration = .3f;
    [SerializeField] float damageMaxValue = .5f;
    [SerializeField] Ease damageEase = Ease.OutExpo;

    [Header("On Hazard")]
    [SerializeField] float hazardFrequency = 2f;
    [SerializeField] Vector2 hazardMinMaxValues = new(.15f, .3f);
    [SerializeField] Ease hazardEase = Ease.InOutSine;

    [Header("On Heal")]
    [SerializeField] float healDuration = .6f;
    [SerializeField] float healMaxValue = .35f;
    [SerializeField] Ease healEase = Ease.InOutSine;

    Material material;
    int damageValueID = Shader.PropertyToID("_WasHitValue");
    int hazardValueID = Shader.PropertyToID("_InHazardValue");
    int healValueID = Shader.PropertyToID("_HealedValue");

    private void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    private void OnEnable()
    {
        if(TryGetComponent(out EnemyHP enemyHP))
        {
            enemyHP.TookDamage += DamageOverlay;
            enemyHP.Healed += HealOverlay;
        }
    }

    private void OnDisable()
    {
        material.DOKill();
        material.SetFloat(damageValueID, 0);
        material.SetFloat(healValueID, 0);
        material.SetFloat(hazardValueID, 0);

        if (TryGetComponent(out EnemyHP enemyHP))
        {
            enemyHP.TookDamage -= DamageOverlay;
            enemyHP.Healed -= HealOverlay;
        }
    }

    public void DamageOverlay()
    {
        material.SetFloat(damageValueID, 0);
        material.DOFloat(damageMaxValue, damageValueID, damageDuration * 0.5f).SetEase(damageEase).SetLoops(2, LoopType.Yoyo)
             .OnKill(() => material.SetFloat(damageValueID, 0));
    }

    public void HealOverlay()
    {
        material.SetFloat(healValueID, 0);
        material.DOFloat(healMaxValue, healValueID, healDuration * 0.5f).SetEase(healEase).SetLoops(2, LoopType.Yoyo)
            .OnKill(() => material.SetFloat(healValueID, 0));
    }

    public void StartHazardOverlay()
    {
        material.SetFloat(hazardValueID, hazardMinMaxValues[0]);
        material.DOFloat(hazardMinMaxValues[1], hazardValueID, hazardFrequency * 0.5f).SetEase(hazardEase).SetLoops(-1, LoopType.Yoyo)
            .OnKill(() => material.SetFloat(hazardValueID, 0)); ;
    }

    public void StopHazardOverlay()
    {
        material.SetFloat(hazardValueID, 0);
        material.DOKill();
    }

}