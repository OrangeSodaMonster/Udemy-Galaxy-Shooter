using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class RareSpawnScript : MonoBehaviour
{
    [Serializable]
    struct RareSpawnsChances
    {
        public GameObject RareSpawn;
        [Tooltip("In %")]
        public float ChancePerMinute;
        public float ChancePerSecond;
    }

    [SerializeField] RareSpawnsChances[] RareSpawns;
    [SerializeField] float intervalToEnableAtBegining = 10f;
    [SerializeField] float intervalBetweenSpawns = 5f;
    [SerializeField] bool playSound = true;
    [SerializeField] GameObject highlightPref;
    [Header("Arrow")]
    [SerializeField] bool showArrow = true;
    [SerializeField] GameObject arrowPref ;
    [SerializeField] float arrowDistanceFromPlayer = 3.2f;
    [SerializeField] float arrowDuration = 1f;

    PoolRefs poolRefs;
    Dictionary<GameObject, GameObject> rareDict = new();
    Transform arrow;
    SpriteRenderer arrowRenderer;
    Color arrowDefaultColor;
    Transform player;
    Transform rareSpawn;
    Transform highlight;
    Vector3 highlightDefaultScale = new();
    WaitForSeconds wait1 = new WaitForSeconds(1);
    WaitForSeconds waitIntervalMinus1;

    void Start()
    {
        poolRefs = FindObjectOfType<PoolRefs>();

        for (int i = 0; i < RareSpawns.Length; i++)
        {
            //rareDict.Add(RareSpawns[i].RareSpawn, Instantiate(RareSpawns[i].RareSpawn, transform));
            //rareDict[RareSpawns[i].RareSpawn].SetActive(false);
            poolRefs.CreatePoolsForObject(RareSpawns[i].RareSpawn, 1);

            RareSpawns[i].ChancePerSecond = RareSpawns[i].ChancePerMinute / 60;
        }

        player = FindObjectOfType<PlayerMove>().transform;
        arrow = Instantiate(arrowPref, transform).transform;
        arrowRenderer = arrow.GetComponent<SpriteRenderer>();
        arrowDefaultColor = arrowRenderer.color;
        arrow.gameObject.SetActive(false);
        highlight = Instantiate(highlightPref,transform).transform;
        highlightDefaultScale = highlight.localScale;
        highlight.gameObject.SetActive(false);

        waitIntervalMinus1 = new WaitForSeconds(intervalBetweenSpawns - 1);
    }

    void OnEnable()
    {
        StartCoroutine(rareSpawnerRoutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    void Update()
    {
        HighlightDealer();
    }

    IEnumerator rareSpawnerRoutine()
    {
        yield return new WaitForSeconds(intervalToEnableAtBegining);

        while(true)
        {
            foreach (var rare in RareSpawns)
            {
                if (CheckSpawn(rare))
                {
                    //SpawnRare(rareDict[rare.RareSpawn]);                    
                    SpawnRare(rare.RareSpawn);                    

                    yield return waitIntervalMinus1;
                }
            }

            yield return wait1;
        }
    }

    bool CheckSpawn(RareSpawnsChances rareSpawn)
    {
        float randonValue = UnityEngine.Random.Range(0f , 100f);

        if(randonValue < rareSpawn.ChancePerSecond)
        {
            Debug.Log($"Spawn >>> {randonValue}");
            return true;
        }
        else return false;
    }

    [Button]
    public void SpawnRare(int i)
    {
        GameObject rare = RareSpawns[i].RareSpawn;

        GameObject spawn = poolRefs.Poolers[rare].GetPooledGameObject();

        spawn.transform.position = EnemySpawner.Instance.GetSpawnPointAheadOfPlayer();
        spawn.transform.rotation = Quaternion.identity;
        spawn.SetActive(true);
        rareSpawn = spawn.transform;

        SetHighlight();

        if (playSound)
            AudioManager.Instance.RareSpawnSound.PlayFeedbacks();
        if (showArrow)
            ShowArrow();
    }

    // Does not use pool, send already pooled obj    
    public void SpawnRare(GameObject rare)
    {
        GameObject spawn = poolRefs.Poolers[rare].GetPooledGameObject();

        spawn.transform.position = EnemySpawner.Instance.GetSpawnPointAheadOfPlayer();
        spawn.transform.rotation = Quaternion.identity;
        spawn.SetActive(true);
        rareSpawn = spawn.transform;

        SetHighlight();

        if (playSound)
            AudioManager.Instance.RareSpawnSound.PlayFeedbacks();
        if(showArrow)
            ShowArrow();
    }    

    void ShowArrow()
    {
        arrow.gameObject.SetActive(true);
        arrowRenderer.color = Color.clear;
        PositionArrow();
        arrowRenderer.DOColor(arrowDefaultColor, (arrowDuration * 0.35f)).OnUpdate(() => PositionArrow()).OnComplete(() => HideArrow());
    }
    void HideArrow()
    {
        arrowRenderer.DOColor(Color.clear, (arrowDuration * (arrowDuration - 0.35f))).OnUpdate(() => PositionArrow()).OnComplete(() => arrow.gameObject.SetActive(false));
    }
    Vector2 direction = new();
    void PositionArrow()
    {
        if(!rareSpawn.gameObject.activeInHierarchy || GameStatus.IsGameover || player == null)
        {
            arrowRenderer.DOKill();
            arrow.gameObject.SetActive(false);
            return;
        }

        direction = (rareSpawn.position - player.position).normalized;
        arrow.SetPositionAndRotation((Vector2)player.position + arrowDistanceFromPlayer * direction,
            Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, direction)));
    }

    void SetHighlight()
    {
        highlight.gameObject.SetActive(true);
        highlight.localScale = Vector3.Scale(highlightDefaultScale, rareSpawn.lossyScale);
    }

    void HighlightDealer()
    {
        if (rareSpawn != null && rareSpawn.gameObject.activeInHierarchy)
        {            
            highlight.position = rareSpawn.position;
        }
        else
        {
            highlight.gameObject.SetActive(false);
        }
    }
}