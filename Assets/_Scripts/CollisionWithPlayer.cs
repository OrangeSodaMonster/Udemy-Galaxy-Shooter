using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionWithPlayer : MonoBehaviour
{
    public int Damage = 1;
    public float ImpactVelocity = 1;

    bool hasCollidedWithPlayer = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent(out PlayerMove playerMove) && !hasCollidedWithPlayer)
        {
            Vector3 l_forceDirection = Vector3.zero;

            if (TryGetComponent(out AsteroidMove asteroidMove))
            {
                l_forceDirection = asteroidMove.MoveDirection.normalized;
            }
            else
                l_forceDirection = playerMove.transform.position - transform.position;

            //PlayerHP.Instance.ChangePlayerHP(-Mathf.Abs(Damage));
            playerMove.GetComponent<PlayerKnockBackDealer>().GetKnockedBack(ImpactVelocity * (Vector2)(l_forceDirection).normalized); 
            hasCollidedWithPlayer = true;

            PlayerHP.Instance.OnPlayerHit(transform.position, Damage, playSound: true);
        }

        //if (collision.gameObject.GetComponent<PlayerHP>() != null)
        //{
        //    AudioManager.Instance.PlayerHitSound.PlayFeedbacks();
        //}
    }

    private void LateUpdate()
    {
        hasCollidedWithPlayer = false;
    }
}