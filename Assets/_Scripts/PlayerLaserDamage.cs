using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum LaserType
{
    Frontal = 0,
    Spread = 1,
    Lateral = 2,
    Back = 3,
}

public class PlayerLaserDamage : MonoBehaviour
{
    [field:SerializeField] public int Damage { get; set; } = 1;
    public LaserType LaserType;
}