using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectiveCol", menuName = "MySOs/ObjectiveCol")]
public class ObjectiveColHolderSO : ScriptableObject
{
    [field: SerializeField] public PolygonCollider2D[] Colliders { get; private set; }    
}