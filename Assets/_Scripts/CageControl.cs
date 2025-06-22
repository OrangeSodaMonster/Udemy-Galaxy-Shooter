using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CageScript))]
public class CageControl : MonoBehaviour
{
    [SerializeField] Transform energyLines;
    [SerializeField, ReadOnly] List<Transform> cageNodes;
    [SerializeField, ReadOnly] LineRenderer cageLine;
    [SerializeField, ReadOnly] GameObject colliders;
    [Space]
    [SerializeField] Material onNodeMat;
    [SerializeField] Material offNodeMat;

    EnemySpawner enemySpawner;

    private void Awake()
    {
        for (int i = 0; i < cageNodes.Count; i++)
        {
            cageNodes[i].GetComponent<ChangeMat>().OnMat = onNodeMat;
            cageNodes[i].GetComponent<ChangeMat>().OffMat = offNodeMat;
        }
    }

    [Button]
    public void CallValidate()
    {
        OnValidate();
    }

    private void OnValidate()
    {
        CageScript cageScript = GetComponent<CageScript>();

        cageNodes = cageScript.Nodes;
        cageLine = cageScript.CageLine;
        colliders = cageScript.Colliders[0].transform.parent.gameObject;
    }

    private void Start()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    [Button]
    public void TurnCageOn()
    {
        for (int i = 0; i < cageNodes.Count; i++)
        {
            cageNodes[i].GetComponent<ChangeMat>().TurnOnMat();
        }
        energyLines.gameObject.SetActive(true);
        cageLine.gameObject.SetActive(true);
        colliders.SetActive(true);
        AudioManager.Instance.CageUpSound.PlayFeedbacks();
    }

    [Button]
    public void TurnCageOff()
    {
        for (int i = 0; i < cageNodes.Count; i++)
        {
            cageNodes[i].GetComponent<ChangeMat>().TurnOffMat();
        }
        energyLines.gameObject.SetActive(false);
        cageLine.gameObject.SetActive(false);
        colliders.SetActive(false);
        AudioManager.Instance.CageDownSound.PlayFeedbacks();
    }

    public void DisableSpawn()
    {
        enemySpawner.GetComponent<RareSpawnScript>().enabled = false;
        enemySpawner.enabled = false;
    }

    public void ReEnableSpawn()
    {
        enemySpawner.enabled = true;
        enemySpawner.GetComponent<RareSpawnScript>().enabled = true;
    }

    private void OnDisable()
    {
        ReEnableSpawn();
    }
}
