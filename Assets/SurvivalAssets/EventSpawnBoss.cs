using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventSpawnBoss : MonoBehaviour
{
    const float labelWidht = 135;
    [Serializable]
    enum BossPosition
    {
        AroundPlayer = 0,
        AtBossPoint = 1,
    }

    [SerializeField, Range(0,10)] int bossIndex;
    [SerializeField] BossPosition withBonusPosition = BossPosition.AtBossPoint;
    [SerializeField] List<Transform> enableWithBonus;
    [HorizontalGroup("after"), LabelWidth(labelWidht)]
    [SerializeField] BossPosition afterBonusPosition = BossPosition.AroundPlayer;
    [HorizontalGroup("after"), LabelWidth(labelWidht)]
    [SerializeField] float waitTimeToCallAfterBonus = .5f;
    [SerializeField] List<Transform> enableAfterBonus;
    [SerializeField] bool checkSpawnsAfterBonus = true;
    [HideIf("checkSpawnsAfterBonus")]
    [SerializeField] List<Transform> checkIfDisabled;

    [FoldoutGroup("Events"), Tooltip("Number of times the event will be called")]
    [SerializeField, Range(1, 12), LabelWidth(labelWidht)]
    int timesToCallOnBonus = 1;
    [FoldoutGroup("Events")]
    [Tooltip("Called when spawn is called")]
    public UnityEvent OnBonus;
    [FoldoutGroup("Events"), Tooltip("Number of times the event will be called")]
    [SerializeField, Range(1, 12), LabelWidth(labelWidht)]
    int timesToCallAfterBonus = 1;
    [FoldoutGroup("Events")]
    [Tooltip("Called each individual spawn")]
    public UnityEvent OnAfterBonus;
    [FoldoutGroup("Events"), Tooltip("Number of times the event will be called")]
    [SerializeField, Range(1, 12), LabelWidth(labelWidht)]
    int timesToCallBossDestroyed = 1;
    [FoldoutGroup("Events")]
    [Tooltip("Called when timed spawns end")]
    public UnityEvent OnBossDestroyed;

    private void OnValidate()
    {
        foreach (Transform t in enableWithBonus)
        {
            if(enableWithBonus.Count != 0 && !t.IsChildOf(transform)) 
                enableWithBonus.Remove(t);
        }
        foreach (Transform t in enableAfterBonus)
        {
            if (enableAfterBonus.Count != 0 && !t.IsChildOf(transform))
                enableAfterBonus.Remove(t);
        }
        foreach (Transform t in checkIfDisabled)
        {
            if (checkIfDisabled.Count != 0 && !t.IsChildOf(transform))
                checkIfDisabled.Remove(t);
        }
        for(int i = 0; i < transform.parent.childCount; i++)
        {
            if(transform.parent.GetChild(i) == this.transform)
                bossIndex = i+1;
        }
    }

    private void OnEnable()
    {
        SurvivalManager.OnBonusAsteroidSpawn.AddListener(CallWithBonus);
        SurvivalManager.OnBonusAsteroidDestroyed.AddListener(CallAfterBonus);

        if (checkSpawnsAfterBonus)
            checkIfDisabled = new(enableAfterBonus);
    }

    [Button("WithBonus", ButtonSizes.Medium), PropertyOrder(-1),HorizontalGroup("Buttons"), GUIColor("lightPurple")]
    public void CallWithBonus()
    {
        if (SurvivalManager.CurrentSection + 1 != bossIndex) return;

        StartCoroutine(Waiter());

        IEnumerator Waiter()
        {
            yield return null;            

            Vector3 pos = Vector3.zero;

            for (int i = 0; i < enableWithBonus.Count; i++)
            {
                if (withBonusPosition == BossPosition.AroundPlayer)
                    pos = EnemySpawner.Instance.GetSpawnPoint360();
                else
                    pos = SurvivalManager.CurrentQuadrant.BossPoint.position;

                enableWithBonus[i].position = pos;
                enableWithBonus[i].gameObject.SetActive(true);
            }

            for (int i = 0; i < timesToCallOnBonus; i++)
                OnBonus.Invoke();
        }
    }

    [Button("AfterBonus", ButtonSizes.Medium), PropertyOrder(-1), HorizontalGroup("Buttons"), GUIColor("lightRed")]
    public void CallAfterBonus()
    {
        if (SurvivalManager.CurrentSection + 1 != bossIndex) return;
        if (SurvivalManager.SurvivalState != SurvivalState.Bonus) return;

        StartCoroutine(AfterBonusRoutine());

        IEnumerator AfterBonusRoutine()
        {
            yield return new WaitForSeconds(waitTimeToCallAfterBonus);

            Vector3 pos = Vector3.zero;            

            for (int i = 0; i < enableAfterBonus.Count; i++)
            {
                if (afterBonusPosition == BossPosition.AroundPlayer)
                    pos = EnemySpawner.Instance.GetSpawnPoint360();
                else
                    pos = SurvivalManager.CurrentQuadrant.BossPoint.position;

                enableAfterBonus[i].position = pos;
                enableAfterBonus[i].gameObject.SetActive(true);
            }

            yield return null;

            for (int i = 0; i < timesToCallAfterBonus; i++)
                OnAfterBonus.Invoke();

            SurvivalManager.SurvivalState = SurvivalState.Boss;
            Debug.Log("<color=cyan>STATE: Boss</color>");
        }
    }

    bool bossDestroyedCalled = false;
    void BossDestroyed()
    {
        if(bossDestroyedCalled) return;

        for (int i = 0; i < timesToCallBossDestroyed; i++)
            OnBossDestroyed.Invoke();

        SurvivalManager.IsWaitingEndEvent = false;
        SurvivalManager.OnBossDestroyed.Invoke();
        bossDestroyedCalled = true;
        Debug.Log($"<color=purple>Boss Destroyed</color>");
        gameObject.SetActive(false);
    }

    bool canCallOnDestroyed;
    float destroyedTimer;
    private void Update()
    {
        if(SurvivalManager.SurvivalState != SurvivalState.Boss) return;
        if(SurvivalManager.CurrentSection + 1 != bossIndex) return;

        bool allDisabled = true;
        for(int i = 0;i < checkIfDisabled.Count; i++)
        {
            if (checkIfDisabled[i].gameObject.activeSelf)
            {
                allDisabled = false;
            }
        }

        if (canCallOnDestroyed && allDisabled)
            BossDestroyed();

        if(destroyedTimer > 2)
            canCallOnDestroyed = true;

        destroyedTimer += Time.deltaTime;
    }
}
