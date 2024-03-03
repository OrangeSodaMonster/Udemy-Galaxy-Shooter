using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VibrationToogleScript : MonoBehaviour
{
    [SerializeField] Image border;
    [SerializeField] Color disabledBorderColor;
    [SerializeField] Image Image;
    [SerializeField] Color disabledImageColor;

    Color enabledBorderColor;
    Color enabledImageColor;

    private void Awake()
    {
        enabledBorderColor = border.color;
        enabledImageColor = Image.color;
    }

    private void OnEnable()
    {
        UpdateVisual();
    }

    public void ChangeVibration()
    {
        if(GameManager.IsVibration)
            GameManager.DisableVibration();
        else
            GameManager.EnableVibration();

        UpdateVisual();
    }

    public void UpdateVisual()
    {
        if (GameManager.IsVibration)
        {
            border.color = enabledBorderColor;
            Image.color = enabledImageColor;
        }
        else
        {
            border.color = disabledBorderColor;
            Image.color = disabledImageColor;
        }
    }
}