using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BonusSelectionButtonScript : MonoBehaviour
{
    BonusSelection.BonusType type = new();

    private void OnEnable()
    {
        StartCoroutine(OnEnableCoroutine());

        IEnumerator OnEnableCoroutine()
        {
             yield return null;

            BonusSelectionBoxScript box = GetComponentInChildren<BonusSelectionBoxScript>();
            type = box.BonusType;
            if (box.BonusType == BonusSelection.BonusType.NONE)
                GetComponent<ButtonScript>().PlayConfirmationSound = false;
            else
                GetComponent<ButtonScript>().PlayConfirmationSound = true;
        }
    }

    public void ChooseBonus()
    {
        if (type == BonusSelection.BonusType.NONE)
        {
            AudioManager.Instance.UpgradeFailSound.PlayFeedbacks();           
            return;
        }

        BonusSelection.Instance.AddToActiveBonuses(type);
        BonusPowersDealer.Instance.AddBonusLevel(type);
        SelectedBonusUI.Instance.UpdateIcons();

        BonusSelectionCanvasScript.Instance.LeaveBonusCanvas();
    }
}
