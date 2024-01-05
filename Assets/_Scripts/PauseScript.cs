using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] CanvasScaler scaler;
    [SerializeField] GraphicRaycaster raycaster;
    [SerializeField] InputSO input;

    public static bool isPaused;
    bool hasReleasedPause;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        scaler = GetComponent<CanvasScaler>();
        raycaster = GetComponent<GraphicRaycaster>();
    }

    void Update()
    {
        if (hasReleasedPause && input.IsPausing && !isPaused)
        {
            canvas.enabled = true;
            scaler.enabled = true;
            raycaster.enabled = true;
            isPaused = true;
            Time.timeScale = 0;
            hasReleasedPause = false;
        }
        else if (hasReleasedPause && input.IsPausing && isPaused)
        {
            canvas.enabled = false;
            scaler.enabled = false;
            raycaster.enabled = false;
            isPaused = false;
            Time.timeScale = 1;
            hasReleasedPause = false;
        }

        if (!input.IsPausing)
            hasReleasedPause = true;
    }
}