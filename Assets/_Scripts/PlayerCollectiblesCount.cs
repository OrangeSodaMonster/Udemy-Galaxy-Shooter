using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    MetalCrumb,
    Metal,
    RareMetalCrumb,
    Alloy,
    EnergyCristal,
    CondensedEnergyCristal,
}

[Serializable]
public struct ResourceNumber
{
    public ResourceType ResourceType;
    public int Ammount;
}

public class PlayerCollectiblesCount : MonoBehaviour
{
	static public int MetalCrumbsAmount = 0;   
	static public int MetalAmount = 10;   
	static public int RareMetalCrumbsAmount = 0;   
	static public int AlloyAmount = 10;   
	static public int EnergyCristalAmount = 0;   
	static public int CondensedEnergyCristalAmount = 0;

    public ResourceNumber[] MetalCost;
    public ResourceNumber[] AlloyCost;

    public int MetalCrumbsView = 0;
    public int MetalView = 0;
    public int RareMetalCrumbsView = 0;
    public int AlloyView = 0;
    public int CristalView = 0;
    public int RareCristalView = 0;

    private void Start()
    {
        StartCoroutine(TransmuteResourcesRotine());
    }

    private void Update()
    {
        MetalCrumbsView = MetalCrumbsAmount;
        MetalView = MetalAmount;
        RareMetalCrumbsView = RareMetalCrumbsAmount;
        AlloyView = AlloyAmount;
        CristalView = EnergyCristalAmount;
        RareCristalView = CondensedEnergyCristalAmount;
    }

    IEnumerator TransmuteResourcesRotine()
    {
        while (true)
        {
            yield return new WaitForSeconds (.3f);

            if (ExpendResources(MetalCost))
                MetalAmount++;
            if (ExpendResources(AlloyCost))
                AlloyAmount++;
        }
    }
   
    static public bool ExpendResources (ResourceNumber[] costs)
    {
        if (CheckResourcesAmmount(costs))
        {
            SubtractResources(costs);
            return true;
        }
        else { return false; }
    }

    static bool CheckResourcesAmmount(ResourceNumber[] costs)
    {
        bool hasEnough = true;

        foreach (ResourceNumber resourceNumber in costs)
        {
            if (resourceNumber.ResourceType == ResourceType.MetalCrumb & resourceNumber.Ammount > MetalCrumbsAmount)
                hasEnough = false;
            else if (resourceNumber.ResourceType == ResourceType.Metal & resourceNumber.Ammount > MetalAmount)
                hasEnough = false;
            else if (resourceNumber.ResourceType == ResourceType.RareMetalCrumb & resourceNumber.Ammount > RareMetalCrumbsAmount)
                hasEnough = false;
            else if (resourceNumber.ResourceType == ResourceType.Alloy & resourceNumber.Ammount > AlloyAmount)
                hasEnough = false;
            else if (resourceNumber.ResourceType == ResourceType.EnergyCristal & resourceNumber.Ammount > EnergyCristalAmount)
                hasEnough = false;
            else if (resourceNumber.ResourceType == ResourceType.CondensedEnergyCristal & resourceNumber.Ammount > CondensedEnergyCristalAmount)
                hasEnough = false;       
        }     
        
        return hasEnough;
    }

    static void SubtractResources(ResourceNumber[] costs)
    {
        foreach (ResourceNumber resourceNumber in costs)
        {
            if (resourceNumber.ResourceType == ResourceType.MetalCrumb)
                MetalCrumbsAmount -= resourceNumber.Ammount;
            else if (resourceNumber.ResourceType == ResourceType.Metal)
                MetalAmount -= resourceNumber.Ammount;
            else if (resourceNumber.ResourceType == ResourceType.RareMetalCrumb)
                RareMetalCrumbsAmount -= resourceNumber.Ammount;
            else if (resourceNumber.ResourceType == ResourceType.Alloy)
                AlloyAmount -= resourceNumber.Ammount;
            else if (resourceNumber.ResourceType == ResourceType.EnergyCristal)
                EnergyCristalAmount -= resourceNumber.Ammount;
            else if (resourceNumber.ResourceType == ResourceType.CondensedEnergyCristal)
                CondensedEnergyCristalAmount -= resourceNumber.Ammount;
        }
    }
}