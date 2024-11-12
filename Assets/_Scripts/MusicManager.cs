using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] bool randomMusic = false;
    [ShowIf("@randomMusic == false")]
    [SerializeField] AudioClip musicClip;
    [ShowIf("@randomMusic == true")]
    [SerializeField] AudioClip[] musicClips;
    [SerializeField, Range(0,1)] float musicVolume = .15f;
    [SerializeField, HorizontalGroup("1")] bool useCustomMusicID;
    [SerializeField, HorizontalGroup("1")] int customMusicID;
    [Space]
    [SerializeField] MMF_Player musicFeedback;
    [SerializeField][PreviouslySerializedAs("fadeOut")] MMF_Player fade;
    [SerializeField] MMF_Player musicControl;
    
    static int lastMusicID = -111;
    static bool wasPaused = false; // Usado para saber se o fadeOut de pausa foi acionado, para saber se foi dado um restart
    MMF_MMSoundManagerSoundFade fadeFB;
    MMF_MMSoundManagerSoundControl musicControlFB;
    float fadeDuration = 1.8f;
    WaitForSeconds halfFade;
    int musicID = -1;

    private void Awake()
    {
        if(randomMusic && musicClips.Length > 0)
            musicClip = musicClips[Random.Range(0, musicClips.Length)];

        fadeFB = fade.GetFeedbackOfType<MMF_MMSoundManagerSoundFade>();
        halfFade = new WaitForSeconds(fadeDuration * 0.5f);
        musicControlFB = musicControl.GetFeedbackOfType<MMF_MMSoundManagerSoundControl>();
        if (useCustomMusicID) musicID = customMusicID;
        else musicID = musicClip.name.GetHashCode();

        UpdadeValues();
    }


    private void OnEnable()
    {
        if (lastMusicID != musicFeedback.GetFeedbackOfType<MMF_MMSoundManagerSound>().ID || wasPaused)
        {
            StartCoroutine(ChangeMusic());
        }

        GameStatus.PausedGame += FadeOutOnPause;
        GameStatus.GameOver += FadeOutOnPause;
        GameStatus.UnPausedGame += FadeInUnpause;
    }

    private void OnDisable()
    {
        GameStatus.PausedGame -= FadeOutOnPause;
        GameStatus.GameOver -= FadeOutOnPause;
        GameStatus.UnPausedGame -= FadeInUnpause;
    }

    IEnumerator ChangeMusic()
    {
        if (FadeOutCO != null)
            StopCoroutine(FadeOutCO);

        //CrossFade = 1/2 fade
        fadeFB.SoundID = lastMusicID;
        fadeFB.FadeDuration = fadeDuration;
        fadeFB.FinalVolume = 0;
        fade.PlayFeedbacks();
        yield return halfFade;
        musicFeedback.GetFeedbackOfType<MMF_MMSoundManagerSound>().Fade = true;
        musicFeedback.GetFeedbackOfType<MMF_MMSoundManagerSound>().FadeDuration = fadeDuration;
        musicFeedback.PlayFeedbacks();
        yield return halfFade;
        musicControlFB.SoundID = lastMusicID;
        musicControlFB.ControlMode = MMSoundManagerSoundControlEventTypes.Free;
        if(!wasPaused)
            musicControl.PlayFeedbacks();

        lastMusicID = musicFeedback.GetFeedbackOfType<MMF_MMSoundManagerSound>().ID;
        wasPaused = false;
    }
    

    Coroutine FadeOutCO;
    void FadeOutOnPause()
    {
        fadeFB.SoundID = lastMusicID;
        fadeFB.FadeDuration = fadeDuration*0.5f;
        fadeFB.FinalVolume = musicVolume * 0.15f;
        fade.PlayFeedbacks();

        //FadeOutCO = StartCoroutine(FadeOutRoutine());
        wasPaused = true;        
    }
    IEnumerator FadeOutRoutine()
    {        
        yield return halfFade;
        Debug.Log("Pause music");
        musicControlFB.SoundID = lastMusicID;
        musicControlFB.ControlMode = MMSoundManagerSoundControlEventTypes.Pause;
        musicControl.PlayFeedbacks();
    }

    void FadeInUnpause()
    {
        Debug.Log("Pause Coroutine");
        if (FadeOutCO != null)
            StopCoroutine(FadeOutCO);
        musicControlFB.SoundID = lastMusicID;
        musicControlFB.ControlMode = MMSoundManagerSoundControlEventTypes.Resume;
        //musicControl.PlayFeedbacks();
        fadeFB.SoundID = lastMusicID;
        fadeFB.FadeDuration = fadeDuration*0.5f;
        fadeFB.FinalVolume = musicVolume;
        fade.PlayFeedbacks();
        wasPaused = false;
    }

    private void OnValidate()
    {
        UpdadeValues();
    }

    void UpdadeValues()
    {
        musicFeedback.GetFeedbackOfType<MMF_MMSoundManagerSound>().Sfx = musicClip;
        musicFeedback.GetFeedbackOfType<MMF_MMSoundManagerSound>().ID = musicID;
        musicFeedback.GetFeedbackOfType<MMF_MMSoundManagerSound>().MaxVolume = musicVolume;
        musicFeedback.GetFeedbackOfType<MMF_MMSoundManagerSound>().MinVolume = musicVolume;
    }

    [Button("TestSound"), HorizontalGroup("B1")]
    void DebugPlay()
    {
        musicFeedback.GetFeedbackOfType<MMF_MMSoundManagerSound>().ExternalTestPlaySound();
    }
    [Button("StopTest"), HorizontalGroup("B1")]
    void DebugStop()
    {
        musicFeedback.GetFeedbackOfType<MMF_MMSoundManagerSound>().ExternalTestStopSound();
    }
}
