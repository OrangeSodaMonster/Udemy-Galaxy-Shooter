using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BonusShieldDisabler : MonoBehaviour
{
    private void OnEnable()
    {
        SurvivalManager.OnBossDestroyed.AddListener(DisableShield);
    }

    void DisableShield()
    {
        //if(gameObject.activeInHierarchy)
            gameObject.SetActive(false);
    }
}
