using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MySpriteHolder", menuName = "MySOs/SpriteHolder")]
public class SpriteHolderSO : ScriptableObject
{
    //[field: SerializeField] public Material[] Mats { get; private set; }
    [field: SerializeField] public Sprite[] Sprites { get; private set; }
}