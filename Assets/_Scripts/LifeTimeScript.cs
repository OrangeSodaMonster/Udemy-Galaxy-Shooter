using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LifeTimeScript : MonoBehaviour
{
    [SerializeField] float lifeTime;
    [SerializeField] bool disableOnDeath = true;
    [SerializeField] bool vfxOnDeath = false;

    [Header("Blinking")]
    [SerializeField] bool blinkNearDeath;
    [SerializeField] float blinkSeconds = 5;
    [SerializeField] float blinksPerSec = 2;
    [SerializeField] float fastBlinkSeconds = 2f;
    [SerializeField] float fastBlinkPerSec = 3f;
    [SerializeField] Ease ease = Ease.InOutSine;

    Tween blinkingTween;
    SpriteRenderer spriteRenderer;
    float blinkTime;
    float fastBlinkTime;
    Color defaultColor;

    WaitForSeconds lifeWait;
    WaitForSeconds blinkWait;
    WaitForSeconds fastBlinkWait;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;

        lifeWait = new WaitForSeconds(lifeTime);
        blinkWait = new WaitForSeconds(lifeTime - blinkSeconds);
        fastBlinkWait = new WaitForSeconds(blinkSeconds - fastBlinkSeconds);
    }

    void OnEnable()
    {
        spriteRenderer.color = defaultColor;
        blinkTime = 1/(blinksPerSec * 2);
        fastBlinkTime = 1/(fastBlinkPerSec * 2);

        if(lifeTime > 0)
            StartCoroutine(LifeTimeRoutine());
        if(blinkNearDeath)
            StartCoroutine(BlinkRoutine());
    }

    void OnDisable()
    {       
        StopAllCoroutines();
        blinkingTween.Kill();
    }

    IEnumerator BlinkRoutine()
    {
        yield return blinkWait;
        blinkingTween = spriteRenderer.DOFade(0, blinkTime).SetEase(ease).SetLoops(-1, LoopType.Yoyo).Play();

        yield return fastBlinkWait;
        blinkingTween.Kill();
        blinkingTween = spriteRenderer.DOFade(0, fastBlinkTime).SetEase(ease).SetLoops(-1, LoopType.Yoyo).Play();
    }    
    IEnumerator LifeTimeRoutine()
    {
        yield return lifeWait;

        if (vfxOnDeath)
        {
            GameObject vfx = VFXPoolerScript.Instance.DesapearVFXPooler.GetPooledGameObject();
            vfx.transform.position = transform.position;
            vfx.SetActive(true);
        }

        if (disableOnDeath)
            gameObject.SetActive(false);
        else
            Destroy(gameObject);
    }

}