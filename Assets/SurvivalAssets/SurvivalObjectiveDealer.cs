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
    [SerializeField] QuadrantDealer CenterQuadrant;
    [SerializeField] float nextObjCD = 60;
    //[SerializeField] List<ObjSpotScript> CenterSpots;

    [SerializeField, GUIColor("lightBlue")] Transform objParents;
    [SerializeField] List<PossibleObjectives> objectivesPerSection = new();
        
    [HorizontalGroup("Sent",.5f)]
    [SerializeField] GameObject Sentinel;
    [HorizontalGroup("Sent")]
    [SerializeField] int SentNum;
    [HorizontalGroup("Sent")]
    [SerializeField] float SentRadius = 3;

    [Sirenix.OdinInspector.ReadOnly]
    public List<ObjSpotScript> InUseSpots;
    public static bool IsWaitingEndEvent = false;
    public static ObjSpotScript LastObj;

    int numActiveObjectives = 0;
    Transform player;
    int numObjParents;    
    PoolRefs poolRefs;
    float nextObjTimer;    

    private void Start()
    {
        player = FindObjectOfType<PlayerHP>().transform;
        poolRefs = FindObjectOfType<PoolRefs>();

        numObjParents = objParents.childCount;
        SurvivalManager.CurrentQuadrant = CenterQuadrant;
        SurvivalTimers.OnSectionChange.AddListener(ResetTimer);
    }

    private void Update()
    {
        if (numActiveObjectives == 0 && !GameStatus.IsPortal && !IsWaitingEndEvent)
        {
            PickAnObjective();
            ResetTimer();
        }

        //if (nextObjTimer > nextObjCD && LastObj != null)
        //{
        //    SetACloseObjectiveFromPlayerAtQuad(SurvivalManager.CurrentQuadrant.Spots);
        //    ResetTimer();
        //}

        //if(!GameStatus.IsPortal && !IsWaitingEndEvent)
        //    nextObjTimer += Time.deltaTime;
    }

    void PickAnObjective()
    {
        Debug.Log($"LastObj: {LastObj}");
        if (LastObj == null)
        {
            Debug.Log($"NEW OBJ AT QUAD: {SurvivalManager.CurrentQuadrant.name}");
            SetACloseObjectiveFromPlayerAtQuad(SurvivalManager.CurrentQuadrant.Spots);
        }
        else
        {
            Debug.Log($"NEW OBJ CLOSE: {SurvivalManager.CurrentQuadrant.name}");
            SetCloseObjective();
        }        
    }

    [Button, GUIColor("lightGreen")] // Called when lastObj == null;
    void SetACloseObjectiveFromPlayerAtQuad(List<ObjSpotScript> quadrantSpots)
    {
        if (numActiveObjectives >= numObjParents) return;   
        LastObj = FindVeryCloseToNearestSpotFromPlayer(quadrantSpots);
        GameObject obj = ObjPools.PoolObjective(GetObjective());

        HandleObjActivation(obj);
    }

    public void SetVeryCloseObjective()
    {
        if (numActiveObjectives >= numObjParents) return;
        LastObj = LastObj.GetVeryClosePoint(InUseSpots);
        GameObject obj = ObjPools.PoolObjective(GetObjective());

        HandleObjActivation(obj);
    }
    public void SetVeryCloseObjective(GameObject obj)
    {
        if (numActiveObjectives >= numObjParents) return;
        LastObj = LastObj.GetVeryClosePoint(InUseSpots);

        HandleObjActivation(obj);
    }

    void SetCloseObjective()
    {
        if (numActiveObjectives >= numObjParents) return;
        LastObj = LastObj.GetClosePoint(InUseSpots);
        GameObject obj = ObjPools.PoolObjective(GetObjective());

        HandleObjActivation(obj);
    }

    void HandleObjActivation(GameObject obj)
    {        
        if(obj == null) return;

        Transform parent = GetFirstDisabledObjParent();        
        parent.GetComponent<ObjSpotHandler>().CurrentSpot = LastObj;
        InUseSpots.Add(LastObj);
        obj.transform.position = parent.position;
        obj.transform.parent = parent;
        obj.SetActive(true);
        parent.position = LastObj.transform.position;
        parent.gameObject.SetActive(true);
        numActiveObjectives++;
        HandleSentinels();
    }

    void HandleSentinels()
    {
        if (Sentinel == null || SentNum == 0) return;

        float originalAngle = UnityEngine.Random.Range(0, 360);
        float angleVariation = 360/SentNum;

        Vector2 pos = LastObj.transform.position;

        for (int i = 0; i < SentNum; i++)
        {
            float angle = (originalAngle + angleVariation * i) * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
            direction *= SentRadius;

            GameObject Sent = poolRefs.Poolers[Sentinel].GetPooledGameObject();
            Sent.transform.position = pos + direction;
            Sent.SetActive(true);            
        }

        Sentinel = null;
        SentNum = 0;
    }

    public void SetSentinels (GameObject sentinel, int sentNum)
    {
        if(!sentinel.GetComponent<SentinelShoot>()) return;
        Sentinel = sentinel;
        SentNum = sentNum;
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
        CenterQuadrant.PopulateList();
        player = FindObjectOfType<PlayerHP>().transform;
        ObjSpotScript spot = FindVeryCloseToNearestSpotFromPlayer(CenterQuadrant.Spots);
        Color oldColor = spot.GetComponent<SpriteRenderer>().color;
        spot.GetComponent<SpriteRenderer>().color = Color.red;
        StartCoroutine(ReturnColor());
        
        IEnumerator ReturnColor()
        {
            yield return new WaitForSeconds(.5f);
            spot.GetComponent<SpriteRenderer>().color = oldColor;
        }
    }

    private void ResetTimer()
    {
        nextObjTimer = 0;
    }

    public void SetObjPerSectionSize(int size)
    {
        Debug.Log($"OBJ LIST SIZE: {size}");
        objectivesPerSection.SetLength(size);
    }

    public static void EraseLastObj()
    {
        LastObj = null;
        Debug.Log("LastObj NULL");
    }
}
