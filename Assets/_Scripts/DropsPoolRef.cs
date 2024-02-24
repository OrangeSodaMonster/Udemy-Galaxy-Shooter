using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropsPoolRef : MonoBehaviour
{
    public static DropsPoolRef Instance;

    [field:SerializeField] public MMSimpleObjectPooler MetalCrumbsPooler { get; private set; }
    [field:SerializeField] public MMSimpleObjectPooler RareMetalCrumbsPooler { get; private set; }
    [field:SerializeField] public MMSimpleObjectPooler EnergyCristalPooler { get; private set; }
    [field:SerializeField] public MMSimpleObjectPooler CondensedEnergyCristalPooler { get; private set; }

    public Dictionary<ResourceType, MMSimpleObjectPooler> ResourcePoolers { get; private set; } = new();

    private void Awake()
    {
        ResourcePoolers.Add(ResourceType.Metal, MetalCrumbsPooler);
        ResourcePoolers.Add(ResourceType.RareMetal, RareMetalCrumbsPooler);
        ResourcePoolers.Add(ResourceType.EnergyCristal, EnergyCristalPooler);
        ResourcePoolers.Add(ResourceType.CondensedEnergyCristal, CondensedEnergyCristalPooler);

        if (Instance == null)
            Instance = this;
    }
}