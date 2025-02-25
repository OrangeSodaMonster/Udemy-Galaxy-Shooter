using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[SelectionBase]
public class ObjectiveSpawnArrow : MonoBehaviour
{
    [SerializeField] Transform arrowPrefab;
    [SerializeField] float arrowDistanceFromPlayer;
    public UnityEvent OnClearedObjective;
    
    EnemyHPBar target;
    Transform arrow;
    Transform player;
    Vector2 direction;
    SpriteRenderer arrowSR;
    [SerializeField, ReadOnly] Color defaultArrowColor;
    Vector3 defaultArrowScale = Vector3.zero;
    float defaultAlpha = 0;
    Camera cam = new();

    void Start()
    {
        player = FindObjectOfType<PlayerMove>().transform;
        direction = (transform.position - player.position).normalized;
        target = GetComponentInChildren<EnemyHPBar>();

        cam = Camera.main;

        
        if(!GameManager.IsSurvival) // Não rodar fora de Survival
        {
            Debug.Log("NOT SURVIVAL");
            arrow = Instantiate(arrowPrefab, (Vector2)player.position + arrowDistanceFromPlayer * direction, Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, direction)));
            arrowSR = arrow.GetComponent<SpriteRenderer>();
            defaultArrowScale = arrow.transform.localScale;
            defaultAlpha = arrowSR.color.a;
            defaultArrowColor = new(target.Color.r, target.Color.g, target.Color.b, defaultAlpha);
            arrowSR.color = defaultArrowColor;
            EnterArrow();
        }        
    }

    private void OnEnable()
    {
        if (!GameManager.IsSurvival) return; // Apenas em survival

        if(arrow == null)
            arrow = arrowPrefab;
        if(defaultArrowScale == Vector3.zero)
            defaultArrowScale = arrow.transform.localScale;
        if(arrowSR == null)
            arrowSR = arrow.GetComponent<SpriteRenderer>();
        if(defaultAlpha == 0)
            defaultAlpha = arrowSR.color.a;

        target = transform.GetChild(0).GetComponent<EnemyHPBar>();
        defaultArrowColor = new(target.Color.r, target.Color.g, target.Color.b, defaultAlpha);
        arrowSR.color = defaultArrowColor;
        arrow.transform.localScale = defaultArrowScale;
        arrow.gameObject.SetActive(true);
        
        EnterArrow();
    }

    Tween fadeArrowTween = null;
    Tween growArrowTween = null;    
    Vector3 hpPosInCam = new();
    [SerializeField, ReadOnly] bool isEnterArrow;
    [SerializeField, ReadOnly] bool isNormalizeArrow;
    [SerializeField, ReadOnly] bool isExitArrow;
    [SerializeField, ReadOnly] bool isShowingArrow;
    void Update()
    {
        if (player.IsDestroyed()) return;

        direction = (transform.position - player.position).normalized;
        arrow.SetPositionAndRotation((Vector2)player.position + arrowDistanceFromPlayer * direction, Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, direction)));

        hpPosInCam = cam.WorldToViewportPoint(transform.position);
        bool isObjInScreen = hpPosInCam.x > 0 && hpPosInCam.x < 1 && hpPosInCam.y > 0 && hpPosInCam.y < 1;
        if (!isObjInScreen && !isShowingArrow)
            EnterArrow();
        else if (isObjInScreen && isShowingArrow && !isEnterArrow && !isNormalizeArrow && !isExitArrow)
            ExitArrow();

        if (!HasChildActive())
        {
            OnClearedObjective?.Invoke();
            gameObject.SetActive(false);
        }        
    }

    float tweenDuration = 0.5f;
    void EnterArrow()
    {
        //Debug.Log("Enter Arrow");
        isEnterArrow = true;
        isShowingArrow = true;
        fadeArrowTween.Kill();
        growArrowTween.Kill();
        arrow.gameObject.SetActive(true);
        arrowSR.color = Color.clear;
        arrow.transform.localScale = defaultArrowScale;
        Color maxColor = new(defaultArrowColor.r, defaultArrowColor.g, defaultArrowColor.b, 0.8f);
        fadeArrowTween = arrowSR.DOColor(maxColor, tweenDuration);
        growArrowTween = arrow.DOScale(defaultArrowScale*1.2f, tweenDuration).OnComplete(() => NormalizeArrow());        
    }

    void NormalizeArrow()
    {
        //Debug.Log("Normalize Arrow");
        isEnterArrow = false;
        isNormalizeArrow = true;
        fadeArrowTween.Kill();
        growArrowTween.Kill();
        fadeArrowTween = arrowSR.DOColor(defaultArrowColor, tweenDuration);
        growArrowTween = arrow.DOScale(defaultArrowScale, tweenDuration).OnComplete(() => isNormalizeArrow = false);
    }

    void ExitArrow()
    {
        //Debug.Log("Exit Arrow");
        isExitArrow = true;
        fadeArrowTween.Kill();
        growArrowTween.Kill();
        fadeArrowTween = arrowSR.DOFade(0f, 0.35f).OnComplete(() => ResetBools());

        void ResetBools()
        {
            isShowingArrow = false;
            isExitArrow = false;
        }
    }

    bool HasChildActive()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeInHierarchy)
            {
                return true;
            }
        }
        return false;
    }

    private void OnDisable()
    {
        fadeArrowTween.Kill();
        growArrowTween.Kill();
        if (arrow != null)
            arrow.gameObject.SetActive(false);
            //Destroy(arrow.gameObject);
    }
}