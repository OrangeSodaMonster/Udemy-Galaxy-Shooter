using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHeal : MonoBehaviour
{
    [SerializeField] float baseSecondsBetweenHeal = 8;
    [SerializeField] ResourceNumber[] HealCost;
    [SerializeField] float currentSecondsBetweenHeal;
    public float CurrentSecondsBetweenHeal => currentSecondsBetweenHeal;

    public bool isFreeHeal = false;
    [HideInInspector] public UnityEvent OnHealTimeChange = new();
    float lastSecondsBetweenHeal;

    PlayerUpgradesManager upgradesManager;
    bool isHealing;

    void Start()
    {
        upgradesManager = FindObjectOfType<PlayerUpgradesManager>();    
    }

    void Update()
    {
        if (!isFreeHeal)
        {
            currentSecondsBetweenHeal = baseSecondsBetweenHeal - GetHealIntervalReduction();
            if(currentSecondsBetweenHeal != lastSecondsBetweenHeal)
            {
                OnHealTimeChange.Invoke();
            }
            lastSecondsBetweenHeal = currentSecondsBetweenHeal;
        }

        // && PlayerHP.LastFrameHP >= PlayerHP.MaxHP
        if (PlayerHP.Instance.CurrentHP <= PlayerHP.Instance.MaxHP - 5 && !isHealing)
        {
            //Debug.Log("Started heal rotine");
            StartCoroutine(HealRotine());
        }
    }

    float GetHealIntervalReduction()
    {
        float healIntervalReduction = 0;

        if(upgradesManager.CurrentUpgrades.Drone_1_Upgrades.Enabled)
            for (int i = 0; i < upgradesManager.CurrentUpgrades.Drone_1_Upgrades.HealingLevel; i++)
                healIntervalReduction += upgradesManager.DroneUpgradesInfo.HealUpgrades[i].ReduceFromHealInterval;
        
        if (upgradesManager.CurrentUpgrades.Drone_2_Upgrades.Enabled)
            for (int i = 0; i < upgradesManager.CurrentUpgrades.Drone_2_Upgrades.HealingLevel; i++)
                healIntervalReduction += upgradesManager.DroneUpgradesInfo.HealUpgrades[i].ReduceFromHealInterval;
        
        if (upgradesManager.CurrentUpgrades.Drone_3_Upgrades.Enabled)
            for (int i = 0; i < upgradesManager.CurrentUpgrades.Drone_3_Upgrades.HealingLevel; i++)
                healIntervalReduction += upgradesManager.DroneUpgradesInfo.HealUpgrades[i].ReduceFromHealInterval;

        return healIntervalReduction;
    }

    IEnumerator HealRotine()
    {
        while (PlayerHP.Instance.CurrentHP <= PlayerHP.Instance.MaxHP - 5)
        {
            isHealing = true;

            //Debug.Log(currentSecondsBetweenHeal);
            yield return new WaitForSeconds(currentSecondsBetweenHeal);

            if (isFreeHeal)
                Heal();
            else if (PlayerCollectiblesCount.ExpendResources(HealCost))
                Heal();
        }
        isHealing = false;
    }

    void Heal()
    {
        PlayerHP.Instance.ChangePlayerHP(+5);
        AudioManager.Instance.ShipFix.PlayFeedbacks();
    }

    public void PowerUpStart(float healCD)
    {
        currentSecondsBetweenHeal = healCD;
        isFreeHeal = true;

        StartCoroutine(HealRotine());
    }

    public void PowerUpEnd()
    {
        currentSecondsBetweenHeal = baseSecondsBetweenHeal - GetHealIntervalReduction();
        isFreeHeal = false;
    }
}