using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public struct RareSpawnsChances
{
    [HorizontalGroup("0", 0.12f), PreviewField(38, Alignment = ObjectFieldAlignment.Left), HideLabel]
    public GameObject RareSpawn;
    [VerticalGroup("0/1"), LabelWidth(10), LabelText(""), ReadOnly]
    public string Name;
    [HorizontalGroup("0/1/2"), LabelWidth(40), LabelText("P/Min")]
    [Tooltip("ChancePerMinute In %")] public float ChancePerMinute;
    [HorizontalGroup("0/1/2"), LabelWidth(40), LabelText("P/Sec")]
    [ReadOnly] public float ChancePerSecond;
}

public class RareSpawnScript : MonoBehaviour
{   
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
    static public RareSpawnScript Instance;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    void Start()
    {
        poolRefs = FindObjectOfType<PoolRefs>();

        if (!GameManager.IsSurvival)
        {
            for (int i = 0; i < RareSpawns.Length; i++)
            {
                poolRefs.CreatePoolsForObject(RareSpawns[i].RareSpawn, 1);
            }
        }            

        SetChances();

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

    void SetChances()
    {
        for (int i = 0; i < RareSpawns.Length; i++)
        {
            RareSpawns[i].ChancePerSecond = RareSpawns[i].ChancePerMinute / 60;
        }
    }

    void OnEnable()
    {
        StartCoroutine(RareSpawnerRoutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    void Update()
    {
        HighlightDealer();
    }

    IEnumerator RareSpawnerRoutine()
    {
        if(intervalToEnableAtBegining > 0)
            yield return new WaitForSeconds(intervalToEnableAtBegining);

        while(true)
        {
            if(RareSpawns.Length > 0)
            {
                //foreach (var rare in RareSpawns)
                for (int i = 0; i < RareSpawns.Length; i++)
                {
                    if (CheckSpawn(RareSpawns[i]))
                    {
                        SpawnRare(RareSpawns[i].RareSpawn);

                        yield return waitIntervalMinus1;
                    }
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

    public void SetRareSpawns(RareSpawnsChances[] rareSpawns)
    {
        intervalToEnableAtBegining = 0;
        RareSpawns = rareSpawns;
        SetChances();
    }

    private void OnValidate()
    {
        SetChances();
        for (int i = 0; i < RareSpawns.Length; i++)
        {
            if (RareSpawns[i].RareSpawn != null)
            {
                RareSpawns[i].Name = RareSpawns[i].RareSpawn.name;
            }
        }
    }
}