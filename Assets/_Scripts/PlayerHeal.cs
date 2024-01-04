using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeal : MonoBehaviour
{
    [SerializeField] float baseSecondsBetweenHeal = 8;
    [SerializeField] ResourceNumber[] HealCost;
    [SerializeField] float currentSecondsBetweenHeal;

    public bool isFreeHeal = false;

    PlayerUpgradesManager upgradesManager;

    void Start()
    {
        upgradesManager = FindObjectOfType<PlayerUpgradesManager>();    
    }

    void Update()
    {
        if(!isFreeHeal)
            currentSecondsBetweenHeal = baseSecondsBetweenHeal - GetHealIntervalReduction();

        // && PlayerHP.LastFrameHP >= PlayerHP.MaxHP
        if (PlayerHP.LastFrameHP > PlayerHP.CurrentHP && !(PlayerHP.LastFrameHP < PlayerHP.MaxHP))
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
                healIntervalReduction += upgradesManager.DroneUpgradesInfo.HealUpgrade[i].ReduceFromHealInterval;
        
        if (upgradesManager.CurrentUpgrades.Drone_2_Upgrades.Enabled)
            for (int i = 0; i < upgradesManager.CurrentUpgrades.Drone_2_Upgrades.HealingLevel; i++)
                healIntervalReduction += upgradesManager.DroneUpgradesInfo.HealUpgrade[i].ReduceFromHealInterval;
        
        if (upgradesManager.CurrentUpgrades.Drone_3_Upgrades.Enabled)
            for (int i = 0; i < upgradesManager.CurrentUpgrades.Drone_3_Upgrades.HealingLevel; i++)
                healIntervalReduction += upgradesManager.DroneUpgradesInfo.HealUpgrade[i].ReduceFromHealInterval;

        return healIntervalReduction;
    }



    IEnumerator HealRotine()
    {
        while (PlayerHP.CurrentHP < PlayerHP.MaxHP)
        {            
            yield return new WaitForSeconds(currentSecondsBetweenHeal);

            if (isFreeHeal)
                PlayerHP.ChangePlayerHP(+10);
            else if (PlayerCollectiblesCount.ExpendResources(HealCost))
                PlayerHP.ChangePlayerHP(+10);
        }
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