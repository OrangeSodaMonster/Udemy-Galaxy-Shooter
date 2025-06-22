using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NewObjectiveInfo", menuName = "MySOs/ObjectiveInfo")]
[InlineEditor]
public class ObjectiveInfoSO : ScriptableObject
{
    #region "values"

    [BoxGroup("Objective Type")]
    [SerializeField, HorizontalGroup("Objective Type/1")] ObjectiveSize objectiveSize;
    public ObjectiveSize ObjectiveSize => objectiveSize;
    [SerializeField, HorizontalGroup("Objective Type/1")] ObjectiveMaterial objectiveMaterial;
    public ObjectiveMaterial ObjectiveMaterial => objectiveMaterial;
    [SerializeField, HorizontalGroup("Objective Type/1")] ObjectiveCrystal objectiveCrystal;
    public ObjectiveCrystal ObjectiveCrystal => objectiveCrystal;

    [SerializeField] ObjectiveInfoMasterSO masterSO;

    [BoxGroup("Basics"), HorizontalGroup("Basics/G", .2f), PreviewField(85, Alignment = ObjectFieldAlignment.Left), HideLabel()]
    [SerializeField] GameObject enemy;

    [ReadOnly, BoxGroup("Basics"), VerticalGroup("Basics/G/1"), LabelWidth(100), GUIColor("#ff5959")]
    public int MaxHP;
    [ReadOnly, BoxGroup("Basics")]
    public int CollisionDamage;
    [ReadOnly, BoxGroup("Basics")]
    [VerticalGroup("Basics/G/2"), LabelWidth(100), GUIColor("#efff85")]
    public float ImpactVelocity;

    // Drops

    DropsToSpawn[] DropsChances;
    DropsGuaranteed[] DropsGuaranteed;

    [ReadOnly] public Drops Drops;

    ///////////////

    [HorizontalGroup("Top", .65f), PropertyOrder(-1), LabelWidth(40)]
    public string Name;

    [HorizontalGroup("Top"), PropertyOrder(-1), HideLabel(), ReadOnly()]
    [SerializeField] UnityEngine.Color saveStateColor = UnityEngine.Color.green;

#endregion

    private void OnValidate()
    {
        saveStateColor = UnityEngine.Color.red;
        masterSO?.OnUpdateAndSaveAll?.AddListener(UpdateValues);

        Name = GetName();
    }

    string GetName()
    {
        string material = "";
        switch (objectiveMaterial)
        {            
            case ObjectiveMaterial.Rare:
                material = "Rare";
                break;
            case ObjectiveMaterial.VeryRare:
                material = "VeryRare";
                break;
        }

        string crystal = "";
        switch (objectiveCrystal)
        {
            case ObjectiveCrystal.None:
                crystal = "";
                break;
            case ObjectiveCrystal.Blue:
                crystal = "Blue";
                break;
            case ObjectiveCrystal.Pink:
                crystal = "Pink";
                break;
        }

        string size = "";
        switch (objectiveSize)
        {
            case ObjectiveSize.Smallest:
                size = "Smallest";
                break;
            case ObjectiveSize.Small:
                size = "Small";
                break;
            case ObjectiveSize.Medium:
                size = "Medium";
                break;
            case ObjectiveSize.Big:
                size = "Big";
                break;
        }

        return material+crystal+size;
    }

    void ConvertDropChance()
    {
        DropsChances = new DropsToSpawn[4];
        DropsChances[0].drop = ResourceType.Metal;
        DropsChances[0].spawnWeight = Drops.DropWeightsLine.MetalCrumb;
        DropsChances[1].drop = ResourceType.RareMetal;
        DropsChances[1].spawnWeight = Drops.DropWeightsLine.RareMetalCrumb;
        DropsChances[2].drop = ResourceType.EnergyCristal;
        DropsChances[2].spawnWeight = Drops.DropWeightsLine.EnergyCristal;
        DropsChances[3].drop = ResourceType.CondensedEnergyCristal;
        DropsChances[3].spawnWeight = Drops.DropWeightsLine.CondensedEnergyCristal;
    }

    void ConvertDropGuaranteed()
    {
        DropsGuaranteed = new DropsGuaranteed[4];
        DropsGuaranteed[0].drop = ResourceType.Metal;
        DropsGuaranteed[0].Amount = (int)Drops.GuaranteedLine.MetalCrumb;
        DropsGuaranteed[1].drop = ResourceType.RareMetal;
        DropsGuaranteed[1].Amount = (int)Drops.GuaranteedLine.RareMetalCrumb;
        DropsGuaranteed[2].drop = ResourceType.EnergyCristal;
        DropsGuaranteed[2].Amount = (int)Drops.GuaranteedLine.EnergyCristal;
        DropsGuaranteed[3].drop = ResourceType.CondensedEnergyCristal;
        DropsGuaranteed[3].Amount = (int)Drops.GuaranteedLine.CondensedEnergyCristal;
    }

    [Button("SavePrefab", ButtonSizes.Medium, ButtonAlignment = 1, Stretch = false), PropertyOrder(-1), GUIColor("Cyan"), HorizontalGroup("Top")]
    public void UpdateValues()
    {
        Drops = masterSO.GetDrops(objectiveSize, objectiveMaterial, objectiveCrystal);
        ConvertDropChance();
        ConvertDropGuaranteed();

        MaxHP = masterSO.CalculateHP(objectiveSize, objectiveMaterial, objectiveCrystal);
        CollisionDamage = masterSO.ColDamage;
        ImpactVelocity = masterSO.ImpactVel;

        if (enemy.TryGetComponent(out EnemyHP enemyHP))
        {
            enemyHP.MaxHP = MaxHP;
        }

        if (enemy.TryGetComponent(out CollisionWithPlayer collision))
        {
            collision.Damage = CollisionDamage;
            collision.ImpactVelocity = ImpactVelocity;
        }

        if (enemy.TryGetComponent(out EnemyDropDealer dropDealer))
        {
            dropDealer.DropsToSpawn = DropsChances;
            dropDealer.MinDropsNum = Drops.MinDrops;
            dropDealer.MaxDropsNum = Drops.MaxDrops;

            dropDealer.dropsGuaranteed = DropsGuaranteed;
        }

        if (enemy.TryGetComponent(out ObjectiveSizeRef sizeRef))
        {
            sizeRef.Size = objectiveSize;
        }

        saveStateColor = UnityEngine.Color.green;

#if UNITY_EDITOR
        PrefabUtility.SavePrefabAsset(enemy, out bool success);
        Debug.Log(success ? $"Saved {name}" : "Could not save asset");
#endif
    }
}
