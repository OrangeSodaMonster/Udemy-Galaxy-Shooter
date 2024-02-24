using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PortalScript : MonoBehaviour
{
    [SerializeField] SpriteRenderer vortexSprite;
    [SerializeField] float rotationSpeed;
    [SerializeField] float sizeVariation;
    [SerializeField] float sizeVariationDuration;
    [SerializeField] Ease sizeVariationCurve;
    [SerializeField] Color enabledColor;
    [Space]
    [SerializeField] SpriteRenderer bgSprite;
    [SerializeField] Color enableBgColor;
    [Space]
    [SerializeField] Transform arrowPrefab;
    [ColorUsage(true,true)]
    [SerializeField] Color arrowColor;
    [SerializeField] float arrowDistanceFromPlayer;

    Collider2D col;
    ObjectivePointer[] Objectives;
    int activeObjectives = 0;

    Transform arrow;
    Transform player;
    Vector2 direction;
    SpriteRenderer arrowSR;
    float defaultAlpha;
    Tween fadeArrowTween = null;

    private void Awake()
    {
        col = GetComponent<Collider2D>();

        PlayerMove pMove = FindObjectOfType<PlayerMove>();
        player = pMove.transform;
        pMove.StartPosition = transform.position;
    }

    void OnEnable()
    {
        vortexSprite.transform.DORotate(Vector3.back, rotationSpeed).SetEase(Ease.Linear).SetSpeedBased(true).SetLoops(-1, LoopType.Incremental);

        float newScale = transform.lossyScale.x + transform.lossyScale.x * sizeVariation;
        vortexSprite.transform.DOScale(newScale * Vector3.one, sizeVariationDuration).SetEase(sizeVariationCurve).SetLoops(-1, LoopType.Yoyo);

        col.enabled = false;

        Objectives = FindObjectsOfType<ObjectivePointer>();        

        // Arrow
        direction = (transform.position - player.position).normalized;

        arrow = Instantiate(arrowPrefab, (Vector2)player.position + arrowDistanceFromPlayer * direction, Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, direction)));
        arrowSR = arrow.GetComponent<SpriteRenderer>();
        defaultAlpha = arrowSR.color.a;
        arrowSR.color = arrowColor;
        arrow.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        fadeArrowTween.Kill();
        if (arrow != null)
            Destroy(arrow.gameObject);
    }

    void Update()
    {
        activeObjectives = 0;
        foreach (var obj in Objectives)
        {
            if(obj.gameObject.activeSelf)
                activeObjectives++;
        }

        if (activeObjectives == 0)
        {
            vortexSprite.color = enabledColor;
            bgSprite.color = enableBgColor;
            col.enabled = true;
            arrow.gameObject.SetActive(true);
        }

        //Arrow
        if (player.IsDestroyed() || !arrow.gameObject.activeSelf) return;

        direction = (transform.position - player.position).normalized;
        arrow.SetPositionAndRotation((Vector2)player.position + arrowDistanceFromPlayer * direction, Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, direction)));

        Vector3 posInCam = Camera.main.WorldToViewportPoint(transform.position);
        if (posInCam.x > 0 && posInCam.x < 1 && posInCam.y > 0 && posInCam.y < 1
             && !fadeArrowTween.IsActive())
            fadeArrowTween = arrowSR.DOFade(0, 0.35f);
        else if (!fadeArrowTween.IsActive())
            fadeArrowTween = arrowSR.DOFade(defaultAlpha, 0.35f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Animate Portaling
        // End fase
    }
}