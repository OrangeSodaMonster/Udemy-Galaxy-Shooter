using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioTrackConfig : MonoBehaviour
{
    [SerializeField] Slider masterVolume;
    [SerializeField] Slider musicVolume;
    [SerializeField] Slider sfxVolume;
    [SerializeField] Slider uiVolume;

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
        SetSliders();

        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.UnmuteTrack, MMSoundManager.MMSoundManagerTracks.Sfx);
    }

    void SetSliders()
    {
        // Mudar para usar os valores salvos dos sliders

        masterVolume.SetValueWithoutNotify(5 * MMSoundManager.Instance.GetTrackVolume(MMSoundManager.MMSoundManagerTracks.Master, false));
        musicVolume.SetValueWithoutNotify(5 * MMSoundManager.Instance.GetTrackVolume(MMSoundManager.MMSoundManagerTracks.Music, false));
        sfxVolume.SetValueWithoutNotify(5 * MMSoundManager.Instance.GetTrackVolume(MMSoundManager.MMSoundManagerTracks.Sfx, false));
        uiVolume.SetValueWithoutNotify(5 * MMSoundManager.Instance.GetTrackVolume(MMSoundManager.MMSoundManagerTracks.UI, false));
    }

    // Chamar em load, salvar valores dos sliders
    public void SetVolumes()
    {
        SetMaster();
        SetMusic();
        SetSFX();
        SetUI();
    }

    public void SetMaster()
    {        
        float newVolume = 2 * masterVolume.value/masterVolume.maxValue;
        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.SetVolumeTrack, MMSoundManager.MMSoundManagerTracks.Master, newVolume);
    }
    public void SetMusic()
    {
        float newVolume = 2 * musicVolume.value/musicVolume.maxValue;
        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.SetVolumeTrack, MMSoundManager.MMSoundManagerTracks.Music, newVolume);
    }    
    public void SetUI()
    {
        float newVolume = 2 * uiVolume.value/uiVolume.maxValue;
        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.SetVolumeTrack, MMSoundManager.MMSoundManagerTracks.UI, newVolume);
    }

    float vfxVolume;
    public void SetSFX()
    {
        vfxVolume = 2 * sfxVolume.value/sfxVolume.maxValue;

        if(!MMSoundManager.Instance.IsMuted(MMSoundManager.MMSoundManagerTracks.Sfx))
            MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.SetVolumeTrack, MMSoundManager.MMSoundManagerTracks.Sfx, vfxVolume);
    }
    public void MuteVFX()
    {
        vfxVolume = MMSoundManager.Instance.GetTrackVolume(MMSoundManager.MMSoundManagerTracks.Sfx, false);
        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.MuteTrack, MMSoundManager.MMSoundManagerTracks.Sfx);
    }
    public void UnmuteVFX()
    {
        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.UnmuteTrack, MMSoundManager.MMSoundManagerTracks.Sfx);
        MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.SetVolumeTrack, MMSoundManager.MMSoundManagerTracks.Sfx, vfxVolume);
    }
}