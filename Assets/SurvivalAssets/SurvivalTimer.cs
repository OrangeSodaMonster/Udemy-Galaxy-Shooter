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
        public float Time;
        public int Level;
    }

    [HorizontalGroup("0"), MinValue(60)]
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
    [GUIColor("red")]
    public SectionEvent EventAtEnd;
    [GUIColor("cyan")]
    [ShowIf("@NumPositiveEvents > 0")]
    public List<SectionEvent> PositiveEvents;
    //Tempo para lidar com o último evento antes de iniciar a próxima seção
    [MinValue(0)]
    public float LastEventExtraTime = 20;
}

public class SurvivalTimer : MonoBehaviour
{
    public List<SurvivalSection> Sections;

    public float TotalTime;

    private void Update()
    {
        TotalTime += Time.deltaTime;
    }

    private void OnValidate()
    {
        SetTimers();
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

            float timeBetweenPositiveEvents = Sections[i].Duration / (Sections[i].PositiveEvents.Count + 1);
            for (int j = 0; j < Sections[i].PositiveEvents.Count; j++)
            {
                Sections[i].PositiveEvents[j].Time = timeBetweenPositiveEvents * (j+1) + totalSectionsDuration;
                if (useRandom)
                    Sections[i].PositiveEvents[j].Time += UnityEngine.Random.Range(-timeBetweenPositiveEvents, timeBetweenPositiveEvents);
            }

            Sections[i].EventAtEnd.Time = Sections[i].Duration + totalSectionsDuration;
            totalSectionsDuration += Sections[i].Duration + Sections[i].LastEventExtraTime;
        }
    }

}
