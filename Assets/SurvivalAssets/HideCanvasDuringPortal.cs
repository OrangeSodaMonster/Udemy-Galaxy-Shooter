using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCanvasDuringPortal : MonoBehaviour
{
    Canvas canvas;

    bool wasEnabled;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    private void Start()
    {
        if(GameStatus.IsPortal)
            canvas.enabled = false;
    }

    private void Update()
    {
        if(wasEnabled) return;

        if (!GameStatus.IsPortal)
        {
            canvas.enabled = true;
            wasEnabled = true;
        }
    }

}
