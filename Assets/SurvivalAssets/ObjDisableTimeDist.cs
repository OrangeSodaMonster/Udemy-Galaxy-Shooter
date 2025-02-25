using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(DisableByDistance))]
public class ObjDisableTimeDist : MonoBehaviour
{
    [SerializeField] UnityEvent onDisable;

    QuadrantDealer quadrant;
    DisableByDistance disableByDistance;
    [SerializeField] float disableTime = 30;
    [SerializeField, Sirenix.OdinInspector.ReadOnly] float timer;
    float closeDist;
    float checkDistTimer;

    void Start()
    {
        disableByDistance = GetComponent<DisableByDistance>();
        closeDist = disableByDistance.DeSpawnRadius/disableByDistance.DistMultiplier;
    }

    private void OnEnable()
    {
        timer = 0;
        quadrant = SurvivalManager.CurrentQuadrant;
    }

    void Update()
    {
        timer += Time.deltaTime;
        checkDistTimer += Time.deltaTime;

        if (checkDistTimer < .5f) return;
        checkDistTimer = 0;

        if (timer > disableTime && disableByDistance.CheckIfTooFar(closeDist))
        {
            DisableObj();
        }

        if(quadrant != null && quadrant != SurvivalManager.CurrentQuadrant && disableByDistance.CheckIfTooFar())
        {
            DisableObj();
        }
    }

    void DisableObj()
    {
        onDisable?.Invoke();
        gameObject.SetActive(false);
    }
}
