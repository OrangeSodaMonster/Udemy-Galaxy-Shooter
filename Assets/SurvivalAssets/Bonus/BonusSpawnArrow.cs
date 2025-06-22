using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class BonusSpawnArrow : MonoBehaviour
{
    [SerializeField] Transform arrowPrefab;
    [SerializeField] float arrowDistanceFromPlayer;

    Transform arrow;
    Transform player;
    Vector2 direction;
    SpriteRenderer arrowSR;
    [SerializeField, ReadOnly] Color defaultArrowColor;
    Vector3 defaultArrowScale = Vector3.zero;
    Camera cam = new();

    void Start()
    {
        player = FindObjectOfType<PlayerMove>().transform;
        direction = (transform.position - player.position).normalized;        

        cam = Camera.main;

        //arrow = Instantiate(arrowPrefab, (Vector2)player.position + arrowDistanceFromPlayer * direction, Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, direction)));
        arrow = BonusArrowRef.Instance.GetArrow();
        arrowSR = arrow.GetComponent<SpriteRenderer>();
        defaultArrowScale = arrowPrefab.GetComponent<SpriteRenderer>().transform.localScale;
        defaultArrowColor = arrowPrefab.GetComponent<SpriteRenderer>().color;
        EnterArrow();
        
    }

    Tween fadeArrowTween = null;
    Tween growArrowTween = null;
    Vector3 posInCam = new();
    [SerializeField, ReadOnly] bool isEnterArrow;
    [SerializeField, ReadOnly] bool isNormalizeArrow;
    [SerializeField, ReadOnly] bool isExitArrow;
    [SerializeField, ReadOnly] bool isShowingArrow;
    void Update()
    {
        if (player.IsDestroyed()) return;

        direction = (transform.position - player.position).normalized;
        arrow.SetPositionAndRotation((Vector2)player.position + arrowDistanceFromPlayer * direction,
            Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, direction)));

        posInCam = cam.WorldToViewportPoint(transform.position);
        bool isObjInScreen = posInCam.x > 0 && posInCam.x < 1 && posInCam.y > 0 && posInCam.y < 1;
        if (!isObjInScreen && !isShowingArrow)
            EnterArrow();
        else if (isObjInScreen && isShowingArrow && !isEnterArrow && !isNormalizeArrow && !isExitArrow)
            ExitArrow();
    }

    float tweenDuration = 0.5f;
    void EnterArrow()
    {
        //Debug.Log("ENTER BONUS ARROW");
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

    private void OnDisable()
    {
        fadeArrowTween.Kill();
        growArrowTween.Kill();
        if (arrow != null)
            arrow.gameObject.SetActive(false);
        //Destroy(arrow.gameObject);
    }
}
