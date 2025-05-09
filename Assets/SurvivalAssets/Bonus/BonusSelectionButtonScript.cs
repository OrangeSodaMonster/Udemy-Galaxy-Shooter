using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSelectionButtonScript : MonoBehaviour
{    
    public void ChooseBonus()
    {
        BonusSelectionBoxScript box = GetComponentInChildren<BonusSelectionBoxScript>();
        BonusSelection.BonusType type = box.BonusType;

        BonusPowersDealer.Instance.AddBonusLevel(type);
        BonusSelectionCanvasScript.Instance.LeaveBonusCanvas();
    }
}
