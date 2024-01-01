using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeal : MonoBehaviour
{
    [SerializeField] float secondsBetweenHeal = 1;
    [SerializeField] ResourceNumber[] HealCost;

    public float currentSecondsBetweenHeal;
    public bool isFreeHeal = false;

    void Start()
    {
        currentSecondsBetweenHeal = secondsBetweenHeal;      
    }

    void Update()
    {
        if (PlayerHP.DamageTaken)
        {
            StartCoroutine(HealRotine());
        }
    }

    IEnumerator HealRotine()
    {
        while (PlayerHP.CurrentHP < PlayerHP.MaxHP)
        {
            yield return new WaitForSeconds(currentSecondsBetweenHeal);

            if (isFreeHeal)
                PlayerHP.ChangePlayerHP(+1);
            else if (PlayerCollectiblesCount.ExpendResources(HealCost))
                PlayerHP.ChangePlayerHP(+1);
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
        currentSecondsBetweenHeal = secondsBetweenHeal;
        isFreeHeal = false;
    }
}