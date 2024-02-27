using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

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
    [Header("Arrival/Exit")]
    [SerializeField] float arrivalDuration = 1.5f;
    [SerializeField] int arrivalRotationNum = 2;
    [SerializeField] Ease arrivalEase = Ease.InExpo;
    [SerializeField] float delayToPlayArrivalSound = 0.4f;
    [SerializeField] VisualEffect arrivalVFX;
    [SerializeField] float exitDuration = 3f;
    [SerializeField] float exitRotationNum = 4;
    [SerializeField] Ease exitEase = Ease.InExpo;
    [SerializeField] float delayToPlayExitSound = 0.4f;
    [SerializeField] VisualEffect exitVFX;
    [SerializeField] float timeToCallOnExit = 1.5f;
    [SerializeField] UnityEvent onExit;
    [SerializeField] UnityEvent onExitImediate;


    Collider2D col;
    ObjectivePointer[] Objectives;
    int activeObjectives = 0;

    Transform arrow;
    Transform player;
    Vector2 direction;
    SpriteRenderer arrowSR;
    float defaultAlpha;
    Tween fadeArrowTween = null;
    PlayModeUI playModeUI;

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
        playModeUI = FindObjectOfType<PlayModeUI>();
        GameStatus.IsPortal = true;

        // Player Animation
        StartCoroutine(PlayerArrivalAnimation());

        // Arrow
        direction = (transform.position - player.position).normalized;

        arrow = Instantiate(arrowPrefab, (Vector2)player.position + arrowDistanceFromPlayer * direction, Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, direction)));
        arrowSR = arrow.GetComponent<SpriteRenderer>();
        arrowSR.color = arrowColor;
        defaultAlpha = arrowSR.color.a;
        arrow.gameObject.SetActive(false);
    }

    IEnumerator PlayerArrivalAnimation()
    {
        yield return null;

        Invoke(nameof(PlayArrivalSound), delayToPlayArrivalSound);
        DisablePlayerCommands.Instance.SetCommands(false);
        Vector3 playerDefaultScale = player.localScale;
        player.localScale = Vector3.zero;
        player.DORotate(360 * arrivalRotationNum * Vector3.back, arrivalDuration, RotateMode.FastBeyond360).SetEase(arrivalEase);
        player.DOScale(playerDefaultScale, arrivalDuration).SetEase(arrivalEase).OnComplete(() => OnCompleteArrival());
    }

    void OnCompleteArrival()
    {
        DisablePlayerCommands.Instance.SetCommands(true);
        arrivalVFX.gameObject.SetActive(true);
        playModeUI.EnableUI();
        GameStatus.IsPortal = false;
    }

    void PlayArrivalSound()
    {
        AudioManager.Instance.PortalArrivalSound.PlayFeedbacks();
    }

    void PlayerExitDealer()
    {
        DisablePlayerCommands.Instance.SetCommands(false);
        DisablePlayerCommands.Instance.SetColliders(false);
        DisablePlayerCommands.Instance.SetRB(false);
        GameStatus.ClearStage();
        Invoke(nameof(PlayExitSound), delayToPlayExitSound);

        player.DOMove(transform.position, exitDuration).SetEase(Ease.OutSine);
        player.DORotate(360 * exitRotationNum * Vector3.back, exitDuration, RotateMode.FastBeyond360).SetEase(exitEase);
        player.DOScale(Vector3.zero, exitDuration).SetEase(exitEase).OnComplete(() => OnCompleteExit());
        GameStatus.IsPortal = true;
    }

    void OnCompleteExit()
    {
        StartCoroutine(CallOnExit());
        exitVFX.gameObject.SetActive(true);
    }

    void PlayExitSound()
    {
        AudioManager.Instance.PortalExitSound.PlayFeedbacks();
    }

    IEnumerator CallOnExit()
    {
        player.gameObject.SetActive(false);
        onExitImediate?.Invoke();

        yield return new WaitForSeconds(timeToCallOnExit);

        onExit?.Invoke();
    }

    private void OnDisable()
    {
        fadeArrowTween.Kill();
        if (arrow != null)
            Destroy(arrow.gameObject);

        vortexSprite.transform.DOKill();
    }

    void Update()
    {
        activeObjectives = 0;
        foreach (var obj in Objectives)
        {
            if(obj.gameObject.activeSelf)
                activeObjectives++;
        }

        Vector3 posInCam = Camera.main.WorldToViewportPoint(transform.position);
        if (activeObjectives == 0)
        {
            vortexSprite.color = enabledColor;
            bgSprite.color = enableBgColor;
            col.enabled = true;
            if(!(posInCam.x > 0 && posInCam.x < 1 && posInCam.y > 0 && posInCam.y < 1))
                arrow.gameObject.SetActive(true);
        }

        //Arrow
        if (player.IsDestroyed() || !arrow.gameObject.activeSelf) return;

        direction = (transform.position - player.position).normalized;
        arrow.SetPositionAndRotation((Vector2)player.position + arrowDistanceFromPlayer * direction, Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, direction)));
        
        if (posInCam.x > 0 && posInCam.x < 1 && posInCam.y > 0 && posInCam.y < 1 && !fadeArrowTween.IsActive())
            fadeArrowTween = arrowSR.DOFade(0, 0.35f);
        else if (!fadeArrowTween.IsActive())
            fadeArrowTween = arrowSR.DOFade(defaultAlpha, 0.35f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerExitDealer();
    }
}