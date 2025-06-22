using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BonusPowerUpScript : MonoBehaviour
{
    private void OnEnable()
    {
        //if (BonusSelection.Instance != null && !BonusSelection.Instance.ChechIfThereAreAPossibleBonusPick())
        //{
        //    gameObject.SetActive(false);
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (BonusSelection.Instance != null && !BonusSelection.Instance.ChechIfThereAreAPossibleBonusPick())
        {
            gameObject.SetActive(false);
            return;
        }

        if (BonusSelectionCanvasScript.Instance != null)
            BonusSelectionCanvasScript.Instance.OpenBonusCanvas();
        else
        {
            FindObjectOfType<BonusSelectionCanvasScript>(true).SetInstance();
            BonusSelectionCanvasScript.Instance.OpenBonusCanvas();
        }

        gameObject.SetActive(false);
    }

    private void Update()
    {
        SurvivalManager.IsBonusPickUpEnabled = true;
    }
}
