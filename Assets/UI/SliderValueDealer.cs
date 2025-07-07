using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueDealer : MonoBehaviour
{
    [SerializeField] string valueSulfix = "";
    [SerializeField] float valueMultiplier = 1;

    TextMeshProUGUI text;

    const float duration = 0.35f;
    const float fadeDuration = 0.15f;
    WaitForSecondsRealtime waitDuration = new WaitForSecondsRealtime(duration);
    WaitForSecondsRealtime waitFade = new WaitForSecondsRealtime(fadeDuration);

    [ShowInInspector] Color defaultColor;
    [ShowInInspector] Color clearColor;
    Slider slider;
    float lastSlideValue;

    private void Awake()
    {
        slider = GetComponentInParent<Slider>();
        text = GetComponent<TextMeshProUGUI>();
        defaultColor = text.color;
    }


    private void OnEnable()
    {
        clearColor = new Color(defaultColor.r, defaultColor.g, defaultColor.b, 0);
        text.color = clearColor;        

        if(slider == null) return;
        lastSlideValue = slider.value;

        //sequence = DOTween.Sequence();
        //sequence.Append(text.DOColor(defaultColor, fadeDuration).SetUpdate(true)).
        //    AppendInterval(duration).
        //    Append(text.DOColor(clearColor, fadeDuration).SetUpdate(true));
    }

    private void Update()
    {
        if(slider == null) return;
        if (lastSlideValue != slider.value)
            ShowValue();

        lastSlideValue = slider.value;
    }

    Tween myTween;
    public void ShowValue()
    {
        Debug.Log("SHOW VALUE");

        text.text = $"{slider.value * valueMultiplier}{valueSulfix}";

        if (text.color == Color.clear) text.color = clearColor;

        StartCoroutine(Waiter());

        IEnumerator Waiter()
        {
            Debug.Log("FADE IN");
            myTween = text.DOColor(defaultColor, duration).SetUpdate(true).SetAutoKill();

            yield return waitFade;
            yield return waitDuration;

            Debug.Log("FADE OUT");
            myTween = text.DOColor(clearColor, duration).SetUpdate(true).SetAutoKill();

        }
    }

    private void OnDisable()
    {
        myTween.Kill();
    }

}
