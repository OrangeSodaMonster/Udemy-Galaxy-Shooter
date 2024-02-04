using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        if(player.IsDestroyed()) return;

        direction = (transform.position - player.position).normalized;
        arrow.SetPositionAndRotation((Vector2)player.position + arrowDistanceFromPlayer * direction, Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, direction)));

        Vector3 hpPosInCam = Camera.main.WorldToViewportPoint(transform.position);
        if (hpPosInCam.x > 0 && hpPosInCam.x < 1 && hpPosInCam.y > 0 && hpPosInCam.y < 1
             && !fadeArrowTween.IsActive())
            fadeArrowTween = arrowSR.DOFade(0, 0.35f);
        else if (!fadeArrowTween.IsActive())
            fadeArrowTween = arrowSR.DOFade(defaultAlpha, 0.35f);

        if (!HasChildActive())
            gameObject.SetActive(false);
    }

    bool HasChildActive()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeInHierarchy)

            return true;
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