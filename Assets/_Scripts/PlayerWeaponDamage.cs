using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponDamage : MonoBehaviour
{
    [SerializeField] float damage = 1;
    public float Damage {get { return damage; } }
}