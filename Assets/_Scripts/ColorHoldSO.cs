using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MyColorHolder", menuName = "MySOs/ColorHolder")]
public class ColorHoldSO : ScriptableObject
{
	[SerializeField] Color[] colors = null;
}