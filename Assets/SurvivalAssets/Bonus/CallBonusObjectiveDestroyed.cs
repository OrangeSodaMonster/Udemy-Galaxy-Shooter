using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CallBonusObjectiveDestroyed : MonoBehaviour
{
    private void OnDisable()
    {
        SurvivalManager.OnBonusAsteroidDestroyed.Invoke();
        Debug.Log("<color=magenta>Bonus Asteroid Disabled</color>");
    }
}
