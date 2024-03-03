using DG.Tweening;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [field:Header("PlayerSkills")]
    [field:SerializeField] public MMFeedbacks LaserSound { get; private set; }
    [field:SerializeField] public MMFeedbacks IonStreamSound { get; private set; }
    [field:SerializeField] public AudioSource DronesSound { get; private set; }
    [field:SerializeField] public MMFeedbacks BombSound { get; private set; }
    [field:SerializeField] public MMFeedbacks ShieldUpSound { get; private set; }
    [field:SerializeField] public MMFeedbacks ShipFix { get; private set; }
    [field:SerializeField] public AudioSource ThrusterSound { get; private set; }
    [field:SerializeField] public AudioSource ReverseSound { get; private set; }

    [field: Header("Collectibles")]
    [field:SerializeField] public MMFeedbacks MetalCrumbSound { get; private set; }
    [field:SerializeField] public MMFeedbacks RareMetalCrumbSound { get; private set; }
    [field:SerializeField] public MMFeedbacks EnergyCrystalSound { get; private set; }
    [field:SerializeField] public MMFeedbacks CondensedEnergyCrystalSound { get; private set; }

    [field: Header("PowerUps")]
    [field: SerializeField] public MMFeedbacks HealPUSound { get; private set; }
    [field: SerializeField] public MMFeedbacks FasterShootingPUSound { get; private set; }
    [field: SerializeField] public MMFeedbacks ShieldPUSound { get; private set; }
    [field: SerializeField] public MMFeedbacks TractorBeamPUSound { get; private set; }
    [field: SerializeField] public MMFeedbacks BombPickSound { get; private set; }
    [field: SerializeField] public MMFeedbacks EndPUSound { get; private set; }

    [field: Header("Impact")]
    [field: SerializeField] public MMFeedbacks AsteroidHitSound { get; private set; }
    [field: SerializeField] public MMFeedbacks EnemyHitSound { get; private set; }
    [field: SerializeField] public MMFeedbacks PlayerHitSound { get; private set; }
    [field: SerializeField] public MMFeedbacks ShieldHitSound { get; private set; }

    [field: Header("Destruction")]
    [field: SerializeField] public MMFeedbacks EnemyDestructionSound { get; private set; }
    [field: SerializeField] public MMFeedbacks EnemyProjectileDestructionSound { get; private set; }
    [field: SerializeField] public MMFeedbacks AsteroidDestructionSound { get; private set; }
    [field: SerializeField] public MMFeedbacks PlayerDestructionSound { get; private set; }

    [field: Header("Enemy Fire")]
    [field: SerializeField] public MMFeedbacks EnemyChargeSound { get; private set; }
    [field: SerializeField] public MMFeedbacks EnemyFireSound { get; private set; }
    [field: SerializeField] public MMFeedbacks DroneSpawnSound { get; private set; }
    [field: SerializeField] public AudioSource SentinelBeamSound { get; private set; }

    [field: Header("Interface")]
    [field: SerializeField] public MMFeedbacks SelectionClickSound { get; private set; }
    [field: SerializeField] public MMFeedbacks ConfirmationClickSound { get; private set; }
    [field: SerializeField] public MMFeedbacks HoverSound { get; private set; }
    [field: SerializeField] public MMFeedbacks BackSound { get; private set; }
    [field: SerializeField] public MMFeedbacks UnlockUpgradeSound { get; private set; }
    [field: SerializeField] public MMFeedbacks UpgradeSound { get; private set; }
    [field: SerializeField] public MMFeedbacks UpgradeFailSound { get; private set; }

    [field: Header("Other")]
    [field: SerializeField] public AudioSource AlarmSound { get; private set; }
    [field: SerializeField] public MMFeedbacks RareSpawnSound { get; private set; }
    [field: SerializeField] public MMFeedbacks PortalArrivalSound { get; private set; }
    [field: SerializeField] public MMFeedbacks PortalExitSound { get; private set; }

    float dronesDefaultVolume;
    float alarmDefaultVolume;
    float thrusterDefaultVolume;
    float reverseDefaultVolume;
    float sentinelDefaultVolume;
    MMFeedbackMMSoundManagerSound enemyChargeFB;
    MMFeedbackMMSoundManagerSound asteroidDestructionFB;
    float asteroidDestructionDefaultVolume;

    public static AudioManager Instance;
    void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void OnEnable()
    {
        transform.parent = null;
        DontDestroyOnLoad(this);

        PauseAllLoops();

        GameStatus.GameOver += PauseAllLoops;
        GameStatus.StageCleared += PauseAllLoops;
    }
    private void OnDisable()
    {
        GameStatus.GameOver -= PauseAllLoops;
        GameStatus.StageCleared -= PauseAllLoops;
    }

    private void Start()
    {
        dronesDefaultVolume = DronesSound.volume;
        alarmDefaultVolume = AlarmSound.volume;
        thrusterDefaultVolume = ThrusterSound.volume;
        reverseDefaultVolume = ReverseSound.volume;
        sentinelDefaultVolume = SentinelBeamSound.volume;

        enemyChargeFB = EnemyChargeSound.GetComponent<MMFeedbackMMSoundManagerSound>();
        asteroidDestructionFB = AsteroidDestructionSound.GetComponent<MMFeedbackMMSoundManagerSound>();
        asteroidDestructionDefaultVolume = asteroidDestructionFB.MinVolume;
    }

    bool playedDrones;
    public void PauseDrone()
    {
        if (!playedDrones) return;

        DronesSound.DOFade(0, .1f).OnComplete(() => DronesSound.Pause());
        playedDrones = false;
    }
    public void PlayDrone()
    {
        if (playedDrones) return;

        DronesSound.volume = 0;
        DronesSound.Play();
        DronesSound.DOFade(dronesDefaultVolume, .3f);
        playedDrones = true;
    }
    public void SetDroneVolume(int activeNum)
    {
        DronesSound.volume = dronesDefaultVolume * activeNum - dronesDefaultVolume * 0.5f * (activeNum - 1);
    }

    bool playedThruster;
    public void PauseThruster()
    {
        if (!playedThruster) return;

        ThrusterSound.DOFade(0, .2f).OnComplete(() => ThrusterSound.Pause());
        playedThruster = false;
    }
    public void PlayThruster()
    {
        if (playedThruster) return;

        ThrusterSound.Play();
        playedThruster = true;
    }

    [HideInInspector] public float ThrusterAccelMod = 0;
    [HideInInspector] public float ThrusterLeftTurningMod = 0;
    [HideInInspector] public float ThrusterRightTurningMod = 0;
    public void SetThrusterVolume()
    {
        float turningWeight = 0.5f;
        float volumeMod = Mathf.Clamp((ThrusterAccelMod + ThrusterLeftTurningMod * turningWeight + ThrusterRightTurningMod * turningWeight), 0, 1.25f);

        ThrusterSound.volume = thrusterDefaultVolume * volumeMod;
    }

    bool playedReverse;
    public void PauseReverse()
    {
        if (!playedReverse) return;

        ReverseSound.DOFade(0, .1f).OnComplete(() => DronesSound.Pause());
        playedReverse = false;
    }
    public void PlayReverse()
    {
        if (playedReverse) return;

        ReverseSound.Play();
        playedReverse = true;
    }

    public void SetReverseVolume(float volumeMod)
    {
        ReverseSound.volume = reverseDefaultVolume * volumeMod;
    }

    bool playedAlarm;
    public void PauseAlarm()
    {
        if (!playedAlarm) return;

        AlarmSound.DOFade(0, .3f).OnComplete(() => AlarmSound.Pause());
        playedAlarm = false;
    }
    public void PlayAlarm()
    {
        if (playedAlarm) return;

        AlarmSound.volume = 0;
        AlarmSound.Play();
        AlarmSound.DOFade(alarmDefaultVolume, 1f);
        playedAlarm = true;
    }

    bool playedSentinel;
    public void PauseSentinel()
    {
        if (!playedSentinel) return;

        SentinelBeamSound.DOFade(0, .1f).OnComplete(() => SentinelBeamSound.Pause());
        playedSentinel = false;
    }
    public void PlaySentinel()
    {
        if (playedSentinel) return;

        SentinelBeamSound.volume = 0;
        SentinelBeamSound.Play();
        SentinelBeamSound.DOFade(sentinelDefaultVolume, .3f);
        playedSentinel = true;
    }

    public void PauseAllLoops()
    {
        PauseAlarm();
        PauseDrone();
        PauseReverse();
        PauseThruster();
        PauseSentinel();

        Debug.Log("Pause All");
    }

    public void PlayEnemyCharge(int enemyHash)
    {
        enemyChargeFB.ID = enemyHash;
        EnemyChargeSound.PlayFeedbacks();
    }
    public void StopEnemyCharge(int enemyHash)
    {
        MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Free, enemyHash);
    }

    public void PlayAsteroidSound (float volumeMultiplier = 1)
    {
        asteroidDestructionFB.MinVolume = asteroidDestructionDefaultVolume * volumeMultiplier;
        float maxVolume = asteroidDestructionDefaultVolume * volumeMultiplier + Random.Range(0f, 0.05f);
        asteroidDestructionFB.MaxVolume = maxVolume;

        AsteroidDestructionSound.PlayFeedbacks();
    }

    bool canPlayLaser = true;
    public void PlayLaserSound(float laserSoundInterval)
    {
        if (!canPlayLaser) return;

        LaserSound.PlayFeedbacks();
        canPlayLaser = false;
        StartCoroutine(EnableLaser());

        IEnumerator EnableLaser()
        {
            yield return new WaitForSeconds(laserSoundInterval);
            canPlayLaser = true;
        }
    }
}