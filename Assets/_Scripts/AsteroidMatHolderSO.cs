using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AsteroidMats", menuName = "MySOs/AsteroidMatHolder")]
public class AsteroidMatHolderSO : ScriptableObject
{
    [field: SerializeField] public Material[] Mats { get; private set; }
}