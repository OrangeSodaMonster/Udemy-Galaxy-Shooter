using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleHorizon : MonoBehaviour
{
    [SerializeField] float timeToPull = 0.2f;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (TryGetComponent(out Rigidbody2D rb))
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
        }

        //if (TryGetComponent(out PlayerMove pMove))
        //    pMove.enabled = false;
        //else if (TryGetComponent(out LaserMove lMove))
        //    lMove.enabled = false;
        //else if (TryGetComponent(out AsteroidMove aMove))
        //    aMove.enabled = false;
        //else if (TryGetComponent(out DroneMove dMove))
        //    dMove.enabled = false;
        //else if (TryGetComponent(out EnemyShipMove eMove))
        //    eMove.enabled = false;

        Sequence shrinkSeq = DOTween.Sequence();
        shrinkSeq.Append(collision.transform.DOMove(transform.position, timeToPull).SetEase(Ease.OutQuad));
        shrinkSeq.Join(collision.transform.DOScale(0, timeToPull).SetEase(Ease.OutQuad));

        shrinkSeq.Play().OnComplete(() => DestroySucked(collision));
    }

    void DestroySucked(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerHP playerHP))
            StartCoroutine(playerHP.PlayerDestructionSequence());
        else if(collision.TryGetComponent(out LaserMove lMove))
            lMove.DestroySilently();
        else if (collision.TryGetComponent(out CollectiblesPickUps drop))
            drop.DestroySequence();
        else if (collision.TryGetComponent(out EnemyHP enemyHP))
            enemyHP.DestroySequence();
        else
            collision.gameObject.SetActive(false);
    }
}

