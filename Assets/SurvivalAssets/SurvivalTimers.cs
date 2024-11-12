using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static SurvivalSection;

[Serializable]
public class SurvivalSection
{
    [Serializable]
    public class SectionEvent
    {
        [Tooltip("The Time For This Event To Happen")]
        [HorizontalGroup("1", .2f, LabelWidth = 35)]
        public float Time;

        [Tooltip("Choose Random Level of 2")]
        [HorizontalGroup("1", .2f, LabelWidth = 60)]
        public bool IsRandomLevel = false;        

        [Tooltip("Event Difficult Level, Starts at 0")]
        [MinValue(0)][HorizontalGroup("1")]
        public int Level;

        [Tooltip("Event Difficult Level is one of two")][GUIColor("yellow")]       
        [MinValue(0)][HorizontalGroup("1")][ShowIf("@IsRandomLevel == true")]
        public int PossibleLevel;
    }

    [HorizontalGroup("0"), MinValue(3)]
    public float Duration = 120;
    [HorizontalGroup("0"), MinValue(0), GUIColor("cyan")]
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
    [GUIColor("red"), HorizontalGroup("END", 0.25f)]
    public bool hasCalledEndEvent = false;
    [GUIColor("red"), HorizontalGroup("END")]
    public SectionEvent EndEvent;
    //Tempo para lidar com o último evento antes de iniciar a próxima seção
    [MinValue(0)][GUIColor("purple")]
    public float LastEventExtraTime = 20;
}

[RequireComponent(typeof(EventsHolder))]
public class SurvivalTimers : MonoBehaviour
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
            
            while (Sections[i].PositiveEvents.Count < Sections[i].NumPositiveEvents)
            {
                Sections[i].PositiveEvents.Add(new SectionEvent());
            }
            while (Sections[i].PositiveEvents.Count > Sections[i].NumPositiveEvents)
            {
                Sections[i].PositiveEvents.RemoveAt(Sections[i].PositiveEvents.Count-1);
            }

            //SET TIME BETWEEN EVENTS            
            List<int> eventIndexes = new List<int>();
            for (int j = 0; j < Sections[i].Events.Count; j++)
            {
                eventIndexes.Add(j);
            }
            eventIndexes.Add(eventIndexes.Count);

                for (int j = 0; j < Sections[i].PositiveEvents.Count; j++)
                {
                    int eventIndex = eventIndexes[UnityEngine.Random.Range(0, eventIndexes.Count)];
                    //Debug.Log($"Event Index: {eventIndex}");

                    if (eventIndex == (int)0)
                    {
                        Sections[i].PositiveEvents[j].Time = Sections[i].Events[eventIndex].Time * 0.5f;
                    }
                    else if (eventIndex == Sections[i].Events.Count)
                    {
                        float timeBetween = Sections[i].EndEvent.Time - Sections[i].Events[eventIndex - 1].Time;
                        Sections[i].PositiveEvents[j].Time = Sections[i].EndEvent.Time - (timeBetween * 0.5f);
                    }
                    else
                    {
                        float timeBetween = Sections[i].Events[eventIndex].Time;
                        timeBetween -= Sections[i].Events[eventIndex - 1].Time;
                        Sections[i].PositiveEvents[j].Time = Sections[i].Events[eventIndex].Time - (timeBetween * 0.5f);
                    } 

                    eventIndexes.Remove(eventIndex);
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
                    int level = Sections[i].Events[j].Level;
                    if(Sections[i].Events[j].IsRandomLevel)
                        level = GetRandomOfTwo(level, Sections[i].Events[j].PossibleLevel);

                    EventsHolder.CallEvent(level);
                    Sections[i].Events.RemoveAt(j);
                }           
            }

            //Positive Events
            for (int j = 0; j < Sections[i].PositiveEvents.Count; j++)
            {       
                if (TotalTime > Sections[i].PositiveEvents[j].Time)
                {
                    int level = Sections[i].PositiveEvents[j].Level;
                    if (Sections[i].PositiveEvents[j].IsRandomLevel)
                        level = GetRandomOfTwo(level, Sections[i].PositiveEvents[j].PossibleLevel);

                    EventsHolder.CallPositiveEvent(level);
                    Sections[i].PositiveEvents.RemoveAt(j);
                }
            }

            //End Event
            if (!Sections[i].hasCalledEndEvent && TotalTime > Sections[i].EndEvent.Time)
            {
                int level = Sections[i].EndEvent.Level;
                if (Sections[i].EndEvent.IsRandomLevel)
                    level = GetRandomOfTwo(level, Sections[i].EndEvent.PossibleLevel);

                EventsHolder.CallEndEvent(level);
                Sections[i].hasCalledEndEvent = true;
            }

            //Change Section
            if (TotalTime > Sections[i].EndEvent.Time + Sections[i].LastEventExtraTime)
            {
                CurrentSection++;
                Sections.RemoveAt(0);
                Debug.Log($"NEW SECTION: {CurrentSection}");
            }
        }       
    }

    int GetRandomOfTwo(int x, int y)
    {
        if (Mathf.Sign(UnityEngine.Random.Range(-1f, 1f)) == 1)
            return x;
        else
            return y;
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
