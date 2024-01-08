using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InterfaceDataHolder", menuName = "MySOs/InterfaceDataHolder")]
public class InterfaceDataHolder : ScriptableObject
{
	[Header("Upgrade Box Colors")]
	public Color unavaliableColor = Color.grey;
    public Color avaliableColor = Color.white;
	public Color boughtColor = Color.green;
	public Color maxedColor = Color.yellow;

    [Header("Upgrade Costs")]
	public Sprite metalSprite = null;
	public Color metalColor;
    public Sprite alloySprite = null;
    public Color alloyColor;
    public Sprite energyCristalSprite = null;
    public Color energyCristalColor;
    public Sprite condensedCristalSprite = null;
    public Color condensedCristalColor;
}