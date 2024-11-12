using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EventsClass
{
    public List<MMF_Player> MyEvents = new List<MMF_Player>();
}

public class EventsHolder : MonoBehaviour
{
    [SerializeField] Transform EventsObj;
    [SerializeField, GUIColor("cyan")] Transform PositiveEventsObj;
    [SerializeField, GUIColor("red")] Transform EndEventsObj;
    [Space]
    public List<EventsClass> ListOfEvents = new();
    [GUIColor("cyan")]
    public List<EventsClass> ListOfPositiveEvents = new();
    [GUIColor("red")]
    public List<EventsClass> ListOfEndEvents = new();

    [Button, PropertyOrder(-1), HorizontalGroup("1")]
    void SetEvents()
    {
        ListOfEvents.Clear();
        foreach(Transform child in EventsObj)
        {
            ListOfEvents.Add(new());
            foreach (Transform childChild in child)
            {
                ListOfEvents[ListOfEvents.Count-1].MyEvents.Add(childChild.GetComponent<MMF_Player>());
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
                ListOfPositiveEvents[ListOfPositiveEvents.Count-1].MyEvents.Add(childChild.GetComponent<MMF_Player>());
            }
        }
    }
    [Button, GUIColor("red"), PropertyOrder(-1), HorizontalGroup("1")]
    void SetEndEvents()
    {
        ListOfEndEvents.Clear();
        foreach (Transform child in EndEventsObj)
        {
            ListOfEndEvents.Add(new());
            foreach (Transform childChild in child)
            {
                ListOfEndEvents[ListOfEndEvents.Count-1].MyEvents.Add(childChild.GetComponent<MMF_Player>());
            }
        }
    }



    [Button, HorizontalGroup("2")]
    public void CallEvent(int level)
    {
        ListOfEvents[level].MyEvents[UnityEngine.Random.Range(0, ListOfEvents[level].MyEvents.Count)].PlayFeedbacks();
    }
    [Button, GUIColor("cyan"), HorizontalGroup("2")]
    public void CallPositiveEvent(int level)
    {
        ListOfPositiveEvents[level].MyEvents[UnityEngine.Random.Range(0, ListOfPositiveEvents[level].MyEvents.Count)].PlayFeedbacks();
    }
    [Button, GUIColor("red"), HorizontalGroup("2")]
    public void CallEndEvent(int level)
    {
        ListOfEndEvents[level].MyEvents[UnityEngine.Random.Range(0, ListOfEndEvents[level].MyEvents.Count)].PlayFeedbacks();
    }
}
