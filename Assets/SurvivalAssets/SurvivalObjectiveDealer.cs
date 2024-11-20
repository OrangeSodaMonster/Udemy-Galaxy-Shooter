using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Serializable]
class PossibleObjectives
{
    [Serializable]
    public class ObjectiveAndWeight
    {
        [HorizontalGroup("0")]
        public ObjectiveRandomizer Objective;
        [HorizontalGroup("0")]
        public float Weight = 10;
    }

    public List<ObjectiveAndWeight> objectives = new();
}

public class SurvivalObjectiveDealer : MonoBehaviour
{
    [BoxGroup("Quadrants")]
    [SerializeField] Transform CenterQuadrant;
    [SerializeField] List<ObjSpotScript> CenterSpots;

    [SerializeField, GUIColor("lightBlue")] Transform objParents;
    [SerializeField] List<PossibleObjectives> objectivesPerSection = new();

    [Sirenix.OdinInspector.ReadOnly]
    public List<ObjSpotScript> InUseSpots;
    [Sirenix.OdinInspector.ReadOnly]
    public static bool IsWaitingEndEvent = false;
    int numActiveObjectives = 0;
    ObjSpotScript lastObj;
    Transform player;
    int numObjParents;

    private void Awake()
    {
        PopulateLists();
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerHP>().transform;

        numObjParents = objParents.childCount;
    }

    private void Update()
    {
        if(numActiveObjectives == 0 && !GameStatus.IsPortal && !IsWaitingEndEvent)
        {
            PickAnObjective();
        }
    }

    void PickAnObjective()
    {
        if(lastObj == null)
        {
            SetVeryCloseObjectiveFromPlayer(CenterSpots); // TO DO: send right quadrant
        }
        else
        {
            SetCloseObjective();
        }
    }

    [Button, GUIColor("lightGreen")] // Called when lastObj == null;
    void SetVeryCloseObjectiveFromPlayer(List<ObjSpotScript> quadrantSpots)
    {
        if (numActiveObjectives >= numObjParents) return;   
        lastObj = FindVeryCloseToNearestSpotFromPlayer(quadrantSpots);
        GameObject obj = ObjPools.PoolObjective(GetObjective());

        HandleObjActivation(obj);
    }

    public void SetVeryCloseObjective()
    {
        if (numActiveObjectives >= numObjParents) return;
        lastObj = lastObj.GetVeryClosePoint(InUseSpots);
        GameObject obj = ObjPools.PoolObjective(GetObjective());

        HandleObjActivation(obj);
    }
    public void SetVeryCloseObjective(GameObject obj)
    {
        if (numActiveObjectives >= numObjParents) return;
        lastObj = lastObj.GetVeryClosePoint(InUseSpots);

        HandleObjActivation(obj);
    }

    void SetCloseObjective()
    {
        if (numActiveObjectives >= numObjParents) return;
        lastObj = lastObj.GetClosePoint(InUseSpots);
        GameObject obj = ObjPools.PoolObjective(GetObjective());

        HandleObjActivation(obj);
    }

    void HandleObjActivation(GameObject obj)
    {        
        if(obj == null) return;

        Transform parent = GetFirstDisabledObjParent();        
        parent.GetComponent<ObjSpotHandler>().CurrentSpot = lastObj;
        InUseSpots.Add(lastObj);
        obj.transform.position = parent.position;
        obj.transform.parent = parent;
        obj.SetActive(true);
        parent.position = lastObj.transform.position;
        parent.gameObject.SetActive(true);
        numActiveObjectives++;
    }

    Transform GetFirstDisabledObjParent()
    {
        for(int i = 0; i < objParents.childCount; i++)
        {
            if (!objParents.GetChild(i).gameObject.activeSelf)
            {
                return objParents.GetChild(i);
            }
        }
            return null;
    }

    ObjSpotScript FindNearestSpotFromPlayer(List<ObjSpotScript> quadrantSpots)
    {
        float minDistSqr = float.MaxValue;
        if (quadrantSpots == null) quadrantSpots = CenterSpots;
        ObjSpotScript spot = quadrantSpots[0];
        for(int i = 0; i < quadrantSpots.Count; i++)
        {
            float distSqrToSpot = Vector2.SqrMagnitude(player.position - quadrantSpots[i].transform.position);

            if (distSqrToSpot < minDistSqr)
            {
                minDistSqr = distSqrToSpot;
                spot = quadrantSpots[i];
            }
        }

        return spot;
    }

    ObjSpotScript FindVeryCloseToNearestSpotFromPlayer(List<ObjSpotScript> quadrantSpots)
    {
        ObjSpotScript nearest = FindNearestSpotFromPlayer(quadrantSpots);

        int index = UnityEngine.Random.Range(0, nearest.VeryCloseSpots.Count+1);

        if (index == nearest.VeryCloseSpots.Count && !InUseSpots.Contains(nearest)) // Para permitir que o próprio ponto mais próximo seja selecionado
        {
            return nearest;
        }
        else
            return nearest.GetVeryClosePoint(InUseSpots);
    }

    GameObject GetObjective()
    {       
        return ChooseObjectiveByWeight(objectivesPerSection[SurvivalManager.CurrentSection].objectives);

        GameObject ChooseObjectiveByWeight(List<PossibleObjectives.ObjectiveAndWeight> objs)
        {
            float totalWeight = 0;
            for (int i = 0; i < objs.Count; i++)
            {
                totalWeight += objs[i].Weight;
            }

            float value = UnityEngine.Random.Range(0, totalWeight);

            for (int i = 0; i < objs.Count; i++)
            {
                if(value <= objs[i].Weight)
                    return objs[i].Objective.gameObject;
                else
                    value -= objs[i].Weight;
            }

            Debug.Log("DIDN'T FIND AN OBJECTIVE, DEFAULTING TO INDEX 0");
            return objs[0].Objective.gameObject;
        }
    }

    public void SubtractObjective()
    {
        numActiveObjectives--;
    }

    [Button]
    void TestGetObjective()
    {
        Debug.Log(GetObjective().name);
    }

    [Button]
    void TestFindVeryCloseToNearest()
    {
        PopulateLists();
        player = FindObjectOfType<PlayerHP>().transform;
        ObjSpotScript spot = FindVeryCloseToNearestSpotFromPlayer(CenterSpots);
        Color oldColor = spot.GetComponent<SpriteRenderer>().color;
        spot.GetComponent<SpriteRenderer>().color = Color.red;
        StartCoroutine(ReturnColor());
        
        IEnumerator ReturnColor()
        {
            yield return new WaitForSeconds(.5f);
            spot.GetComponent<SpriteRenderer>().color = oldColor;
        }
    }

    private void OnValidate()
    {
        PopulateLists();
    }

    void PopulateLists()
    {
        CenterQuadrant.GetComponentsInChildren(CenterSpots);
    }

    public void SetObjPerSectionSize(int size)
    {
        Debug.Log($"OBJ LIST SIZE: {size}");
        objectivesPerSection.SetLength(size);
    }
}
