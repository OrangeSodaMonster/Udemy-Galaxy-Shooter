using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SurvivalSection;

[Serializable]
public class SurvivalSection
{
    [Serializable]
    public class SectionEvent
    {
        [Tooltip("The Time For This Event To Happen")]
        [HorizontalGroup("1")]
        public float Time;
        [Tooltip("Event Difficult Level, Starts at 0")]
        [MinValue(0)][HorizontalGroup("1")]
        public int Level;
    }

    [HorizontalGroup("0"), MinValue(3)]
    public float Duration = 120;
    [HorizontalGroup("0"), MinValue(0)]
    public int NumPositiveEvents = 1;
    [HorizontalGroup("1"), MinValue(1)]
    public int NumberOfEvents = 5;
    [HorizontalGroup("1")]
    public float TimeBetweenEvents = 10;
    [HorizontalGroup("1"), MinValue(0)]
    public float EventTimeVariation = 10;
    public List<SectionEvent> Events;
    [GUIColor("cyan")]
    [ShowIf("@NumPositiveEvents > 0")]
    public List<SectionEvent> PositiveEvents;
    [GUIColor("red")]
    public SectionEvent EndEvent;
    //Tempo para lidar com o último evento antes de iniciar a próxima seção
    [MinValue(0)]
    public float LastEventExtraTime = 20;
}

[RequireComponent(typeof(EventsHolder))]
public class SurvivalTimer : MonoBehaviour
{
    public List<SurvivalSection> Sections;
    public float TotalTime;
    public int CurrentSection;

    EventsHolder EventsHolder;

    private void Awake()
    {
        EventsHolder = GetComponent<EventsHolder>();
    }

    private void Start()
    {
        SetTimers(useRandom:true);

        #if UNITY_EDITOR
            CheckEventLevels();
        #endif
    }

    private void Update()
    {
        TotalTime += Time.deltaTime;
        CheckTimers();
    }

    private void OnValidate()
    {
        SetTimers();
        CheckEventLevels();
    }

    [Button("SetTimers"), PropertyOrder(-1)]
    void SetTimers(bool useRandom = false)
    {
        float totalSectionsDuration = 0;
        for (int i = 0; i < Sections.Count; i++)
        {
            //Events
            while (Sections[i].Events.Count < Sections[i].NumberOfEvents)
            {
                Sections[i].Events.Add(new SectionEvent());
            }
            while (Sections[i].Events.Count > Sections[i].NumberOfEvents)
            {
                Sections[i].Events.RemoveAt(Sections[i].Events.Count-1);
            }            

            float timeBetweenEvents = Sections[i].Duration / (Sections[i].Events.Count + 1);
            Sections[i].TimeBetweenEvents = timeBetweenEvents;
            for (int j = 0; j < Sections[i].Events.Count; j++)
            {
                Sections[i].Events[j].Time = timeBetweenEvents * (j+1) + totalSectionsDuration;
                if(useRandom)
                    Sections[i].Events[j].Time += UnityEngine.Random.Range(-Sections[i].EventTimeVariation, Sections[i].EventTimeVariation);
            }

            //Positive Events 
            //TO DO: SET TIME BETWEEN EVENTS
            while (Sections[i].PositiveEvents.Count < Sections[i].NumPositiveEvents)
            {
                Sections[i].PositiveEvents.Add(new SectionEvent());
            }
            while (Sections[i].PositiveEvents.Count > Sections[i].NumPositiveEvents)
            {
                Sections[i].PositiveEvents.RemoveAt(Sections[i].PositiveEvents.Count-1);
            }

            float timeBetweenPositiveEvents = Sections[i].Duration / (Sections[i].PositiveEvents.Count + 1);
            for (int j = 0; j < Sections[i].PositiveEvents.Count; j++)
            {
                Sections[i].PositiveEvents[j].Time = timeBetweenPositiveEvents * (j+1) + totalSectionsDuration;
                if (useRandom)
                    Sections[i].PositiveEvents[j].Time += UnityEngine.Random.Range(-timeBetweenPositiveEvents, timeBetweenPositiveEvents);
            }

            Sections[i].EndEvent.Time = Sections[i].Duration + totalSectionsDuration;
            totalSectionsDuration += Sections[i].Duration + Sections[i].LastEventExtraTime;
        }
    }

    void CheckTimers()
    {
        for (int i = 0; i < Sections.Count; i++)
        {   
            //Events
            for (int j = 0; j < Sections[i].Events.Count; j++)
            {
                if(TotalTime > Sections[i].Events[j].Time)
                {
                    EventsHolder.CallEvent(Sections[i].Events[j].Level);
                    Sections[i].Events.RemoveAt(j);
                }           
            }
            //Positive Events
            for (int j = 0; j < Sections[i].PositiveEvents.Count; j++)
            {
                if (TotalTime > Sections[i].PositiveEvents[j].Time)
                {
                    EventsHolder.CallEvent(Sections[i].PositiveEvents[j].Level);
                    Sections[i].PositiveEvents.RemoveAt(j);
                }
            }
            //End Event
            if (TotalTime > Sections[i].EndEvent.Time)
            {
                EventsHolder.CallEvent(Sections[i].EndEvent.Level);
                Sections[i].EndEvent.Time = float.MaxValue;
            }
            //Change Section
            if (TotalTime > Sections[i].EndEvent.Time + Sections[i].LastEventExtraTime)
            {
                CurrentSection++;
                Sections.RemoveAt(i);
            }
        }       
    }

    void CheckEventLevels()
    {
        EventsHolder = GetComponent<EventsHolder>();

        for (int i = 0; i < Sections.Count; i++)
        {
            for (int j = 0; j < Sections[i].Events.Count; j++)
            {
                if (Sections[i].Events[j].Level > EventsHolder.ListOfEvents.Count-1)
                {
                    Debug.Log($"<color=orange> Event Level Out Of Range </color>");
                }
            }
        }

        for (int i = 0; i < Sections.Count; i++)
        {
            for (int j = 0; j < Sections[i].PositiveEvents.Count; j++)
            {
                if (Sections[i].PositiveEvents[j].Level > EventsHolder.ListOfPositiveEvents.Count-1)
                {
                    Debug.Log($"<color=orange> Positive Event Level Out Of Range </color>");
                }
            }
        }

        for (int i = 0; i < Sections.Count; i++)
        {
            if (Sections[i].EndEvent.Level > EventsHolder.ListOfEndEvents.Count-1)
            {
                Debug.Log($"<color=orange> End Event Level Out Of Range </color>");
            }
        }
    }
}
