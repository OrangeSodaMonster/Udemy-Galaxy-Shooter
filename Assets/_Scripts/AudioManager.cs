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

    [field: Header("Collectibles")]
    [field:SerializeField] public MMFeedbacks MetalCrumbSound { get; private set; }
    [field:SerializeField] public MMFeedbacks RareMetalCrumbSound { get; private set; }
    [field:SerializeField] public MMFeedbacks EnergyCrystalSound { get; private set; }
    [field:SerializeField] public MMFeedbacks CondensedEnergyCrystalSound { get; private set; }

    [field: Header("Impact")]
    [field: SerializeField] public MMFeedbacks AsteroidHitSound { get; private set; }

    [field: Header("Destruction")]
    [field: SerializeField] public MMFeedbacks EnemyDestructionSound { get; private set; }
    [field: SerializeField] public MMFeedbacks EnemyProjectileDestructionSound { get; private set; }
    [field: SerializeField] public MMFeedbacks AsteroidDestructionSound { get; private set; }

    [field: Header("Enemy Fire")]
    [field: SerializeField] public MMFeedbacks EnemyChargeSound { get; private set; }
    [field: SerializeField] public MMFeedbacks EnemyFireSound { get; private set; }

    public List<int> DronesActive = new();
    int dronesActiveLastFrame;
    float dronesDefaultVolume;
    MMFeedbackAudioSource droneAudioFeedback;

    public static AudioManager Instance;
    void Awake()
    {
        if(Instance == null)
            Instance = this;

    }

    private void Start()
    {
        PauseDrone();

        dronesDefaultVolume = DronesSound.volume;
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

    void PauseDrone()
    {
        DronesSound.DOFade(0, .1f).OnComplete(() => DronesSound.Pause());
        
    }
    void PlayDrone()
    {
        DronesSound.volume = 0;
        DronesSound.Play();
        DronesSound.DOFade(dronesDefaultVolume, .1f);
    }



}