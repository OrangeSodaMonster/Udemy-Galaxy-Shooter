using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnPoolHolder : MonoBehaviour
{
    public Transform ReturnPool;

    public void ReturnToPool()
    {
        if (ReturnPool != null)
            transform.parent = ReturnPool;
    }
}
