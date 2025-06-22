using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range2D_Check : MonoBehaviour
{
    [SerializeField] bool dontDraw;
    [SerializeField] bool drawOnSelected = true;
    [SerializeField] float radius = 1;

    private void OnDrawGizmos()
    {
        //if(dontDraw || drawOnSelected) return;

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(this.transform.position, radius);
    }

    private void OnDrawGizmosSelected()
    {

        //if (dontDraw || !drawOnSelected) return;

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(this.transform.position, radius);
    }
}
