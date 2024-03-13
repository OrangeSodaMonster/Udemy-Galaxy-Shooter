using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.VFX;

public class ThrusterControl : MonoBehaviour
{
    enum ThrusterType
    {
        Acceleration = 1,
        TurnLeft = 2,
        TurnRight = 3,
        Reverse = 4,
    }


    [SerializeField] AnimationCurve flickCurve;
    [SerializeField] float flickTime = 3f;
    [SerializeField] float timeToMaxSize = 2;
    [Space]
    [SerializeField] ThrusterType thrusterType;
    [SerializeField] bool enlargeByUpgradeLevel = true;
    [SerializeField] float addScaleUpgradeLevel = 0.3f;

    float runTime = 0;
    Vector3 defaultScale = Vector3.one;
    bool isAccel;
    bool isReverse;
    int turnDir;
    float inputSizeMod = 0;
    Tween inputSizeModTween;
    Vector3 newScale = new();
    PlayerMove playerMove;

    VisualEffect trailVFX;
    float defaultVFXSpawnRate;

    private void Awake()
    {
        defaultScale = transform.localScale;
        trailVFX = GetComponentInChildren<VisualEffect>();
        if(thrusterType != ThrusterType.Reverse)
            defaultVFXSpawnRate = trailVFX.GetFloat("SpawnRate");
    }

    private void Start()
    {
        if (thrusterType != ThrusterType.Reverse)
            AudioManager.Instance.PlayThruster();
        else
            AudioManager.Instance.PlayReverse();

        playerMove = FindObjectOfType<PlayerMove>();
    }

    void Update()
    {
        isAccel = InputHolder.Instance.Acceleration > 0;
        isReverse = InputHolder.Instance.Acceleration < 0;
        turnDir = playerMove.GetTurningDirection() == 0 ? (int)Mathf.Sign(InputHolder.Instance.Turning) : playerMove.GetTurningDirection();
        if (InputHolder.Instance.Turning == 0 && playerMove.GetTurningDirection() == 0) turnDir = 0;

        InputSizeDealer();
        SetTrailVFX();

        float addScale = GetAddScale();

        float flickValue = runTime / flickTime;

        newScale.x = (defaultScale.x * flickCurve.Evaluate(flickValue) + addScale)  * inputSizeMod;
        newScale.y = (defaultScale.y * flickCurve.Evaluate(flickValue) + addScale) * inputSizeMod;

        transform.localScale = newScale;

        runTime += Time.deltaTime;
        if (runTime > flickTime)
            runTime -= flickTime;

        SetSoundVolumeMod();      

        //SetPlaySound();
    }

    //static void SetPlaySound()
    //{
    //    if(InputHolder.Instance.Input.Acceleration != 0 || InputHolder.Instance.Input.Turning != 0)
    //    {
    //        AudioManager.Instance.PlayThruster();
    //        AudioManager.Instance.PlayReverse();
    //    }
    //    else
    //    {
    //        //AudioManager.Instance.PauseThruster();
    //    }
    //}

    void SetSoundVolumeMod()
    {
        if (thrusterType == ThrusterType.Acceleration)
        {
            AudioManager.Instance.ThrusterAccelMod = inputSizeMod;
        }
        else if (thrusterType == ThrusterType.TurnLeft)
        {
            AudioManager.Instance.ThrusterLeftTurningMod = inputSizeMod;
        }
        else if (thrusterType == ThrusterType.TurnRight)
        {
            AudioManager.Instance.ThrusterRightTurningMod = inputSizeMod;
        }
        else if (thrusterType == ThrusterType.Reverse)
            AudioManager.Instance.SetReverseVolume(inputSizeMod);

        AudioManager.Instance.SetThrusterVolume();
    }

    private void SetTrailVFX()
    {
        if (thrusterType != ThrusterType.Reverse)
            trailVFX.SetFloat("SpawnRate", defaultVFXSpawnRate * inputSizeMod);
    }

    private float GetAddScale()
    {
        float addScale = 0;
        if (thrusterType == ThrusterType.Acceleration && enlargeByUpgradeLevel)
            addScale = (PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.SpeedLevel - 1) * addScaleUpgradeLevel;
        if ((thrusterType == ThrusterType.TurnLeft || thrusterType == ThrusterType.TurnRight) && enlargeByUpgradeLevel)
            addScale = (PlayerUpgradesManager.Instance.CurrentUpgrades.ShipUpgrades.ManobrabilityLevel - 1) * addScaleUpgradeLevel;
        return addScale;
    }

    private void InputSizeDealer()
    {
        //Debug.Log($"TurningDir = {turnDir}, Last = {turnDirLastFrame}");
        // Accel Thrust
        if (thrusterType == ThrusterType.Acceleration && isAccel && !wasAccelLastFrame)
        {
            TweenInputSize(1);
        }
        else if (thrusterType == ThrusterType.Acceleration && !isAccel && wasAccelLastFrame)
        {
            TweenInputSize(0);
        }

        //Reverse
        if (thrusterType == ThrusterType.Reverse && isReverse && !wasReverseLastFrame)
        {
            TweenInputSize(1);
        }
        else if (thrusterType == ThrusterType.Reverse && !isReverse && wasReverseLastFrame)
        {
            TweenInputSize(0);
        }

        // Turning Left
        else if (thrusterType == ThrusterType.TurnLeft && (turnDir == -1) && turnDirLastFrame != -1)
        {
            TweenInputSize(1);
            //Debug.Log("LEFT start");
        }
        else if (thrusterType == ThrusterType.TurnLeft && (turnDir != -1) && turnDirLastFrame == -1)
        {
            TweenInputSize(0);
            //Debug.Log("LEFT stop");

        }

        // Turning Right
        else if (thrusterType == ThrusterType.TurnRight && (turnDir == 1) && turnDirLastFrame != 1)
        {
            TweenInputSize(1);
            //Debug.Log("RIGHT start");

        }
        else if (thrusterType == ThrusterType.TurnRight && (turnDir != 1) && turnDirLastFrame == 1)
        {
            TweenInputSize(0);
            //Debug.Log("Right stop");

        }
    }

    private void TweenInputSize( float endValue)
    {
        inputSizeModTween.Kill();
        inputSizeModTween = DOTween.To(() => inputSizeMod, x => inputSizeMod = x, endValue, timeToMaxSize).SetSpeedBased();
    }

    bool wasAccelLastFrame;
    bool wasReverseLastFrame;
    int turnDirLastFrame;
    private void LateUpdate()
    {
        wasAccelLastFrame = isAccel;
        wasReverseLastFrame = isReverse;
        turnDirLastFrame = turnDir;
    }

    private void OnDisable()
    {
        inputSizeModTween.Kill();
    }
}
