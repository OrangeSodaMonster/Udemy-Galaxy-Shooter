using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockBackDealer : MonoBehaviour
{
	[SerializeField] float knockBackImunityTime = 2f;
	bool isKnockBackable = true;
	Rigidbody2D rb;
    WaitForSeconds knockBackWait;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        knockBackWait = new WaitForSeconds(knockBackImunityTime);
    }

    public void GetKnockedBack ( Vector2 velocityToAdd)
    {
        if (isKnockBackable)
        {
            rb.velocity += velocityToAdd;
            isKnockBackable = false;

            StartCoroutine(TurnKnockBackOn());
        }
    }

    IEnumerator TurnKnockBackOn()
    {
        yield return knockBackWait;

        isKnockBackable = true;
    }
}