using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static BonusSelection;

public class BonusSelectionBoxScript : MonoBehaviour
{
    [SerializeField] BonusSelection.BonusType bonusType;
    public BonusSelection.BonusType BonusType {  get { return bonusType; } }

    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI valueText;
    [SerializeField] TextMeshProUGUI nameText;

    [SerializeField] Color color;

    string valueLanguage = "";
    string levelLanguage = "";
    int nextLevel = 0;
    string nextValue = "";

    bool isSuper = false;
    BonusPowersDealer bonusDealer;
    Dictionary<BonusType, int> currentBonusLevels = new();

    private void OnEnable()
    {
        bonusDealer = BonusPowersDealer.Instance;

        if (GameManager.CurrentLanguage == Language.English)
        {
            valueLanguage = "Value: ";
            levelLanguage = "Lvl: ";
        }
        else 
        {
            valueLanguage = "Valor: ";
            levelLanguage = "Nvl: ";
        }

        SelectUpdateFunc();
        SetColor();

        if (isSuper)
            levelText.enabled = false;
        else
        {
            levelText.enabled = true;
            levelText.text = $"{levelLanguage}{nextLevel}";
        }
            valueText.text = $"{valueLanguage}{nextValue}";  
        
        transform.parent.GetComponent<Image>().color = color;
    }

    private void OnValidate()
    {
        SetColor();
    }

    void SetColor()
    {
        levelText.color = color;
        valueText.color = color;
        nameText.color = color;
    }

    void SelectUpdateFunc()
    {
        currentBonusLevels = bonusDealer.GetBonusLevels();
        switch (bonusType)
        {
            case BonusSelection.BonusType.LaserPower:
                LaserPowerUpdate();
                break;
            case BonusSelection.BonusType.DronePower:
                DronePowerUpdate();
                break;
            case BonusSelection.BonusType.IonStreamPower:
                IonStreamPowerUpdate();
                break;
            case BonusSelection.BonusType.BombPower:
                BombPowerUpdate();
                break;
            case BonusSelection.BonusType.LaserIonCadency:
                CadencyUpdate();
                break;
            case BonusSelection.BonusType.BombGeneration:
                BombGenerationUpdate();
                break;
            case BonusSelection.BonusType.EnemyExtraDamage:
                EnemyExtraDamageUpdate();
                break;
            case BonusSelection.BonusType.ObjectiveExtraDamage:
                ObjectiveExtraDamageUpdate();
                break;
            case BonusSelection.BonusType.Tractor:
                TractorUpdate();
                break;
            case BonusSelection.BonusType.DroneIonBombRange:
                RangeUpdate();
                break;
            case BonusSelection.BonusType.Mobility :
                MobilityUpdate();
                break;
            case BonusSelection.BonusType.PowerUpDrop :
                PowerUpDropUpdate();
                break;
            case BonusSelection.BonusType.CristalDrop :
                CristalDropUpdate();
                break;
            case BonusSelection.BonusType.AutoConvertion :
                AutoConvertionUpdate();
                break;
            case BonusSelection.BonusType.HpRecovery :
                HpRecoveryUpdate();
                break;
            case BonusSelection.BonusType.ShieldStrenght :
                ShieldStrenghtUpdate();
                break;
            case BonusSelection.BonusType.ShieldRecovery :
                ShieldRecoveryUpdate();
                break;
            case BonusSelection.BonusType.SuperFourthDrone :
                Super4thDroneUpdate();
                break;
            case BonusSelection.BonusType.SuperLaserCadency :
                SuperLaserCadencyUpdate();
                break;
            case BonusSelection.BonusType.SuperBomb :
                SuperBombUpdate();
                break;
            case BonusSelection.BonusType.SuperSecondIonStream :
                SuperSecondIonStreamUpdate();
                break;
        }
    }

