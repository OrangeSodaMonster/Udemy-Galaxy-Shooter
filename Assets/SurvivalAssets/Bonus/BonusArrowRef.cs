using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusArrowRef : MonoBehaviour
{
    [SerializeField] List<Transform> arrows = new List<Transform>();

    static public BonusArrowRef Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public Transform GetArrow()
    {
        for(int i = 0; i < arrows.Count; i++)
        {
            if (!arrows[i].gameObject.activeSelf)
                return arrows[i];
        }
        return arrows[0];
    }
}
