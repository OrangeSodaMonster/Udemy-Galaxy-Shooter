using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    //MetalCrumb,
    Metal = 1,
    //RareMetalCrumb,
    RareMetal = 2,
    EnergyCristal = 3,
    CondensedEnergyCristal = 4,
}

[Serializable]
public struct ResourceNumber
{
    public ResourceType ResourceType;
    public int Amount;
}

public class PlayerCollectiblesCount : MonoBehaviour
{
	//static public int MetalCrumbsAmount = 0;   
	static public int MetalAmount = 0;   
	//static public int RareMetalCrumbsAmount = 0;   
	static public int RareMetalAmount = 0;   
	static public int EnergyCristalAmount = 0;   
	static public int CondensedEnergyCristalAmount = 0;

    public ResourceNumber[] MetalCost;
    public ResourceNumber[] AlloyCost;

    //public int MetalCrumbsView = 0;
    public int MetalView = 0;
    //public int RareMetalCrumbsView = 0;
    public int RareMetalView = 0;
    public int CristalView = 0;
    public int RareCristalView = 0;

    private void Awake()
    {
        // REMOVER
        //MetalCrumbsAmount = MetalCrumbsView;
        //MetalAmount = MetalView;
        //RareMetalCrumbsAmount = RareMetalCrumbsView;
        //RareMetalAmount = RareMetalView;
        //EnergyCristalAmount = CristalView;
        //CondensedEnergyCristalAmount = RareCristalView;
    }

    private void Start()
    {
        //StartCoroutine(TransmuteResourcesRotine());        
    }

    private void Update()
    {
        //MetalCrumbsView = MetalCrumbsAmount;
        MetalView = MetalAmount;
        //RareMetalCrumbsView = RareMetalCrumbsAmount;
        RareMetalView = RareMetalAmount;
        CristalView = EnergyCristalAmount;
        RareCristalView = CondensedEnergyCristalAmount;
    }

    //IEnumerator TransmuteResourcesRotine()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds (.3f);

    //        if (ExpendResources(MetalCost))
    //            MetalAmount++;
    //        if (ExpendResources(AlloyCost))
    //            RareMetalAmount++;
    //    }
    //}
   
    static public bool ExpendResources (ResourceNumber[] costs)
    {
        if (CheckResourcesAmmount(costs))
        {
            SubtractResources(costs);
            return true;
        }
        else { return false; }
    }

    static public bool CheckResourcesAmmount(ResourceNumber[] costs)
    {
        bool hasEnough = true;

        foreach (ResourceNumber resourceNumber in costs)
        {
            //if (resourceNumber.ResourceType == ResourceType.MetalCrumb & resourceNumber.Amount > MetalCrumbsAmount)
            //    hasEnough = false;
            if (resourceNumber.ResourceType == ResourceType.Metal & resourceNumber.Amount > MetalAmount)
                hasEnough = false;
            //else if (resourceNumber.ResourceType == ResourceType.RareMetalCrumb & resourceNumber.Amount > RareMetalCrumbsAmount)
            //    hasEnough = false;
            else if (resourceNumber.ResourceType == ResourceType.RareMetal & resourceNumber.Amount > RareMetalAmount)
                hasEnough = false;
            else if (resourceNumber.ResourceType == ResourceType.EnergyCristal & resourceNumber.Amount > EnergyCristalAmount)
                hasEnough = false;
            else if (resourceNumber.ResourceType == ResourceType.CondensedEnergyCristal & resourceNumber.Amount > CondensedEnergyCristalAmount)
                hasEnough = false;       
        }     
        
        return hasEnough;
    }

    static void SubtractResources(ResourceNumber[] costs)
    {
        foreach (ResourceNumber resourceNumber in costs)
        {
            //if (resourceNumber.ResourceType == ResourceType.MetalCrumb)
            //    MetalCrumbsAmount -= resourceNumber.Amount;
            if (resourceNumber.ResourceType == ResourceType.Metal)
                MetalAmount -= resourceNumber.Amount;
            //else if (resourceNumber.ResourceType == ResourceType.RareMetalCrumb)
            //    RareMetalCrumbsAmount -= resourceNumber.Amount;
            else if (resourceNumber.ResourceType == ResourceType.RareMetal)
                RareMetalAmount -= resourceNumber.Amount;
            else if (resourceNumber.ResourceType == ResourceType.EnergyCristal)
                EnergyCristalAmount -= resourceNumber.Amount;
            else if (resourceNumber.ResourceType == ResourceType.CondensedEnergyCristal)
                CondensedEnergyCristalAmount -= resourceNumber.Amount;
        }
    }

    public static void LoadResources(int metal, int rareMetal, int EnergyCrystal, int CondEnergyCrystal)
    {
        MetalAmount = metal;
        RareMetalAmount = rareMetal;
        EnergyCristalAmount = EnergyCrystal;
        CondensedEnergyCristalAmount = CondEnergyCrystal;
    }
}