using System;
using System.Collections.Generic;
using System.Reflection;
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
        resolutionOptions = ConfigurationManager.GetResolutionsOptions();
        resolutions = ConfigurationManager.GetResolutions();
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = GetCurrentResolution();
        resolutionDropdown.RefreshShownValue();
        Resolution resolution = resolutions[resolutionDropdown.value];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        // Set saved fullscreen value
        if (!PlayerPrefs.HasKey(ConfigurationManager.FullScreenToggleID))
        {
            PlayerPrefs.SetInt(ConfigurationManager.FullScreenToggleID, Screen.fullScreen ? 1 : 0);
        }
        bool isFullScreen = PlayerPrefs.GetInt(ConfigurationManager.FullScreenToggleID) == 1;
        fullScreenToggle.isOn = isFullScreen;
        Screen.fullScreen = isFullScreen;

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
        if (!PlayerPrefs.HasKey(ConfigurationManager.VsyncToggleID))
        {
            // cap to one, in case where VSyncCount > 1
            int vSyncCount = Math.Min(QualitySettings.vSyncCount, 1);
            PlayerPrefs.SetInt(ConfigurationManager.VsyncToggleID, vSyncCount);
        }
        int vsyncCount = PlayerPrefs.GetInt(ConfigurationManager.VsyncToggleID);
        QualitySettings.vSyncCount = vsyncCount;
        vsyncToggle.isOn = vsyncCount == 1;
        frameRateDropdown.interactable = vsyncCount == 0;
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
        PlayerPrefs.SetInt(ConfigurationManager.CurrentResolutionIndexID, index);
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    private void OnFullscreenChange(bool value)
    {
        PlayerPrefs.SetInt(ConfigurationManager.FullScreenToggleID, value ? 1 : 0);
        Screen.fullScreen = value;
    }

    private void OnFrameRateChange(int index)
    {
        PlayerPrefs.SetInt(ConfigurationManager.CurrentFrameRateIndexID, index);
        int frameRate = settingsData.MaxFrameRates[index];
        Application.targetFrameRate = frameRate;
    }

    private void OnVsyncChange(bool value)
    {
        PlayerPrefs.SetInt(ConfigurationManager.VsyncToggleID, value ? 1 : 0);
        QualitySettings.vSyncCount = value ? 1 : 0;
        frameRateDropdown.interactable = !value;
    }

    private int GetCurrentResolution()
    {
        int currentResolutionIndex = 0;
        if (PlayerPrefs.HasKey(ConfigurationManager.CurrentResolutionIndexID))
        {
            currentResolutionIndex = PlayerPrefs.GetInt(ConfigurationManager.CurrentResolutionIndexID);
        }
        else
        {
            for (int i = 0; i < resolutions.Count; i++)
            {
                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                    PlayerPrefs.SetInt(ConfigurationManager.CurrentResolutionIndexID, currentResolutionIndex);
                }
            }
        }
        return currentResolutionIndex;
    }

    private int GetCurrentFrameRateIndex()
    {
        int currentFrameRateIndex = 0;
        if (PlayerPrefs.HasKey(ConfigurationManager.CurrentFrameRateIndexID))
        {
            currentFrameRateIndex = PlayerPrefs.GetInt(ConfigurationManager.CurrentFrameRateIndexID);
        }
        else
        {
            for (int i = 0; i < settingsData.MaxFrameRates.Length; i++)
            {
                if (settingsData.MaxFrameRates[i] == Application.targetFrameRate)
                {
                    currentFrameRateIndex = i;
                    PlayerPrefs.SetInt(ConfigurationManager.CurrentFrameRateIndexID, currentFrameRateIndex);
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
