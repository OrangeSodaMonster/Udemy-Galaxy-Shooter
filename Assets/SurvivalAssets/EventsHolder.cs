using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EventsClass
{
    public List<EventInfoClass> MyEvents = new List<EventInfoClass>();
}
[Serializable]
public class EventInfoClass
{
    [HorizontalGroup("1",0.6f), LabelWidth(45)]
    public MMF_Player Event;
    [HorizontalGroup("1"), LabelWidth(45)]
    public bool Around;
    [HorizontalGroup("1"), LabelWidth(25)]
    public int CD = 0;
}

public class EventsHolder : MonoBehaviour
{
    [SerializeField] Transform EventsObj;
    [SerializeField, GUIColor("cyan")] Transform PositiveEventsObj;
    [Space]
    public List<EventsClass> ListOfEvents = new();
    [GUIColor("cyan")]
    public List<EventsClass> ListOfPositiveEvents = new();
    [Space]
    [SerializeField] int eventCd = 2;
    [SerializeField] int posEventCd = 1;

    [Button, PropertyOrder(-1), HorizontalGroup("1")]
    void SetEvents()
    {
        ListOfEvents.Clear();
        foreach(Transform child in EventsObj)
        {
            ListOfEvents.Add(new());
            foreach (Transform childChild in child)
            {
                ListOfEvents[ListOfEvents.Count-1].MyEvents.Add(new EventInfoClass());
                int MyEventsSize = ListOfEvents[ListOfEvents.Count-1].MyEvents.Count;
                ListOfEvents[ListOfEvents.Count-1].MyEvents[MyEventsSize-1].Event = childChild.GetComponent<MMF_Player>();
                if(childChild.GetComponentInChildren<EventAddToSpawnAround>() != null)
                    ListOfEvents[ListOfEvents.Count-1].MyEvents[MyEventsSize-1].Around = true;
            }
        }
    }
    [Button, GUIColor("cyan"), PropertyOrder(-1), HorizontalGroup("1")]
    void SetPositiveEvents()
    {
        ListOfPositiveEvents.Clear();
        foreach (Transform child in PositiveEventsObj)
        {
            ListOfPositiveEvents.Add(new());
            foreach (Transform childChild in child)
            {
                ListOfPositiveEvents[ListOfPositiveEvents.Count-1].MyEvents.Add(new EventInfoClass());
                int MyEventsSize = ListOfPositiveEvents[ListOfPositiveEvents.Count-1].MyEvents.Count;
                ListOfPositiveEvents[ListOfPositiveEvents.Count-1].MyEvents[MyEventsSize-1].Event = childChild.GetComponent<MMF_Player>();
                if (childChild.GetComponentInChildren<EventAddToSpawnAround>() != null)
                    ListOfPositiveEvents[ListOfPositiveEvents.Count-1].MyEvents[MyEventsSize-1].Around = true;
            }
        }
    }

    private void OnEnable()
    {
        SurvivalManager.OnNewObjectiveSpawn.AddListener(ResetHasPickedAround);
    }
    bool hasPickedAroundForNextObj = false;
    void ResetHasPickedAround()
    {
        hasPickedAroundForNextObj = false;
    }
    
    [Button, HorizontalGroup("2")]
    public void CallEvent(int level)
    {
        SubtractEventsCD();

        int index = 0;
        for (int i = 0; i < 7; i++)
        {
            index = UnityEngine.Random.Range(0, ListOfEvents[level].MyEvents.Count);
            if (ListOfEvents[level].MyEvents[index].CD <= 0 &&
                (!ListOfEvents[level].MyEvents[index].Around || !hasPickedAroundForNextObj))
                break;
        }          

        ListOfEvents[level].MyEvents[index].Event.PlayFeedbacks();
        ListOfEvents[level].MyEvents[index].CD = eventCd;
        if(ListOfEvents[level].MyEvents[index].Around) hasPickedAroundForNextObj = true;
    }    

    [Button, GUIColor("cyan"), HorizontalGroup("2")]
    public void CallPositiveEvent(int level)
    {
        SubtractPositiveEventsCD();

        int index = 0;
        for (int i = 0; i < 5; i++)
        {
            index = UnityEngine.Random.Range(0, ListOfPositiveEvents[level].MyEvents.Count);
            if (ListOfPositiveEvents[level].MyEvents[index].CD <= 0) break;
        }

        ListOfPositiveEvents[level].MyEvents[index].Event.PlayFeedbacks();
        ListOfPositiveEvents[level].MyEvents[index].CD = posEventCd;
    }

    void SubtractEventsCD()
    {
        for(int i = 0; i < ListOfEvents.Count; i++)
        {
            for (int j = 0; j < ListOfEvents[i].MyEvents.Count; j++)
            {
                ListOfEvents[i].MyEvents[j].CD--;

                if(ListOfEvents[i].MyEvents[j].CD < 0)
                    ListOfEvents[i].MyEvents[j].CD = 0;
            }
        }
    }

    void SubtractPositiveEventsCD()
    {
        for (int i = 0; i < ListOfPositiveEvents.Count; i++)
        {
            for (int j = 0; j < ListOfPositiveEvents[i].MyEvents.Count; j++)
            {
                ListOfPositiveEvents[i].MyEvents[j].CD--;

                if (ListOfPositiveEvents[i].MyEvents[j].CD < 0)
                    ListOfPositiveEvents[i].MyEvents[j].CD = 0;
            }
        }
    }
}
