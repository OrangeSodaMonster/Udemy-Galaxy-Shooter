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
        //[HorizontalGroup("0")]
        //public ObjectiveRandomizer Objective;
        //[HorizontalGroup("0")]
        //public float Weight = 10;

        [HorizontalGroup("0", 0.12f), PreviewField(38, Alignment = ObjectFieldAlignment.Left), HideLabel]
        public GameObject Objective;
        [VerticalGroup("0/1"), LabelWidth(10), LabelText(""), Sirenix.OdinInspector.ReadOnly]
        public string Name;
        [VerticalGroup("0/1"), LabelWidth(40)]
        public float Weight = 10;
    }

    public List<ObjectiveAndWeight> objectives = new();
}

public class SurvivalObjectiveDealer : MonoBehaviour
{
    [SerializeField] int minObjectivesPerSection = 2;
    [BoxGroup("Quadrants")]
    [SerializeField] QuadrantDealer CenterQuadrant;
    [SerializeField] float nextObjCD = 60;
    //[SerializeField] List<ObjSpotScript> CenterSpots;

    [SerializeField, GUIColor("lightBlue")] Transform objParents;
    [SerializeField] List<PossibleObjectives> objectivesPerSection = new();    

    [Sirenix.OdinInspector.ReadOnly]
    public List<ObjSpotScript> InUseSpots;    
    public static ObjSpotScript LastObj;
    GameObject lastSpawnedObj;
    [ShowInInspector] public static List<GameObject> ObjectsToSpawnAround = new();

    int numActiveObjectives = 0;
    Transform player;
    int numObjParents;    
    PoolRefs poolRefs;

    SurvivalTimers timers;
    BonusRefScript bonusRef;
    SpawnAround spawnAround;

    int objectivesThisSection = 0;
    int lastBonusSpawned = 0; // check the last section a bonus was spawned to prevent spawning multiple times in the same section

    private void Start()
    {
        player = FindObjectOfType<PlayerHP>().transform;
        poolRefs = FindObjectOfType<PoolRefs>();
        spawnAround = GetComponent<SpawnAround>();

        numObjParents = objParents.childCount;
        SurvivalManager.CurrentQuadrant = CenterQuadrant;

        timers = FindObjectOfType<SurvivalTimers>();
        bonusRef = FindObjectOfType<BonusRefScript>();
    }

    private void OnValidate()
    {
        for (int i = 0; i < objectivesPerSection.Count; i++)
        {
            for(int j = 0; j < objectivesPerSection[i].objectives.Count; j++)
            {                
                if(objectivesPerSection[i].objectives[j].Objective == null) continue;
                if(objectivesPerSection[i].objectives[j].Objective.GetComponent<ObjectiveRandomizer>() == null)
                {
                    objectivesPerSection[i].objectives[j].Objective = null;
                    Debug.LogWarning("Not a valid objective");
                }
                else
                {
                    objectivesPerSection[i].objectives[j].Name = objectivesPerSection[i].objectives[j].Objective.name;
                }
            }
        }
    }

    private void Update()
    {
        if (!GameStatus.IsPaused && numActiveObjectives == 0 && !GameStatus.IsPortal &&
            !SurvivalManager.IsWaitingEndEvent && lastBonusSpawned < SurvivalManager.CurrentSection + 1)
        {
            if(timers.SectionTime >= timers.Sections[SurvivalManager.CurrentSection].Duration &&
                SurvivalManager.SurvivalState == SurvivalState.Spawning && objectivesThisSection >= minObjectivesPerSection)
            {
                lastBonusSpawned++;
                Instantiate(bonusRef.BonusOfSection[SurvivalManager.CurrentSection], SurvivalManager.CurrentQuadrant.BossPoint.position, Quaternion.identity);
                lastSpawnedObj = bonusRef.BonusOfSection[SurvivalManager.CurrentSection];
                SurvivalManager.IsWaitingEndEvent = true;
                //SurvivalManager.IsNextSectionReady = true;
                SurvivalManager.SurvivalState = SurvivalState.Bonus;
                Debug.Log("<color=cyan>STATE: Bonus</color>");
                SurvivalManager.OnBonusObjectiveSpawn.Invoke();
                objectivesThisSection = 0;

                if (ObjectsToSpawnAround.Count > 0)
                {
                    spawnAround.Spawn(SurvivalManager.CurrentQuadrant.BossPoint.position, ObjectsToSpawnAround, 6.1f);
                    ObjectsToSpawnAround.Clear();
                }
            }
            else
            {
                PickAnObjective();
                objectivesThisSection++;
            }
            SurvivalManager.OnNewObjectiveSpawn.Invoke();
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
        lastSpawnedObj = obj;
        parent.position = LastObj.transform.position;
        parent.gameObject.SetActive(true);
        numActiveObjectives++;
        HandleSpawnAround();
    }

    void HandleSpawnAround()
    {
        if (ObjectsToSpawnAround.Count > 0)
        {
            float range = lastSpawnedObj.gameObject.GetComponent<ObjectiveSizeRef>().GetRange();
            spawnAround.Spawn(LastObj.transform.position, ObjectsToSpawnAround, range);
            ObjectsToSpawnAround.Clear();
        }
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
