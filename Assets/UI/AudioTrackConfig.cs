using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioTrackConfig : MonoBehaviour
{
    public Slider masterVolume;
    public Slider musicVolume;
    public Slider sfxVolume;
    public Slider uiVolume;

    //int currentMaster = 1;
    //int currentMusic = 1;
    //int currentSFX = 1;
    //int currentUI = 1;

    public static AudioTrackConfig Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.UnmuteTrack, MMSoundManager.MMSoundManagerTracks.Sfx);
        StartCoroutine(LoadVolumes());
    }

    public void SetSliders()
    {
        // Mudar para usar os valores salvos dos sliders

        masterVolume.SetValueWithoutNotify(GameManager.MasterVolume);
        musicVolume.SetValueWithoutNotify(GameManager.MusicVolume);
        sfxVolume.SetValueWithoutNotify(GameManager.EffectsVolume);
        uiVolume.SetValueWithoutNotify(GameManager.UiVolume);

        Debug.Log($"SET SLIDERS: {GameManager.MasterVolume}, {GameManager.MusicVolume}, {GameManager.EffectsVolume}, {GameManager.UiVolume}");
    }
    //public void LoadVolume(int master, int music, int sfx, int ui)
    //{
    //    StartCoroutine(Routine());

    //    IEnumerator Routine()
    //    {
    //        yield return null;

    //        // slider 5 >>> volume 1
    //        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.SetVolumeTrack, MMSoundManager.MMSoundManagerTracks.Master, master * 0.2f);
    //        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.SetVolumeTrack, MMSoundManager.MMSoundManagerTracks.Music, music * 0.2f);
    //        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.SetVolumeTrack, MMSoundManager.MMSoundManagerTracks.Sfx, sfx * 0.2f);
    //        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.SetVolumeTrack, MMSoundManager.MMSoundManagerTracks.UI, ui * 0.2f);
    //    }

    //    //currentMaster = master;
    //    //currentMusic = music;
    //    //currentSFX = sfx;
    //    //currentUI = ui;

    //    Debug.Log($"Volumes: {master}, {music}, {sfx}, {ui}");
    //}

    public IEnumerator LoadVolumes()
    {
        yield return null;

        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.SetVolumeTrack, MMSoundManager.MMSoundManagerTracks.Master, GameManager.MasterVolume * 0.2f);
        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.SetVolumeTrack, MMSoundManager.MMSoundManagerTracks.Music, GameManager.MusicVolume * 0.2f);
        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.SetVolumeTrack, MMSoundManager.MMSoundManagerTracks.Sfx, GameManager.EffectsVolume * 0.2f);
        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.SetVolumeTrack, MMSoundManager.MMSoundManagerTracks.UI, GameManager.UiVolume * 0.2f);
    }

    public void SetVolumes()
    {
        SetMaster();
        SetMusic();
        SetSFX();
        SetUI();
    }

    public void SetMaster()
    {        
        float newVolume = 0.2f * masterVolume.value;
        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.SetVolumeTrack, MMSoundManager.MMSoundManagerTracks.Master, newVolume);
        GameManager.MasterVolume = (int)masterVolume.value;
    }
    public void SetMusic()
    {
        float newVolume = 0.2f * musicVolume.value;
        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.SetVolumeTrack, MMSoundManager.MMSoundManagerTracks.Music, newVolume);
        GameManager.MusicVolume = (int)musicVolume.value;
    }       

    float sfxTrackVolume;
    public void SetSFX()
    {
        sfxTrackVolume = 0.2f * sfxVolume.value;

        if(!MMSoundManager.Instance.IsMuted(MMSoundManager.MMSoundManagerTracks.Sfx))
            MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.SetVolumeTrack, MMSoundManager.MMSoundManagerTracks.Sfx, sfxTrackVolume);
        GameManager.EffectsVolume = (int)sfxVolume.value;

        Debug.Log(sfxTrackVolume);
    }
    public void SetUI()
    {
        float newVolume = 0.2f * uiVolume.value;
        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.SetVolumeTrack, MMSoundManager.MMSoundManagerTracks.UI, newVolume);
        GameManager.UiVolume = (int)uiVolume.value;
    }

    public void MuteVFX()
    {
        sfxTrackVolume = MMSoundManager.Instance.GetTrackVolume(MMSoundManager.MMSoundManagerTracks.Sfx, false);
        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.MuteTrack, MMSoundManager.MMSoundManagerTracks.Sfx);

        //Debug.Log($"Mute SFX {sfxTrackVolume}");
    }
    public void UnmuteVFX()
    {
        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.UnmuteTrack, MMSoundManager.MMSoundManagerTracks.Sfx);
        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.SetVolumeTrack, MMSoundManager.MMSoundManagerTracks.Sfx, sfxTrackVolume);

        //Debug.Log($"Unmute SFX {sfxTrackVolume}");
    }
}