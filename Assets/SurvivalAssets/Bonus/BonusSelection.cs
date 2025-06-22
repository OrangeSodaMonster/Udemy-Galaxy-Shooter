using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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
        NONE = 21,
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
    [FoldoutGroup("Active")]
    [ReadOnly] public List<BonusType> ActivePowerBonuses = new();
    [FoldoutGroup("Active")]
    [ReadOnly] public List<BonusType> ActiveUtilityBonuses = new();
    [FoldoutGroup("Active")]
    [ReadOnly] public List<BonusType> ActiveSuperBonus = new();
    [FoldoutGroup("Lists")]
    [ReadOnly] public List<BonusType> PowerBonuses = new();
    [FoldoutGroup("Lists")]
    [ReadOnly] public List<BonusType> UtilityBonuses = new();
    [FoldoutGroup("Lists")]
    [ReadOnly] public List<BonusType> SuperBonuses = new();

    Dictionary<BonusType, int> CurrentBonusLevels = new();

    public static BonusSelection Instance;
    private void Awake()
    {
        SetInstance();
    }
    public void SetInstance()
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
        for (int i = 0; i < numBonus - 5; i++)
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
        for (int i = 8; i < ListOfChances.Count - 5; i++)
        {
            UtilitySelectionChances.Add(ListOfChances[i]);
        }
    }

    [Button(),FoldoutGroup("Lists")]
    public void PopulateTypeLists()
    {
        int numBonus = Enum.GetNames(typeof(BonusType)).Length;
        PowerBonuses.Clear();
        UtilityBonuses.Clear();
        SuperBonuses.Clear();
        for (int i = 0; i < 8; i++)
        {
            PowerBonuses.Add((BonusType)i);
        }
        for (int i = 8; i < numBonus-5; i++)
        {
            UtilityBonuses.Add((BonusType)i);
        }
        for (int i = numBonus-5; i < numBonus-1; i++)
        {
            SuperBonuses.Add((BonusType)i);
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

            PopulatePowerBonuses();
            PopulateUtilityBonuses();
        }        

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
                    if (CurrentBonusLevels[BonusType.BombPower] < 3 && CurrentBonusLevels[BonusType.BombGeneration] < 3)
                        isPossible = false;
                    else if ((CurrentBonusLevels[BonusType.BombPower] + CurrentBonusLevels[BonusType.BombGeneration] + CurrentBonusLevels[BonusType.DroneIonBombRange]) < 5)
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
                    if (ionStream.PowerUpgrades.UpgradesPerc < 99 && ionStream.CadencyUpgrades.UpgradesPerc < 99 && ionStream.HitNumberUpgrades.UpgradesPerc < 99)
                        isPossible = false;
                    else if (ionStream.Upgrades.UpgradesPerc < 80)
                        isPossible = false;
                    ListOfSupers[i].IsPossible = isPossible;
                    break;
            }
        }
    }

    List<BonusChances> GetPossiblePowerBonuses()
    {
        List<BonusChances> list = new(PowerSelectionChances);

        return list
        .Where(b =>
            CurrentBonusLevels[b.Type] < 3 && // Só inclui se o nível for menor que 3
            (
                !removedBonuses.Contains(b.Type) &&
                (ActivePowerBonuses.Contains(b.Type) || // Já está ativo
                ActivePowerBonuses.Count() < 3 && // Ainda cabe mais um
                !(ActivePowerBonuses.Count() == 2 && ActiveUtilityBonuses.Count() >= 3)) // Regra extra para evitar ultrapassar o limite
            )
        )
        .ToList();
    }

    List<BonusChances> GetPossibleUtilityBonuses()
    {
        List<BonusChances> list = new(UtilitySelectionChances);        

        return list
        .Where(b =>
            CurrentBonusLevels[b.Type] < 3 && // Só inclui se o nível for menor que 3
            (
                !removedBonuses.Contains(b.Type) &&
                (ActiveUtilityBonuses.Contains(b.Type) || // Já está ativo
                ActiveUtilityBonuses.Count() < 3 && // Ainda cabe mais um
                !(ActiveUtilityBonuses.Count() == 2 && ActivePowerBonuses.Count() >= 3)) // Regra extra para evitar ultrapassar o limite
            )
        )
        .ToList();
    }

    void SetPossibleLists()
    {
        UpdateBonusChances();
        CurrentBonusLevels = BonusPowersDealer.Instance.GetBonusLevels();
        possiblePowerBonuses.Clear();
        possiblePowerBonuses = GetPossiblePowerBonuses();
        possibleUtilityBonuses.Clear();
        possibleUtilityBonuses = GetPossibleUtilityBonuses();
    }
    List<BonusChances> possiblePowerBonuses = new();
    List<BonusChances> possibleUtilityBonuses = new();
    public void GetBonusBoxes(out GameObject left, out GameObject middle, out GameObject right)
    {
        SetPossibleLists();

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

            if (possiblePowerBonuses.Count > 0)
            {
                BonusType picked = GetBonus(possiblePowerBonuses);
                RemoveFromListOfChances(picked, possiblePowerBonuses);
                return picked;
            }
            else if (possibleUtilityBonuses.Count > 0)
            {
                BonusType picked = GetBonus(possibleUtilityBonuses);
                RemoveFromListOfChances(picked, possibleUtilityBonuses);
                return picked;
            }
            else return BonusType.NONE;
        }
        BonusType GetRightSide()
        {
            CheckPossibleSupers();
            if (possibleSupers.Count >= 3)
                return possibleSupers[2];

            if (possibleUtilityBonuses.Count > 0)
            {
                BonusType picked = GetBonus(possibleUtilityBonuses);
                RemoveFromListOfChances(picked, possibleUtilityBonuses);
                return picked;
            }
            else if (possiblePowerBonuses.Count > 0)
            {
                BonusType picked = GetBonus(possiblePowerBonuses);
                RemoveFromListOfChances(picked, possiblePowerBonuses);
                return picked;
            }
            else return BonusType.NONE;
        }
        BonusType GetMiddle()
        {
            CheckPossibleSupers();
            if (possibleSupers.Count >= 1)
                return possibleSupers[0];

            List<BonusChances> list = new(possiblePowerBonuses);
            list.AddRange(possibleUtilityBonuses);

            if(list.Count > 0)
                return GetBonus(list);
            else
                return BonusType.NONE;
        }
    }

    void RemoveFromListOfChances(BonusType type, List<BonusChances> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Type == type)
                list.RemoveAt(i);
        }
    }
    public void AddToActiveBonuses(BonusSelection.BonusType type)
    {
        if(PowerBonuses.Contains(type) && !ActivePowerBonuses.Contains(type))
        {
            ActivePowerBonuses.Add(type);
            RemoveBonusPossibility();
        }
        else if(UtilityBonuses.Contains(type) && !ActiveUtilityBonuses.Contains(type))
        {
            ActiveUtilityBonuses.Add(type);
            RemoveBonusPossibility();
        }
        else if (SuperBonuses.Contains(type))
        {
            ActiveSuperBonus.Add(type);
            RemoveBonusPossibility();
        }
    }
    
    List<BonusType> removedBonuses = new();
    bool isFirstPick = true;
    void RemoveBonusPossibility()
    {
        if (isFirstPick)
        {
            isFirstPick = false;
            Debug.Log("First Bonus Pick");
            return;
        }

        Debug.Log("NOT First Bonus Pick");
        float minValue = float.MaxValue;
        int minIndex = -1;
        for(int i = 0;i < ListOfChances.Count; i++)
        {
            if (ListOfChances[i].TotalChance < minValue)
            {
                if(ActivePowerBonuses.Contains(ListOfChances[i].Type)) continue;
                if(ActiveUtilityBonuses.Contains(ListOfChances[i].Type)) continue;
                if(removedBonuses.Contains(ListOfChances[i].Type)) continue;

                minValue = ListOfChances[i].TotalChance;
                minIndex = i;
            }
        }
        Debug.Log("Should Remove " + ListOfChances[minIndex].Type);
        if (minIndex >= 0)
        {
            Debug.Log($"<color=purple>Removed {ListOfChances[minIndex].Type} from list</color>");
            removedBonuses.Add(ListOfChances[minIndex].Type);
        }
        else
        {
            Debug.Log($"<color=red>MININDEX = -1 ERROR</color>");
        }
    }

    public bool ChechIfThereAreAPossibleBonusPick()
    {
        CurrentBonusLevels = BonusPowersDealer.Instance.GetBonusLevels();

        if(ActivePowerBonuses.Count < 2 || ActivePowerBonuses.Count < 3 && ActiveUtilityBonuses.Count < 3) return true;
        for(int i = 0; i < ActivePowerBonuses.Count; i++)
        {
            if (CurrentBonusLevels[ActivePowerBonuses[i]] < 3) return true;
        }

        if (ActiveUtilityBonuses.Count < 2 || ActiveUtilityBonuses.Count < 3 && ActivePowerBonuses.Count < 3) return true;
        for (int i = 0; i < ActiveUtilityBonuses.Count; i++)
        {
            if (CurrentBonusLevels[ActiveUtilityBonuses[i]] < 3) return true;
        }

        if(ActiveSuperBonus.Count < 1)
        {
            CheckPossibleSupers();
            if(possibleSupers.Count > 0) return true;
        }

        return false;
    }
}
