using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        nextLevel = bonusDealer.LaserPower + 1;
        nextValue = $"{bonusDealer.LaserPowerExtraDamage[nextLevel-1]}";
    }
    void DronePowerUpdate()
    {
        nextLevel = bonusDealer.DronePower + 1;
        nextValue = $"{bonusDealer.DronePowerExtraDamage[nextLevel-1]}";
    }
    void IonStreamPowerUpdate()
    {
        nextLevel = bonusDealer.IonStreamPower + 1;
        nextValue = $"{bonusDealer.IonStreamPowerExtraDamage[nextLevel-1]}";
    }
    void BombPowerUpdate()
    {
        nextLevel = bonusDealer.BombPower + 1;
        nextValue = $"{bonusDealer.BombPowerExtraDamage[nextLevel-1]}";
    }
    void CadencyUpdate()
    {
        nextLevel = bonusDealer.LaserIonStreamCadency + 1;
        nextValue = $"{bonusDealer.LaserIonStreamCadencyPerc[nextLevel-1]}%";
    }
    void BombGenerationUpdate()
    {
        nextLevel = bonusDealer.BombGeneration + 1;
        nextValue = $"{bonusDealer.BombRegenerationTimer[nextLevel-1]}s";
    }
    void EnemyExtraDamageUpdate()
    {
        nextLevel = bonusDealer.ExtraDamageToEnemies + 1;
        nextValue = $"{bonusDealer.ExtraDamageToEnemiesPerc[nextLevel-1]}%";
    }
    void ObjectiveExtraDamageUpdate()
    {
        nextLevel = bonusDealer.ExtraDamageToObjectives + 1;
        nextValue = $"{bonusDealer.ExtraDamageToObjectivesPerc[nextLevel-1]}%";
    }
    void TractorUpdate()
    {
        nextLevel = bonusDealer.Tractor + 1;
        nextValue = $"{bonusDealer.TractorExtraPerc[nextLevel-1]}%";
    }
    void RangeUpdate()
    {
        nextLevel = bonusDealer.DroneIonStreamBombRange + 1;
        nextValue = $"{bonusDealer.DroneIonStreamRangeExtraPerc[nextLevel-1]}%";
    }
    void MobilityUpdate()
    {
        nextLevel = bonusDealer.Mobility + 1;
        nextValue = $"{bonusDealer.MobilityExtraPerc[nextLevel-1]}%";
    }
    void PowerUpDropUpdate()
    {
        nextLevel = bonusDealer.PowerUpDrop + 1;
        nextValue = $"{bonusDealer.PowerUpDropExtraPerc[nextLevel-1]}%";
    }
    void CristalDropUpdate()
    {
        nextLevel = bonusDealer.EnergyCristalsDrop + 1;
        nextValue = $"{bonusDealer.EnergyCristalsExtraPerc[nextLevel-1]}%";
    }
    void AutoConvertionUpdate()
    {
        nextLevel = bonusDealer.AutoConvertion + 1;
        nextValue = $"{bonusDealer.AutoConvertionTimer[nextLevel-1]}s";
    }
    void HpRecoveryUpdate()
    {
        nextLevel = bonusDealer.HP_Recovery + 1;
        nextValue = $"{bonusDealer.HpRecoveryTimerExtraPerc[nextLevel-1]}%";
    }
    void ShieldStrenghtUpdate()
    {
        nextLevel = bonusDealer.ShieldStrenght + 1;
        nextValue = $"{bonusDealer.ShieldExtraStr[nextLevel-1]}";
    }
    void ShieldRecoveryUpdate()
    {
        nextLevel = bonusDealer.ShieldRecovery + 1;
        nextValue = $"{bonusDealer.ShieldRecoveryExtraPerc[nextLevel-1]}%";
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
        nextValue = $"{bonusDealer.SuperBombExtraDamage}";
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

}