    void LaserPowerUpdate()
    {
        UpdateBonus(BonusSelection.BonusType.LaserPower, bonusDealer.LaserPowerExtraDamage);
    }
    void DronePowerUpdate()
    {
        UpdateBonus(BonusSelection.BonusType.DronePower, bonusDealer.DronePowerExtraDamage);
    }
    void IonStreamPowerUpdate()
    {
        UpdateBonus(BonusSelection.BonusType.IonStreamPower, bonusDealer.IonStreamPowerExtraDamage);
    }
    void BombPowerUpdate()
    {
        UpdateBonus(BonusSelection.BonusType.BombPower, bonusDealer.BombPowerExtraDamage);
    }
    void CadencyUpdate()
    {
        UpdateBonus(BonusSelection.BonusType.LaserIonCadency, bonusDealer.LaserIonStreamCadencyPerc);
    }
    void BombGenerationUpdate()
    {
        UpdateBonus(BonusSelection.BonusType.BombGeneration, bonusDealer.BombRegenerationTimer);
    }
    void EnemyExtraDamageUpdate()
    {
        UpdateBonus(BonusSelection.BonusType.EnemyExtraDamage, bonusDealer.ExtraDamageToEnemiesPerc);
    }
    void ObjectiveExtraDamageUpdate()
    {
        UpdateBonus(BonusSelection.BonusType.ObjectiveExtraDamage, bonusDealer.ExtraDamageToObjectivesPerc);
    }
    void TractorUpdate()
    {
        UpdateBonus(BonusSelection.BonusType.Tractor, bonusDealer.TractorExtraPerc);
    }
    void RangeUpdate()
    {
        UpdateBonus(BonusSelection.BonusType.DroneIonBombRange, bonusDealer.DroneIonStreamRangeExtraPerc);
    }
    void MobilityUpdate()
    {
        UpdateBonus(BonusSelection.BonusType.Mobility, bonusDealer.MobilityExtraPerc);
    }
    void PowerUpDropUpdate()
    {
        UpdateBonus(BonusSelection.BonusType.PowerUpDrop, bonusDealer.PowerUpDropExtraPerc);
    }
    void CristalDropUpdate()
    {
        UpdateBonus(BonusSelection.BonusType.CristalDrop, bonusDealer.EnergyCristalsExtraPerc);
    }
    void AutoConvertionUpdate()
    {
        UpdateBonus(BonusSelection.BonusType.AutoConvertion, bonusDealer.AutoConvertionTimer);
    }
    void HpRecoveryUpdate()
    {
        UpdateBonus(BonusSelection.BonusType.HpRecovery, bonusDealer.HpRecoveryTimerExtraPerc);
    }
    void ShieldStrenghtUpdate()
    {
        UpdateBonus(BonusSelection.BonusType.ShieldStrenght, bonusDealer.ShieldExtraStr);
    }
    void ShieldRecoveryUpdate()
    {
        UpdateBonus(BonusSelection.BonusType.ShieldRecovery, bonusDealer.ShieldRecoveryExtraPerc);
    }
    void Super4thDroneUpdate()
    {
        isSuper = true ;
        nextLevel = 0;
        nextValue = "";
        valueLanguage = "";
    }
    void SuperBombUpdate()
    {
        isSuper = true;
        nextLevel = 0;
        nextValue = $"{bonusDealer.SuperBombExtraDamage}/{bonusDealer.SuperBombExtraRange}%";
    }
    void SuperSecondIonStreamUpdate()
    {
        isSuper = true;
        nextLevel = 0;
        nextValue = "";
        valueLanguage = "";
    }
    void SuperLaserCadencyUpdate()
    {
        isSuper = true;
        nextLevel = 0;
        nextValue = $"{bonusDealer.MoreLaserCadencyPerc}%";
    }

    void UpdateBonus(BonusSelection.BonusType type, int[] values)
    {
        int currentLevel = currentBonusLevels[type];
        //Debug.Log($"{this.gameObject.name} -> CurrentLevel: {currentLevel}");
        nextLevel = currentLevel + 1;
        nextValue = $"{values[currentLevel]}";
    }

}
