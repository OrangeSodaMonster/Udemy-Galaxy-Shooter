using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PushPickUps : MonoBehaviour
{
    [SerializeField] float maxPushForce = 3;
    public float MaxPushForce { get => maxPushForce; set => maxPushForce = value; }
    public float TimeToMaxPullSpeed = 2f;
}
