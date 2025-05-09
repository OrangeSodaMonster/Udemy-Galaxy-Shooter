using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BonusSelection;

public class SelectedBonusUI : SerializedMonoBehaviour
{
    public Dictionary<BonusType, Sprite> BonusSprites = new();

    //[Button]
    public void PopulateBonusSprires()
    {
        int numBonus = Enum.GetNames(typeof(BonusType)).Length;
        for (int i = 0; i < numBonus; i++)
        {
            BonusSprites.Add((BonusType)i, null);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
