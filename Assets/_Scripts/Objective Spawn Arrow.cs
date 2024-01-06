using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveSpawnArrow : MonoBehaviour
{
    [SerializeField] Transform arrowPrefab;
    [SerializeField] float arrowDistanceFromPlayer;
    [SerializeField] EnemyHPBar hpBar;

    Transform arrow;
    Transform player;
    Vector2 direction;
    SpriteRenderer arrowSR;
    float defaultAlpha;

    void Start()
    {
        player = FindObjectOfType<PlayerMove>().transform;
        direction = (transform.position - player.position).normalized;

        arrow = Instantiate(arrowPrefab, (Vector2)player.position + arrowDistanceFromPlayer * direction, Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, direction)));
        arrowSR = arrow.GetComponent<SpriteRenderer>();
        defaultAlpha = arrowSR.color.a;
        arrowSR.color = new(hpBar.Color.r, hpBar.Color.g, hpBar.Color.b, defaultAlpha);
    }

    Tween fadeArrowTween = null;
    void Update()
    {
        direction = (transform.position - player.position).normalized;
        arrow.SetPositionAndRotation((Vector2)player.position + arrowDistanceFromPlayer * direction, Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, direction)));

        Vector3 hpPosInCam = Camera.main.WorldToViewportPoint(transform.position);
        if (hpPosInCam.x > 0 && hpPosInCam.x < 1 && hpPosInCam.y > 0 && hpPosInCam.y < 1
             && !fadeArrowTween.IsActive())
            fadeArrowTween = arrowSR.DOFade(0, 0.35f);
        else if (!fadeArrowTween.IsActive())
            fadeArrowTween = arrowSR.DOFade(defaultAlpha, 0.35f);

        if (transform.childCount == 0)
            Destroy(gameObject);
    }

    private void OnDisable()
    {
        Destroy(arrow.gameObject);
    }
}