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
        StartCoroutine(HealRotine());
    }

    void Update()
    {
        
    }

    IEnumerator HealRotine()
    {
        while (true)
        {
            if (PlayerHP.CurrentHP >= PlayerHP.MaxHP)
                { }
            else if (isFreeHeal)
                PlayerHP.ChangePlayerHP(+1);
            else if (PlayerCollectiblesCount.ExpendResources(HealCost))
                PlayerHP.ChangePlayerHP(+1);

            yield return new WaitForSeconds(currentSecondsBetweenHeal);
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