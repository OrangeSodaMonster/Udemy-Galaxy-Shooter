using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXPoolerScript : MonoBehaviour
{
    [field: SerializeField] public MMSimpleObjectPooler LaserVFXPooler { get; private set; }
    [field: SerializeField] public MMSimpleObjectPooler ProjectileVFXPooler { get; private set; }
    [field: SerializeField] public MMSimpleObjectPooler DroneAttackVFXPooler { get; private set; }
    [field: SerializeField] public MMSimpleObjectPooler IonStreamVFXPooler { get; private set; }

    public static VFXPoolerScript Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}