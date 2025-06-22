using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusRefScript : MonoBehaviour
{
    public List<GameObject> BonusOfSection = new();

    [Button]
    public void SetListSize()
    {
        SurvivalTimers timers = FindAnyObjectByType<SurvivalTimers>();
        List<GameObject> list = new List<GameObject>(BonusOfSection);

        BonusOfSection.Clear();
        BonusOfSection = new(new GameObject[timers.Sections.Count]);

        for(int i = 0; i < list.Count; i++)
        {
            BonusOfSection[i] = list[i];
        }
    }
}
