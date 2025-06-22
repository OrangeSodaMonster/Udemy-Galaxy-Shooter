using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpsLog : MonoBehaviour
{
    [Serializable]
    public class Droped
    {
        public int Metal;
        public int RareMetal;
        public int EnergyCrystal;
        public int CondensedEnergyCrystal;
    }

    public Droped PickedDrops = new Droped();
    public Droped LostDrops = new Droped();

    static public PickUpsLog Instance;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }
}