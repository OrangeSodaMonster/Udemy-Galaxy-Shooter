using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ThrusterControl : MonoBehaviour
{
    [SerializeField] AnimationCurve flickCurve;
    [SerializeField] float flickTime = 3f;
    [SerializeField] float timeToMaxSize = 2;
    [Space]
    [SerializeField] bool isTurningThrust;
    [SerializeField] bool isTurnLeftThrust;
    [SerializeField] bool enlargeByUpgradeLevel = true;
    [SerializeField] float addScaleUpgradeLevel = 0.3f;

    float runTime = 0;
    Vector3 defaultScale = Vector3.one;
    InputSO input;
    bool isAccel;
    int turnDir;
    float inputSizeMod = 0;
    Tween inputSizeModTween;
    Vector3 newScale = new();

    private void Awake()
    {
        defaultScale = transform.localScale;
    }

    private void Start()
    {
        input = InputHolder.Instance.Input;
    }

    void Update()
    {
        isAccel = input.Acceleration > 0;
        turnDir = (int)Mathf.Sign(input.Turning);
        if(input.Turning == 0) turnDir = 0;

        // Accel Thrust
        if(!isTurningThrust && isAccel && !wasAccelLastFrame)
        {
            TweenInputSize(1);
        }
        else if(!isTurningThrust && !isAccel && wasAccelLastFrame)
        {
            TweenInputSize(0);
        }

        // Turning Left
        else if (isTurningThrust && isTurnLeftThrust && turnDir == -1 && turnDirLastFrame != -1)
        {
            TweenInputSize(1);
        }
        else if (isTurningThrust && isTurnLeftThrust && turnDir != -1 && turnDirLastFrame == -1)
        {
            TweenInputSize(0);
        }

        // Turning Right
        else if (isTurningThrust && !isTurnLeftThrust && turnDir == 1 && turnDirLastFrame != 1)
        {
            TweenInputSize(1);
        }
        else if (isTurningThrust && !isTurnLeftThrust && turnDir != 1 && turnDirLastFrame == 1)
        {
            TweenInputSize(0);
        }

        float addScale = 0;
        if (!isTurningThrust && enlargeByUpgradeLevel)
            addScale = (PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.SpeedLevel - 1) * addScaleUpgradeLevel;
        if (isTurningThrust && enlargeByUpgradeLevel)
            addScale = (PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.ManobrabilityLevel - 1) * addScaleUpgradeLevel;

        Debug.Log($"{name}: {addScale}");

        float flickValue = runTime / flickTime;

        newScale.x = (defaultScale.x * flickCurve.Evaluate(flickValue) + addScale)  * inputSizeMod;
        newScale.y = (defaultScale.y * flickCurve.Evaluate(flickValue) + addScale) * inputSizeMod;

        transform.localScale = newScale;

        runTime += Time.deltaTime;
        if (runTime > flickTime)
            runTime -= flickTime;
    }

    private void TweenInputSize( float endValue)
    {
        inputSizeModTween.Kill();
        inputSizeModTween = DOTween.To(() => inputSizeMod, x => inputSizeMod = x, endValue, timeToMaxSize).SetSpeedBased();
    }

    bool wasAccelLastFrame;
    int turnDirLastFrame;
    private void LateUpdate()
    {
        wasAccelLastFrame = isAccel;
        turnDirLastFrame = turnDir;
    }

    private void OnDisable()
    {
        inputSizeModTween.Kill();
    }

}