using DG.Tweening;
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
    float defaultAlpha;
    Camera cam = new();

    void OnEnable()
    {
        player = FindObjectOfType<PlayerMove>().transform;
        direction = (transform.position - player.position).normalized;
        target = GetComponentInChildren<EnemyHPBar>();

        arrow = Instantiate(arrowPrefab, (Vector2)player.position + arrowDistanceFromPlayer * direction, Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, direction)));
        arrowSR = arrow.GetComponent<SpriteRenderer>();
        defaultAlpha = arrowSR.color.a;
        arrowSR.color = new(target.Color.r, target.Color.g, target.Color.b, defaultAlpha);

        cam = Camera.main;
    }

    Tween fadeArrowTween = null;
    Vector3 hpPosInCam = new();
    void Update()
    {
        if(player.IsDestroyed()) return;

        direction = (transform.position - player.position).normalized;
        arrow.SetPositionAndRotation((Vector2)player.position + arrowDistanceFromPlayer * direction, Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, direction)));

        hpPosInCam = cam.WorldToViewportPoint(transform.position);
        if (hpPosInCam.x > 0 && hpPosInCam.x < 1 && hpPosInCam.y > 0 && hpPosInCam.y < 1
             && !fadeArrowTween.IsActive())
            fadeArrowTween = arrowSR.DOFade(0, 0.35f);
        else if (!fadeArrowTween.IsActive())
            fadeArrowTween = arrowSR.DOFade(defaultAlpha, 0.35f);

        if (!HasChildActive())
        {
            OnClearedObjective?.Invoke();
            gameObject.SetActive(false);
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
        if (arrow != null)
            Destroy(arrow.gameObject);
    }
}