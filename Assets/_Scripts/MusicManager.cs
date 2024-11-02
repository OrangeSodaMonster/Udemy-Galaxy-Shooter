using DG.Tweening;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioClip musicClip;
    [SerializeField, Range(0,1)] float musicVolume = .15f;
    [SerializeField, HorizontalGroup("1")] bool useCustomMusicID;
    [SerializeField, HorizontalGroup("1")] int customMusicID;
    [Space]
    [SerializeField] MMF_Player musicFeedback;
    [SerializeField][PreviouslySerializedAs("fadeOut")] MMF_Player fade;
    [SerializeField] MMF_Player freeAudio;

    static int lastMusicID = -111;
    MMF_MMSoundManagerSoundFade fadeOutFB;
    MMF_MMSoundManagerSoundControl freeAudioFB;
    float fadeDuration = 2f;
    WaitForSeconds halfFade;
    int musicID = -1;

    private void Awake()
    {
        fadeOutFB = fade.GetFeedbackOfType<MMF_MMSoundManagerSoundFade>();
        halfFade = new WaitForSeconds(fadeDuration * 0.5f);
        freeAudioFB = freeAudio.GetFeedbackOfType<MMF_MMSoundManagerSoundControl>();
        if (useCustomMusicID) musicID = customMusicID;
        else musicID = musicClip.name.GetHashCode();

        UpdadeValues();
    }

    private void OnEnable()
    {
        Debug.Log("MusicManager Enabled");
        if (lastMusicID != musicFeedback.GetFeedbackOfType<MMF_MMSoundManagerSound>().ID)
        {
            Debug.Log("-- Change Music --");
            StartCoroutine(ChangeMusic());
        }

        GameStatus.PausedGame += FadeOutOnPause;
        GameStatus.UnPausedGame += FadeInUnpause;
    }

    private void OnDisable()
    {
        GameStatus.PausedGame -= FadeOutOnPause;
        GameStatus.UnPausedGame -= FadeInUnpause;
    }

    IEnumerator ChangeMusic()
    {
        //CrossFade = 1/2 fade
        fadeOutFB.SoundID = lastMusicID;
        fadeOutFB.FadeDuration = fadeDuration;
        fadeOutFB.FinalVolume = 0;
        fade.PlayFeedbacks();
        yield return halfFade;
        musicFeedback.GetFeedbackOfType<MMF_MMSoundManagerSound>().Fade = true;
        musicFeedback.GetFeedbackOfType<MMF_MMSoundManagerSound>().FadeDuration = fadeDuration;
        musicFeedback.PlayFeedbacks();
        yield return halfFade;
        freeAudioFB.SoundID = lastMusicID;
        freeAudio.PlayFeedbacks();

        lastMusicID = musicFeedback.GetFeedbackOfType<MMF_MMSoundManagerSound>().ID;
    }

    void FadeOutOnPause()
    {
        fadeOutFB.SoundID = lastMusicID;
        fadeOutFB.FadeDuration = fadeDuration;
        fadeOutFB.FinalVolume = 0;
        fade.PlayFeedbacks();
    }

    void FadeInUnpause()
    {
        fadeOutFB.SoundID = lastMusicID;
        fadeOutFB.FadeDuration = fadeDuration;
        fadeOutFB.FinalVolume = musicVolume;
        fade.PlayFeedbacks();
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
