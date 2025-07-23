using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISettings : MonoBehaviour
{
    [SerializeField] private VolumeData volumeData;

    public static event Action<float> onMusicSliderChange;
    public static event Action<float> onSfxSliderChange;

    [SerializeField] private GameObject panel;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Button backButton;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullScreenToggle;
    [SerializeField] private TMP_Dropdown targetFrameRateDropdown;

    private const string currentResolutionIndexID = "CurrentResolutionIndex";

    private List<Resolution> resolutions;
    private List<string> resolutionOptions;
    private int currentResolutionIndex;

    private void Awake()
    {
        musicSlider.onValueChanged.AddListener(OnMusicSliderChange);
        sfxSlider.onValueChanged.AddListener(OnSfxSliderChange);
        backButton.onClick.AddListener(OnBackButtonClick);
        resolutionDropdown.onValueChanged.AddListener(OnSetResolution);
        fullScreenToggle.onValueChanged.AddListener(OnFullscreenChange);

        (List<string>, List<Resolution>) resolutionTuple  = GetResolutions();
        resolutionOptions = resolutionTuple.Item1;
        resolutions = resolutionTuple.Item2;

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = GetCurrentResolution();
        resolutionDropdown.RefreshShownValue();

        fullScreenToggle.isOn = Screen.fullScreen;
    }
    private void Start()
    {
        musicSlider.value = volumeData.Music;
        sfxSlider.value = volumeData.Sfx;
    }

    private void OnDestroy()
    {
        musicSlider.onValueChanged.RemoveListener(OnMusicSliderChange);
        sfxSlider.onValueChanged.RemoveListener(OnSfxSliderChange);
        backButton.onClick.RemoveListener(OnBackButtonClick);
        resolutionDropdown.onValueChanged.RemoveListener(OnSetResolution);
        fullScreenToggle.onValueChanged.RemoveListener(OnFullscreenChange);
    }

    private void OnBackButtonClick()
    {
    }

    private void OnSfxSliderChange(float value)
    {
        onSfxSliderChange?.Invoke(value);
    }

    private void OnMusicSliderChange(float value)
    {
        onMusicSliderChange?.Invoke(value);
    }

    private void OnSetResolution(int index)
    {
        PlayerPrefs.SetInt(currentResolutionIndexID, index);
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    private void OnFullscreenChange(bool value)
    { 
        Screen.fullScreen = value;
    }

    private (List<string>, List<Resolution>) GetResolutions()
    {
        List<Resolution> resolutions = new List<Resolution>();
        List <string> resolutionOptions = new List<string>();
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            Resolution resolution = Screen.resolutions[i];

            string option = resolution.width + " x " + resolution.height;
            if (!resolutionOptions.Contains(option))
            {
                resolutions.Add(resolution);
                resolutionOptions.Add(option);
            }
        }
        return (resolutionOptions, resolutions);
    }

    private int GetCurrentResolution()
    {
        int currentResolutionIndex = 0;
        if (PlayerPrefs.HasKey(currentResolutionIndexID))
        {
            currentResolutionIndex = PlayerPrefs.GetInt(currentResolutionIndexID);
        }
        else
        {
            for (int i = 0; i < resolutions.Count; i++)
            {
                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                    PlayerPrefs.SetInt(currentResolutionIndexID, currentResolutionIndex);
                }
            }
        }
        return currentResolutionIndex;
    }
}
