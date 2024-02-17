using DG.Tweening;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [field:Header("PlayerWeapons")]
    [field:SerializeField] public MMFeedbacks LaserSound { get; private set; }
    [field:SerializeField] public MMFeedbacks IonStreamSound { get; private set; }
    [field:SerializeField] public AudioSource DronesSound { get; private set; }
    [field:SerializeField] public MMFeedbacks BombSound { get; private set; }
    [field:SerializeField] public MMFeedbacks ShieldUpSound { get; private set; }
    [field:SerializeField] public MMFeedbacks ShipFix { get; private set; }

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

    [field: Header("Interface")]
    [field: SerializeField] public MMFeedbacks SelectionClickSound { get; private set; }
    [field: SerializeField] public MMFeedbacks ConfirmationClickSound { get; private set; }
    [field: SerializeField] public MMFeedbacks HoverSound { get; private set; }
    [field: SerializeField] public MMFeedbacks BackSound { get; private set; }
    [field: SerializeField] public MMFeedbacks UnlockUpgradeSound { get; private set; }
    [field: SerializeField] public MMFeedbacks UpgradeSound { get; private set; }

    [field: Header("Other")]
    [field: SerializeField] public AudioSource AlarmSound { get; private set; }
    [field: SerializeField] public MMFeedbacks RareSpawnSound { get; private set; }

    [HideInInspector] public List<int> DronesActive = new();
    int dronesActiveLastFrame;
    float dronesDefaultVolume;
    float alarmDefaultVolume;
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
    }

    private void Start()
    {
        DronesSound.Pause();
        dronesDefaultVolume = DronesSound.volume;
        AlarmSound.Pause();
        alarmDefaultVolume = AlarmSound.volume;

        enemyChargeFB = EnemyChargeSound.GetComponent<MMFeedbackMMSoundManagerSound>();
        asteroidDestructionFB = AsteroidDestructionSound.GetComponent<MMFeedbackMMSoundManagerSound>();
        asteroidDestructionDefaultVolume = asteroidDestructionFB.MinVolume;
    }
    
    private void LateUpdate()
    {
        if (DronesActive.Count > 0)
        {
            DronesSound.volume = dronesDefaultVolume * DronesActive.Count - dronesDefaultVolume * 0.5f * (DronesActive.Count - 1);
        }

        if (DronesActive.Count > 0 && dronesActiveLastFrame == 0)
        {
            PlayDrone();            
        }
        else if (DronesActive.Count == 0 && dronesActiveLastFrame > 0)
        {
            PauseDrone();            
        }

        dronesActiveLastFrame = DronesActive.Count;
    }

    bool playedDrones;
    void PauseDrone()
    {
        if (!playedDrones) return;

        DronesSound.DOFade(0, .1f).OnComplete(() => DronesSound.Pause());
        playedDrones = false;
    }
    void PlayDrone()
    {
        if (playedDrones) return;

        DronesSound.volume = 0;
        DronesSound.Play();
        DronesSound.DOFade(dronesDefaultVolume, .3f);
        playedDrones = true;
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