using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ResourceType
{
    Metal = 1,
    RareMetal = 2,
    EnergyCristal = 3,
    CondensedEnergyCristal = 4,
}

[Serializable]
public struct ResourceNumber
{
    [HorizontalGroup("G",width: .6f), LabelWidth(90)]
    public ResourceType ResourceType;
    [HorizontalGroup("G"),GUIColor("cyan"), LabelWidth(45)]
    public int Amount;
}

[Serializable]
public struct CollectibleLine
{
    [HorizontalGroup("G", 0.2f), LabelText("Metal"), GUIColor("white"), LabelWidth(40), MinValue(0)]
    public float MetalCrumb;
    [HorizontalGroup("G", .26f), LabelText("RareMetal"), GUIColor("green"), LabelWidth(65), MinValue(0)]
    public float RareMetalCrumb;
    [HorizontalGroup("G"), LabelText("Crystal"), GUIColor("blue"), LabelWidth(50), MinValue(0)]
    public float EnergyCristal;
    [HorizontalGroup("G", .3f), LabelText("CondCrystal"), GUIColor("purple"), LabelWidth(77), MinValue(0)]
    public float CondensedEnergyCristal;
}

[Serializable]
public struct UpgradeCost
{
    [HorizontalGroup("G"), HideLabel, MinValue(1)]
    public ResourceType Resource1;
    [HorizontalGroup("G"), HideLabel, MinValue(0)]
    public int Cost1;
    [HorizontalGroup("G"), HideLabel, MinValue(1)]
    public ResourceType Resource2;
    [HorizontalGroup("G"), HideLabel, MinValue(0)]
    public int Cost2;
}

public class PlayerCollectiblesCount : MonoBehaviour
{
	//static public int MetalCrumbsAmount = 0;   
	static public int MetalAmount = 0;   
	//static public int RareMetalCrumbsAmount = 0;   
	static public int RareMetalAmount = 0;   
	static public int EnergyCristalAmount = 0;   
	static public int CondensedEnergyCristalAmount = 0;

    //public ResourceNumber[] MetalCost;
    //public ResourceNumber[] AlloyCost;

    //public int MetalCrumbsView = 0;
    public int MetalView = 0;
    //public int RareMetalCrumbsView = 0;
    public int RareMetalView = 0;
    public int CristalView = 0;
    public int RareCristalView = 0;

    public static UnityEvent OnChangedCollectibleAmount = new();   

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

    static public bool ExpendResources(ResourceNumber costs)
    {
        ResourceNumber[] temp = new ResourceNumber[1];
        temp[0] = costs;

        return ExpendResources(temp);
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
    static public bool CheckResourcesAmmount(ResourceNumber costs)
    {
        ResourceNumber[] temp = new ResourceNumber[1];
        temp[0] = costs;

        return CheckResourcesAmmount(temp);
    }

    static public void SubtractResources(ResourceNumber[] costs)
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
            OnChangedCollectibleAmount?.Invoke();
    }

    public static void AddResourceNumber(ResourceNumber[] adds)
    {
        for (int i = 0; i < adds.Length; i++)
        {
            switch (adds[i].ResourceType)
            {
                case ResourceType.Metal:
                    MetalAmount += adds[i].Amount;
                    break;
                case ResourceType.RareMetal:
                    RareMetalAmount += adds[i].Amount;
                    break;
                case ResourceType.EnergyCristal:
                    EnergyCristalAmount += adds[i].Amount;
                    break;
                case ResourceType.CondensedEnergyCristal:
                    CondensedEnergyCristalAmount += adds[i].Amount;
                    break;
                case 0:
                    break;
            }
        }
    }

    public static void AddResourceNumber(ResourceNumber add)
    {
        ResourceNumber[] temp = new ResourceNumber[1];
        temp[0] = add;

        AddResourceNumber(temp);
    }

    public static void LoadResources(int metal, int rareMetal, int EnergyCrystal, int CondEnergyCrystal)
    {
        MetalAmount = metal;
        RareMetalAmount = rareMetal;
        EnergyCristalAmount = EnergyCrystal;
        CondensedEnergyCristalAmount = CondEnergyCrystal;
    }

    public static void ChangedCollectbleAmount()
    {
        OnChangedCollectibleAmount?.Invoke();
    }

    static public ResourceNumber[] ConvertUpgradeCost(UpgradeCost cost)
    {
        ResourceNumber[] costArray = new ResourceNumber[2];

        costArray[0].ResourceType = cost.Resource1;
        costArray[0].Amount = cost.Cost1;

        costArray[1].ResourceType = cost.Resource2;
        costArray[1].Amount = cost.Cost2;

        return costArray;
    }
}