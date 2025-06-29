using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour 
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private VolumeData volumeData;

    private void Awake()
    {
        UIManager.onMasterSliderChange += OnMasterSliderChange;
        UIManager.onMusicSliderChange += OnMusicSliderChange;
        UIManager.onSfxSliderChange += OnSfxSliderChange;
        mixer.SetFloat("SfxVolume", Utils.LinearToDecibel(volumeData.Sfx));
        mixer.SetFloat("MusicVolume", Utils.LinearToDecibel(volumeData.Music));
        mixer.SetFloat("MasterVolume", Utils.LinearToDecibel(volumeData.Master));
    }
    private void OnDestroy()
    {
        UIManager.onMasterSliderChange -= OnMasterSliderChange;
        UIManager.onMusicSliderChange -= OnMusicSliderChange;
        UIManager.onSfxSliderChange -= OnSfxSliderChange;
    }
    private void OnSfxSliderChange(float value)
    {
        volumeData.Sfx = value;
        mixer.SetFloat("SfxVolume", Utils.LinearToDecibel(volumeData.Sfx));
    }

    private void OnMusicSliderChange(float value)
    {
        volumeData.Music = value;
        mixer.SetFloat("MusicVolume", Utils.LinearToDecibel(volumeData.Music));
    }

    private void OnMasterSliderChange(float value)
    {
        volumeData.Master = value;
        mixer.SetFloat("MasterVolume", Utils.LinearToDecibel(volumeData.Master));
    }

}
