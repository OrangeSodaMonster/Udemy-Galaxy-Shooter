using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetStatsCanvasBG : MonoBehaviour
{
    [SerializeField] RectTransform backGround;
    [SerializeField] Vector2 thisSize;
    [SerializeField] int borderSize = 4;

    RectTransform thisRT;

    private void OnValidate()
    {
        thisRT = GetComponent<RectTransform>();
        thisRT.sizeDelta = thisSize;

        backGround.sizeDelta = new Vector2(thisSize.x - borderSize*2, thisSize.y - borderSize*2);
    }


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
