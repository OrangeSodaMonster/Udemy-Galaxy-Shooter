using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPowerUp : MonoBehaviour
{
    public float Duration = 10;
    public float SurvivalDuration = 5;
    public float HealCD = .5f;

    private void Start()
    {
        if (GameManager.IsSurvival)
            Duration = SurvivalDuration;
    }
}