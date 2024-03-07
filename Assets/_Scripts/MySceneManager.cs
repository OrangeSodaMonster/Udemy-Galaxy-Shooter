using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    [SerializeField] float timeToEnableFeedbacks = 0.5f;
    [field: SerializeField] public MMFeedbackLoadScene RestartScene { get; private set; }

    public static bool IsFeedbackEnabled;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SetRestartSceneName;

        StartCoroutine(ChildrenEnableRoutine());
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SetRestartSceneName;
    }
    
    private void SetRestartSceneName(Scene s1, LoadSceneMode s2)
    {
        RestartScene.DestinationSceneName = s1.name;
    }

    IEnumerator ChildrenEnableRoutine()
    {
        SetChildrenEnableStatus(false);

        yield return new WaitForSeconds(timeToEnableFeedbacks);

        SetChildrenEnableStatus(true);
    }

    void SetChildrenEnableStatus(bool status)
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(status);
        }
        IsFeedbackEnabled = status;
    }

    public void DisableFeedbacks()
    {
        SetChildrenEnableStatus(false);
    }

    public void CallGC()
    {
        System.GC.Collect();
    }
}