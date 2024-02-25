using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SetSlidersOnEnable : MonoBehaviour
{
    //[SerializeField] UnityEvent onEnable;
    [SerializeField] Slider masterVolume;
    [SerializeField] Slider musicVolume;
    [SerializeField] Slider sfxVolume;
    [SerializeField] Slider uiVolume;

    private void OnEnable()
    {
        //onEnable?.Invoke();

        AudioTrackConfig.Instance.masterVolume = masterVolume;
        AudioTrackConfig.Instance.musicVolume = musicVolume;
        AudioTrackConfig.Instance.sfxVolume = sfxVolume;
        AudioTrackConfig.Instance.uiVolume = uiVolume;

        masterVolume.onValueChanged.AddListener(MasterListener);
        musicVolume.onValueChanged.AddListener(MusicListener);
        sfxVolume.onValueChanged.AddListener(SfxListener);
        uiVolume.onValueChanged.AddListener(UiListener);

        AudioTrackConfig.Instance.SetSliders();
    }

    public void MasterListener(float value)
    {
        AudioManager.Instance.HoverSound.PlayFeedbacks();
        AudioTrackConfig.Instance.SetMaster();
    }

    public void MusicListener(float value)
    {
        AudioManager.Instance.HoverSound.PlayFeedbacks();
        AudioTrackConfig.Instance.SetMusic();
    }

    public void SfxListener(float value)
    {
        AudioManager.Instance.HoverSound.PlayFeedbacks();
        AudioTrackConfig.Instance.SetSFX();
    }

    public void UiListener(float value)
    {
        AudioManager.Instance.HoverSound.PlayFeedbacks();
        AudioTrackConfig.Instance.SetUI();
    }
}