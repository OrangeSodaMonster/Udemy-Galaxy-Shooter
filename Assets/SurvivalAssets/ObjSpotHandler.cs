using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class ObjSpotHandler : MonoBehaviour
{
    [Sirenix.OdinInspector.ReadOnly]
    public ObjSpotScript CurrentSpot;

    SurvivalObjectiveDealer objDealer;

    private void Start()
    {
        objDealer = FindFirstObjectByType<SurvivalObjectiveDealer>();
    }

    void ClearSpot()
    {
        if (CurrentSpot != null)
            objDealer.InUseSpots.Remove(CurrentSpot);
        else
            Debug.LogWarning($"CurrentSpot in {this.name} was null");

        CurrentSpot = null;
    }

    private void OnDisable()
    {
        ClearSpot();
    }
}
