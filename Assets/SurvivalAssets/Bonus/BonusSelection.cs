using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BonusSelection : SerializedMonoBehaviour
{
    [Serializable]
    public enum BonusType
    {
        LaserPower = 0,
        DronePower = 1,
        IonStreamPower = 2,
        BombPower = 3,
        LaserIonCadency = 4,
        BombGeneration = 5,
        EnemyExtraDamage = 6,
        ObjectiveExtraDamage = 7,
        Tractor = 8,
        DroneIonBombRange = 9,
        Mobility = 10,
        PowerUpDrop = 11,
        CristalDrop = 12,
        AutoConvertion = 13,
        HpRecovery = 14,
        ShieldStrenght = 15,
        ShieldRecovery = 16,
        SuperFourthDrone = 17,
        SuperLaserCadency = 18,
        SuperBomb = 19,
        SuperSecondIonStream = 20,
    }

    [Serializable]
    public class BonusChances
    {
        [HorizontalGroup("Top")]
        [ReadOnly] public BonusType Type;
        [HorizontalGroup("Top")]
        [LabelText("Default")] public float BaseChance = 10;
        [HorizontalGroup("Middle")]
        [ReadOnly, LabelText("Picked")] public float PickedBonusChance = 0;
        [HorizontalGroup("Middle")]
        [ReadOnly, LabelText("Upgrades")] public float UpgradesExtraChance = 0;
        [HorizontalGroup("Middle")]
        [ReadOnly, LabelText("Rounds")] public float RoundExtraChance = 0;
        [HorizontalGroup("Bottom")]
        [ReadOnly, LabelText("Total")] public float TotalChance = 0;
        [HorizontalGroup("Bottom")]
        [ReadOnly, LabelText("Effective")] public float EffectiveChance = 0;
    }
    [Serializable]
    public class SuperBonusChances
    {
        public BonusType Type;
        public bool IsPossible = false;
    }

    public Dictionary<BonusType, BonusSelectionBoxScript> BonusSelectionBoxes = new Dictionary<BonusType, BonusSelectionBoxScript> ();
    public List<BonusChances> ListOfChances = new();
    public List<SuperBonusChances> ListOfSupers = new();
    [ReadOnly] public List<BonusChances> PowerSelectionChances = new();
    [ReadOnly] public List<BonusChances> UtilitySelectionChances = new();

    Dictionary<BonusSelection.BonusType, int> CurrentBonusLevels = new();

    public static BonusSelection Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    [Button, GUIColor("green")]
    public void PopulateBonusChances(float baseChance)
    {
        int numBonus = Enum.GetNames(typeof(BonusType)).Length;
        for (int i = 0; i < numBonus - 4; i++)
        {
            BonusChances bonus = new();
            bonus.Type = (BonusType)i;
            bonus.BaseChance = baseChance;
            ListOfChances.Add(bonus);
        }
    }

    //[Button]
    //public void PopulateBonusSelectionBoxes()
    //{
    //    int numBonus = Enum.GetNames(typeof(BonusType)).Length;
    //    for (int i = 0; i < numBonus; i++)
    //    {
    //        BonusSelectionBoxes.Add((BonusType)i, null);
    //    }
    //}

    [Button, HorizontalGroup("1"), GUIColor("purple")]
    public void PopulatePowerBonuses()
    {
        PowerSelectionChances.Clear();
        for (int i = 0; i < 8; i++)
        {
            PowerSelectionChances.Add(ListOfChances[i]);
        }
    }
    [Button, HorizontalGroup("1"), GUIColor("blue")]
    public void PopulateUtilityBonuses()
    {
        UtilitySelectionChances.Clear();
        for (int i = 8; i < 17; i++)
        {
            UtilitySelectionChances.Add(ListOfChances[i]);
        }
    }
    //[Button, HorizontalGroup("1"), GUIColor("yellow")]   
    public BonusType GetBonus(List<BonusChances> bonusesList)
    {
        float totalWeight = 0;
        for (int i = 0;i < bonusesList.Count;i++)
        {
            totalWeight += bonusesList[i].EffectiveChance;
        }

        float randomValue = UnityEngine.Random.Range(0, totalWeight);

        for (int i = 0; i < bonusesList.Count; i++)
        {
            if (randomValue < bonusesList[i].EffectiveChance)
                return bonusesList[i].Type;
            else
                randomValue -= bonusesList[i].EffectiveChance;
        }

        Debug.Log("<color=red> Returned default item from list of bonus type </color>");
        return bonusesList[0].Type;
    }

    [Button]
    public void TestGetBonus()
    {
        BonusType bt = GetBonus(ListOfChances);
        Debug.Log(bt);
    }

    void UpdateBonusChances()
    {
        CurrentBonusLevels = BonusPowersDealer.Instance.GetBonusLevels();

        for (int i = 0; i < ListOfChances.Count; i++)
        {
            int totalUpgrades = 0;
            switch (ListOfChances[i].Type)
            {
                case BonusType.LaserPower:
                    ListOfChances[i].UpgradesExtraChance = PlayerStats.Instance.Lasers.PowerUpgrades.Upgrades * 1.5f;
                    ListOfChances[i].PickedBonusChance = UpdatePickedChance(ListOfChances[i].Type);
                    SumTotalChance(ListOfChances[i], false);
                    break;
                case BonusType.DronePower:
                    ListOfChances[i].UpgradesExtraChance = PlayerStats.Instance.Drones.PowerUpgrades.Upgrades * 2f;
                    ListOfChances[i].PickedBonusChance = UpdatePickedChance(ListOfChances[i].Type);
                    SumTotalChance(ListOfChances[i], false);
                    break;
                case BonusType.IonStreamPower:
                    ListOfChances[i].UpgradesExtraChance = PlayerStats.Instance.IonStream.PowerUpgrades.Upgrades * 4f;
                    ListOfChances[i].PickedBonusChance = UpdatePickedChance(ListOfChances[i].Type);
                    SumTotalChance(ListOfChances[i], false);
                    break;
                case BonusType.BombPower:
                    ListOfChances[i].UpgradesExtraChance = CurrentBonusLevels[BonusType.BombGeneration] * 5;
                    ListOfChances[i].PickedBonusChance = UpdatePickedChance(ListOfChances[i].Type);
                    SumTotalChance(ListOfChances[i], true, true);
                    break;
                case BonusType.LaserIonCadency:
                    totalUpgrades = PlayerStats.Instance.Lasers.CadencyUpgrades.Upgrades + PlayerStats.Instance.IonStream.CadencyUpgrades.Upgrades;
                    ListOfChances[i].UpgradesExtraChance = totalUpgrades * 1.5f;
                    ListOfChances[i].PickedBonusChance = UpdatePickedChance(ListOfChances[i].Type);
                    SumTotalChance(ListOfChances[i], false);
                    break;
                case BonusType.BombGeneration:
                    ListOfChances[i].UpgradesExtraChance = CurrentBonusLevels[BonusType.BombPower] * 5;
                    ListOfChances[i].PickedBonusChance = UpdatePickedChance(ListOfChances[i].Type);
                    SumTotalChance(ListOfChances[i], true, true);
                    break;
                case BonusType.EnemyExtraDamage:
                    ListOfChances[i].PickedBonusChance = UpdatePickedChance(ListOfChances[i].Type);
                    SumTotalChance(ListOfChances[i], true);
                    break;
                case BonusType.ObjectiveExtraDamage:
                    ListOfChances[i].PickedBonusChance = UpdatePickedChance(ListOfChances[i].Type);
                    SumTotalChance(ListOfChances[i], true);
                    break;
                case BonusType.Tractor:
                    ListOfChances[i].UpgradesExtraChance = PlayerStats.Instance.Ship.TractorUpgrades.Upgrades * 3.5f;
                    ListOfChances[i].PickedBonusChance = UpdatePickedChance(ListOfChances[i].Type);
                    SumTotalChance(ListOfChances[i], false);
                    break;
                case BonusType.DroneIonBombRange:
                    totalUpgrades = PlayerStats.Instance.Drones.RangeUpgrades.Upgrades + PlayerStats.Instance.IonStream.RangeUpgrades.Upgrades;
                    ListOfChances[i].UpgradesExtraChance = totalUpgrades * 2.5f;
                    ListOfChances[i].PickedBonusChance = UpdatePickedChance(ListOfChances[i].Type);
                    SumTotalChance(ListOfChances[i], false);
                    break;
                case BonusType.Mobility:
                    totalUpgrades = PlayerStats.Instance.Ship.SpeedUpgrades.Upgrades + PlayerStats.Instance.Ship.ManobrabilityUpgrades.Upgrades;
                    ListOfChances[i].UpgradesExtraChance = totalUpgrades * 2.5f;
                    ListOfChances[i].PickedBonusChance = UpdatePickedChance(ListOfChances[i].Type);
                    SumTotalChance(ListOfChances[i], false);
                    break;
                case BonusType.PowerUpDrop:
                    ListOfChances[i].PickedBonusChance = UpdatePickedChance(ListOfChances[i].Type);
                    SumTotalChance(ListOfChances[i], true);
                    break;
                case BonusType.CristalDrop:
                    ListOfChances[i].PickedBonusChance = UpdatePickedChance(ListOfChances[i].Type);
                    SumTotalChance(ListOfChances[i], true);
                    break;
                case BonusType.AutoConvertion:
                    ListOfChances[i].PickedBonusChance = UpdatePickedChance(ListOfChances[i].Type);
                    SumTotalChance(ListOfChances[i], true);
                    break;
                case BonusType.HpRecovery:
                    ListOfChances[i].UpgradesExtraChance = PlayerStats.Instance.Drones.HealUpgrades.Upgrades * 3f;
                    ListOfChances[i].PickedBonusChance = UpdatePickedChance(ListOfChances[i].Type);
                    SumTotalChance(ListOfChances[i], false);
                    break;
                case BonusType.ShieldStrenght:
                    ListOfChances[i].UpgradesExtraChance = PlayerStats.Instance.Shields.StrenghtUpgrades.Upgrades * 1.5f;
                    ListOfChances[i].PickedBonusChance = UpdatePickedChance(ListOfChances[i].Type);
                    SumTotalChance(ListOfChances[i], false);
                    break;
                case BonusType.ShieldRecovery:
                    ListOfChances[i].UpgradesExtraChance = PlayerStats.Instance.Shields.RecoveryUpgrades.Upgrades * 1.8f;
                    ListOfChances[i].PickedBonusChance = UpdatePickedChance(ListOfChances[i].Type);
                    SumTotalChance(ListOfChances[i], false);
                    break;
            }
        }

        PopulatePowerBonuses();
        PopulateUtilityBonuses();

        void SumTotalChance(BonusChances chances, bool addRound, bool halfRound = false)
        {
            float upgrades = Mathf.Clamp(chances.UpgradesExtraChance, 0f, 20f);
            float round = Mathf.Clamp(chances.RoundExtraChance, 0f, 20f);

            chances.TotalChance = chances.BaseChance + chances.UpgradesExtraChance + chances.PickedBonusChance;
            chances.EffectiveChance = chances.BaseChance + upgrades + chances.PickedBonusChance;
            if (addRound)
            {
                if (halfRound)
                {
                    chances.TotalChance += chances.RoundExtraChance*0.5f;
                    chances.EffectiveChance += round*0.5f;
                }
                else
                {
                    chances.TotalChance += chances.RoundExtraChance;
                    chances.EffectiveChance += round;
                }
            }
        }

        float UpdatePickedChance(BonusType type)
        {
            return CurrentBonusLevels[type] * 10;
        }
    }

    void UpdateBonusRoundChances()
    {
        for (int i = 0; i < ListOfChances.Count; i++)
        {
            float random = UnityEngine.Random.Range(1.5f, 2.5f);
            ListOfChances[i].RoundExtraChance += random;
        }
    }   
    
    List<BonusType> possibleSupers= new();
    void CheckPossibleSupers()
    {
        possibleSupers.Clear();

        UpdateSupers();
        for (int i = 0; i < ListOfSupers.Count; i++)
        {
            if(ListOfSupers[i].IsPossible)
                possibleSupers.Add(ListOfSupers[i].Type);
        }
        if(possibleSupers.Count >= 4)
        {
            int i = UnityEngine.Random.Range(0, 4);
            possibleSupers.RemoveAt(i);
        }
    }

    
    void UpdateSupers()
    {
        CurrentBonusLevels = BonusPowersDealer.Instance.GetBonusLevels();

        if(BonusPowersDealer.Instance.IsFourthDrone || BonusPowersDealer.Instance.IsMoreLaserCadency ||
            BonusPowersDealer.Instance.IsSuperBomb ||BonusPowersDealer.Instance.IsSecondIonStream)
        {
            for (int i = 0; i < ListOfSupers.Count; i++)
            {
                ListOfSupers[i].IsPossible = false;
            }
            return;
        }

        for (int i = 0; i < ListOfSupers.Count; i++)
        {
            bool isPossible = true;
            switch (ListOfSupers[i].Type)
            {
                case BonusType.SuperBomb:
                    if (CurrentBonusLevels[BonusType.BombPower] < 3 || CurrentBonusLevels[BonusType.BombGeneration] < 3)
                        isPossible = false;
                    else if ((CurrentBonusLevels[BonusType.BombPower] + CurrentBonusLevels[BonusType.BombGeneration] + CurrentBonusLevels[BonusType.DroneIonBombRange] < 5))
                        isPossible = false;
                    ListOfSupers[i].IsPossible = isPossible;
                    break;
                case BonusType.SuperFourthDrone:
                    if (!PlayerStats.Instance.Drones.Drone1.Unlocked || !PlayerStats.Instance.Drones.Drone2.Unlocked || !PlayerStats.Instance.Drones.Drone3.Unlocked)
                        isPossible = false;
                    else if (PlayerStats.Instance.Drones.Upgrades.UpgradesPerc < 80)
                        isPossible = false;
                    ListOfSupers[i].IsPossible = isPossible;
                    break;
                case BonusType.SuperLaserCadency:
                    PlayerStats.LasersStats lasers = PlayerStats.Instance.Lasers;
                    if (!lasers.FrontLaser.Unlocked || !lasers.SpreadLaser.Unlocked || !lasers.SideLaser.Unlocked || !lasers.BackLaser.Unlocked)
                        isPossible = false;
                    else if (lasers.Upgrades.UpgradesPerc < 80)
                        isPossible = false;
                    ListOfSupers[i].IsPossible = isPossible;
                    break;
                case BonusType.SuperSecondIonStream:
                    PlayerStats.IonStreamStats ionStream = PlayerStats.Instance.IonStream;
                    if (!(ionStream.PowerUpgrades.UpgradesPerc < 99) && !(ionStream.CadencyUpgrades.UpgradesPerc < 99) && !(ionStream.HitNumberUpgrades.UpgradesPerc < 99))
                        isPossible = false;
                    else if (ionStream.Upgrades.UpgradesPerc < 80)
                        isPossible = false;
                    ListOfSupers[i].IsPossible = isPossible;
                    break;
            }
        }
    }

    public void GetBonusBoxes(out GameObject left, out GameObject middle, out GameObject right)
    {
        UpdateBonusChances();

        BonusType leftType = GetLeftSide();
        BonusType rightType = GetRightSide();
        BonusType middleType = GetMiddle();

        left = BonusSelectionBoxes[leftType].gameObject;
        middle = BonusSelectionBoxes[middleType].gameObject;
        right = BonusSelectionBoxes[rightType].gameObject;

        UpdateBonusRoundChances();
        UpdateBonusChances();

        BonusType GetLeftSide()
        {
            CheckPossibleSupers();
            if (possibleSupers.Count >= 2)
                return possibleSupers[1];

            return GetBonus(PowerSelectionChances);
        }
        BonusType GetRightSide()
        {
            CheckPossibleSupers();
            if (possibleSupers.Count >= 3)
                return possibleSupers[2];

            return GetBonus(UtilitySelectionChances);
        }
        BonusType GetMiddle()
        {
            CheckPossibleSupers();
            if (possibleSupers.Count >= 1)
                return possibleSupers[0];

            List<BonusChances> list = PowerSelectionChances;
            list.AddRange(UtilitySelectionChances);

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Type == leftType || list[i].Type == rightType)
                    list.RemoveAt(i);
            }

            return GetBonus(list);
        }
    }   
}
