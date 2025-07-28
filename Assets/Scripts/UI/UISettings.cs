using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISettings : MonoBehaviour
{
    [SerializeField] private VolumeData volumeData;
    [SerializeField] private SettingsData settingsData;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Button backButton;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullScreenToggle;
    [SerializeField] private TMP_Dropdown frameRateDropdown;
    [SerializeField] private Toggle vsyncToggle;

    private const string currentResolutionIndexID = "CurrentResolutionIndex";
    private const string fullScreenToggleID = "FullScreenToggle";
    private const string currentFrameRateIndexID = "CurrentFrameRateIndex";
    private const string vsyncToggleID = "VsyncToggle";

    private List<Resolution> resolutions;
    private List<string> resolutionOptions;
    private List<string> frameRatesOptions;

    private void Awake()
    {
        musicSlider.onValueChanged.AddListener(OnMusicSliderChange);
        sfxSlider.onValueChanged.AddListener(OnSfxSliderChange);
        backButton.onClick.AddListener(OnBackButtonClick);
        resolutionDropdown.onValueChanged.AddListener(OnSetResolution);
        fullScreenToggle.onValueChanged.AddListener(OnFullscreenChange);
        frameRateDropdown.onValueChanged.AddListener(OnFrameRateChange);
        vsyncToggle.onValueChanged.AddListener(OnVsyncChange);

        // Set saved resolution
        (List<string>, List<Resolution>) resolutionTuple  = GetResolutions();
        resolutionOptions = resolutionTuple.Item1;
        resolutions = resolutionTuple.Item2;
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = GetCurrentResolution();
        resolutionDropdown.RefreshShownValue();

        // Set saved fullscreen value
        if (!PlayerPrefs.HasKey(fullScreenToggleID))
        {
            PlayerPrefs.SetInt(fullScreenToggleID, Screen.fullScreen ? 1 : 0);
        }
        fullScreenToggle.isOn = PlayerPrefs.GetInt(fullScreenToggleID) == 1;

        // Set saved Max FPS
        frameRatesOptions = new List<string>();
        foreach (int frameRate in settingsData.MaxFrameRates)
        { 
            string frameRateOption = frameRate.ToString();
            if (frameRate == -1)
            {
                frameRateOption = "Unbound";
            }
            frameRatesOptions.Add(frameRateOption);
        }

        frameRateDropdown.ClearOptions();
        frameRateDropdown.AddOptions(frameRatesOptions);
        int frameRateIndex = GetCurrentFrameRateIndex();
        frameRateDropdown.value = frameRateIndex;
        frameRateDropdown.RefreshShownValue();
        Application.targetFrameRate = settingsData.MaxFrameRates[frameRateIndex];

        // Set saved Vsync value
        if (!PlayerPrefs.HasKey(vsyncToggleID))
        {
            // cap to one, in case where VSyncCount > 1
            int vSyncCount = Math.Min(QualitySettings.vSyncCount, 1);
            PlayerPrefs.SetInt(vsyncToggleID, vSyncCount);
        }
        int vsyncCount = PlayerPrefs.GetInt(vsyncToggleID);
        QualitySettings.vSyncCount = vsyncCount;
        vsyncToggle.isOn = vsyncCount == 1;
        frameRateDropdown.enabled = vsyncCount == 0;
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
        frameRateDropdown.onValueChanged.RemoveListener(OnFrameRateChange);
        vsyncToggle.onValueChanged.RemoveListener(OnVsyncChange);
    }

    private void OnEnable()
    {
        GameEventManager.Instance.AddListener<JoystickOrKeyboardUseEvent>(OnJoystickAndKeyboardUse);
        OnJoystickAndKeyboardUse(null);
    }

    private void OnDisable()
    {
        GameEventManager.Instance.RemoveListener<JoystickOrKeyboardUseEvent>(OnJoystickAndKeyboardUse);
    }

    private void OnBackButtonClick()
    {
        GameEventManager.Instance.TriggerEvent(SettingBackButtonClickEvent.GetEvent());
    }

    private void OnSfxSliderChange(float value)
    {
        GameEventManager.Instance.TriggerEvent(SfxSliderChangeEvent.GetEvent(value));
    }

    private void OnMusicSliderChange(float value)
    {
        GameEventManager.Instance.TriggerEvent(MusicSliderChangeEvent.GetEvent(value));
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
        PlayerPrefs.SetInt(fullScreenToggleID, Screen.fullScreen ? 1 : 0);
    }

    private void OnFrameRateChange(int index)
    {
        PlayerPrefs.SetInt(currentFrameRateIndexID, index);
        int frameRate = settingsData.MaxFrameRates[index];
        Application.targetFrameRate = frameRate;
    }

    private void OnVsyncChange(bool value)
    {
        PlayerPrefs.SetInt(vsyncToggleID, value ? 1 : 0);
        QualitySettings.vSyncCount = value ? 1 : 0;
        frameRateDropdown.enabled = !value;
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

    private int GetCurrentFrameRateIndex()
    {
        int currentFrameRateIndex = 0;
        if (PlayerPrefs.HasKey(currentFrameRateIndexID))
        {
            currentFrameRateIndex = PlayerPrefs.GetInt(currentFrameRateIndexID);
        }
        else
        {
            for (int i = 0; i < settingsData.MaxFrameRates.Length; i++)
            {
                if (settingsData.MaxFrameRates[i] == Application.targetFrameRate)
                {
                    currentFrameRateIndex = i;
                    PlayerPrefs.SetInt(currentFrameRateIndexID, currentFrameRateIndex);
                }
            }
        }
        return currentFrameRateIndex;
    }

    private void OnJoystickAndKeyboardUse(GameEvent gameEvent)
    {
        EventSystem.current.firstSelectedGameObject = musicSlider.gameObject;
        musicSlider.Select();
    }
}
