using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class ObjSpotScript : MonoBehaviour
{
    const float closeRange = 50;
    [ShowInInspector, Sirenix.OdinInspector.ReadOnly] int closeSpotsNumber;
    const float veryCloseRange = 25;
    [ShowInInspector, Sirenix.OdinInspector.ReadOnly] float veryCloseSpotsNumber;

    public List<ObjSpotScript> CloseSpots = new();// { get; private set;}
    public List<ObjSpotScript> VeryCloseSpots = new();// { get; private set;}    

    static public UnityEvent UpdateLists = new UnityEvent();

    private void OnValidate()
    {
        UpdateLists.AddListener(PopulateCloseSpots);
        PopulateCloseSpots();
    }

    [Button, PropertyOrder(-1)]
    void CallUpdateLists()
    {
        UpdateLists?.Invoke();
        Debug.Log($"CloseRange: {closeRange}, VeryCloseRange: {veryCloseRange}");
    }

    List<ObjSpotScript> validSpots = new List<ObjSpotScript>();
    public ObjSpotScript GetClosePoint(List<ObjSpotScript> invalidSpots, bool otherCountZero = false)
    {
        validSpots = new(CloseSpots);
        for(int i = 0; i<invalidSpots.Count; i++)
        {
            validSpots.Remove(invalidSpots[i]);
        }
        if (validSpots.Count == 0 && !otherCountZero)
        {
            return GetVeryClosePoint(invalidSpots, otherCountZero:true);
        }
        else if (otherCountZero) return null;

        ObjSpotScript closeSpot = validSpots[Random.Range(0, validSpots.Count)];
        return closeSpot.GetComponent<ObjSpotScript>();
    }
    public ObjSpotScript GetVeryClosePoint(List<ObjSpotScript> invalidSpots, bool otherCountZero = false)
    {
        validSpots = new(VeryCloseSpots);
        for (int i = 0; i<invalidSpots.Count; i++)
        {
            validSpots.Remove(invalidSpots[i]);
        }
        if (validSpots.Count == 0 && !otherCountZero)
        {
            return GetClosePoint(invalidSpots, otherCountZero: true);
        }
        else if (otherCountZero) return null;

        ObjSpotScript closeSpot = validSpots[Random.Range(0, validSpots.Count)];
        return closeSpot.GetComponent<ObjSpotScript>();
    }

    private void PopulateCloseSpots()
    {
        if(transform.parent == null) return;

        float closeSqr = closeRange * closeRange;
        float veryCloseSqr = veryCloseRange * veryCloseRange;

        CloseSpots.Clear();
        VeryCloseSpots.Clear();

        foreach (Transform t in transform.parent)
        {
            if (t == transform) continue;
            if (Vector2.SqrMagnitude(transform.position - t.position) <= closeSqr)
            {
                CloseSpots.Add(t.GetComponent<ObjSpotScript>());
            }
            if (Vector2.SqrMagnitude(transform.position - t.position) <= veryCloseSqr)
            {
                VeryCloseSpots.Add(t.GetComponent<ObjSpotScript>());
            }
        }

        foreach (ObjSpotScript spot in VeryCloseSpots)
        {
            if (CloseSpots.Contains(spot.GetComponent<ObjSpotScript>()))
                CloseSpots.Remove(spot.GetComponent<ObjSpotScript>());
        }

        closeSpotsNumber = CloseSpots.Count;
        if (closeSpotsNumber == 0) Debug.Log($"No close spots to {transform.parent.transform.parent.name}.{transform.parent.name}.{this.name}");
        veryCloseSpotsNumber = VeryCloseSpots.Count;
        //if (veryCloseSpotsNumber == 0) Debug.Log($"No very close spots to {transform.parent.transform.parent.name}.{transform.parent.name}.{this.name}");
    }

    bool isDebug = false;
    private void OnDrawGizmosSelected()
    {
        if(!isDebug) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, closeRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, veryCloseRange);
    }
}
