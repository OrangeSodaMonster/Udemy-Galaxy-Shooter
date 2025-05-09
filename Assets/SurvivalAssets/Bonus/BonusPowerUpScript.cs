using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BonusPowerUpScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(BonusSelectionCanvasScript.Instance != null)
            BonusSelectionCanvasScript.Instance.OpenBonusCanvas();
        else
        {
            FindObjectOfType<BonusSelectionCanvasScript>(true).SetInstance();
            BonusSelectionCanvasScript.Instance.OpenBonusCanvas();
        }

        Destroy(gameObject);
    }
}
