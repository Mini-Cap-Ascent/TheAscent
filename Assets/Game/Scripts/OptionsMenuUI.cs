using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Manages the UI interactions for the Options Menu, allowing players to adjust settings such as resolution,
/// fullscreen mode, and audio volumes. It communicates changes through a generic event bus system.
/// </summary>
public class OptionsMenuUI : MonoBehaviour
{
    #region Fields
    [SerializeField] private TMP_Dropdown resolutionDropwdon;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Button backButton;
    private Resolution[] resolutions;
    #endregion

    #region Unity Methods

    private void Start()
    {
        InitializeUIState();
        UpdateUIWithCurrentSettings();
        InitializeUIComponents();
        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }
    #endregion

    #region Initialization
    /// <summary>
    /// Initializes UI components and registers callbacks for UI interactions.
    /// </summary>
    private void InitializeUIComponents()
    {
        // Back Button
        backButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlaySound("BtnClick");
            GameManager.Instance.ResumePreviousState();
        });

        // Fullscreen Toggle
        fullscreenToggle.onValueChanged.AddListener(isOn =>
        {
            SettingsManager.Instance.SaveIsFullScreen(isOn);
        });

        // Master Volume Slider
        masterVolumeSlider.onValueChanged.AddListener(volume =>
        {
            SettingsManager.Instance.SaveAudioSettings(
                volume,
                musicVolumeSlider.value,
                sfxVolumeSlider.value
            );
        });

        // Music Volume Slider 
        musicVolumeSlider.onValueChanged.AddListener(volume =>
        {
            SettingsManager.Instance.SaveAudioSettings(
                masterVolumeSlider.value,
                volume,
                sfxVolumeSlider.value
            );
        });

        // SFX Volume Slider
        sfxVolumeSlider.onValueChanged.AddListener(volume =>
        {
            SettingsManager.Instance.SaveAudioSettings(
                masterVolumeSlider.value,
                musicVolumeSlider.value,
                volume
            );
        });

        // Assuming resolutionDropdown is correctly set up elsewhere
        resolutionDropwdon.onValueChanged.AddListener(index =>
        {
            SettingsManager.Instance.SaveResolution(
                resolutions[index].width,
                resolutions[index].height,
                fullscreenToggle.isOn
            );
        });

        qualityDropdown.onValueChanged.AddListener(index =>
        {
            SettingsManager.Instance.SaveQualityLevel(index);
        });

    }

    /// <summary>
    /// Sets the initial state of the UI based on current game settings.
    /// </summary>
    private void InitializeUIState()
    {
        LoadResolutions();
        ToggleOptionsMenuVisibility(false);
    }

    private void UpdateUIWithCurrentSettings()
    {
        // Ensure SettingsManager instance is available
        if (SettingsManager.Instance != null)
        {
            // Update sliders
            masterVolumeSlider.value = SettingsManager.Instance.MasterVolume;
            musicVolumeSlider.value = SettingsManager.Instance.MusicVolume;
            sfxVolumeSlider.value = SettingsManager.Instance.SfxVolume;

            // Update toggle
            fullscreenToggle.isOn = SettingsManager.Instance.IsFullScreen;

            // Update resolution dropdown - find the current resolution index
            int currentResolutionIndex = Array.FindIndex(resolutions, r => r.width == SettingsManager.Instance.GameResolution.width && r.height == SettingsManager.Instance.GameResolution.height);
            resolutionDropwdon.value = resolutionDropwdon.value = currentResolutionIndex;

            qualityDropdown.value = SettingsManager.Instance.QualityLevel;
        }
        else
        {
            Debug.LogWarning("SettingsManager instance not found. Make sure it's initialized before accessing it.");
        }
    }

    #endregion

    #region Menu Visibility Management

    /// <summary>
    /// Handles the event to show the Options Menu.
    /// </summary>
    /// <param name="eventData">The event data associated with showing the options menu. Currently unused but can be extended for future use.</param>
    private void OnShowOptionsMenu(object eventData)
    {
        // Code to show the Options Menu
        Debug.Log(eventData.ToString());
        ToggleOptionsMenuVisibility(true);
    }

    /// <summary>
    /// Handles the event to hide the Options Menu.
    /// </summary>
    /// <param name="eventData">The event data associated with hiding the options menu. Currently unused but can be extended for future use.</param>
    private void OnHideOptionsMenu(object eventData)
    {
        // Code to hide the Options Menu
        ToggleOptionsMenuVisibility(false);
    }

    #endregion


    #region Event Subscriptions
    /// <summary>
    /// Subscribes to necessary events on the event bus.
    /// </summary>
    private void SubscribeToEvents()
    {
        if (EventBus.Instance != null)
        {
            EventBus.Instance.Subscribe<ShowOptionsMenuEvent>(OnShowOptionsMenu);
            EventBus.Instance.Subscribe<HideOptionsMenuEvent>(OnHideOptionsMenu);
        }
    }

    /// <summary>
    /// Unsubscribes from events on the event bus.
    /// </summary>
    private void UnsubscribeFromEvents()
    {
        if (EventBus.Instance != null)
        {
            EventBus.Instance.Unsubscribe<ShowOptionsMenuEvent>(OnShowOptionsMenu);
            EventBus.Instance.Unsubscribe<HideOptionsMenuEvent>(OnHideOptionsMenu);
        }
    }
    #endregion

    #region UI Methods
    /// <summary>
    /// Loads and populates the resolution dropdown with available screen resolutions.
    /// </summary>
    private void LoadResolutions()
    {
        resolutions = Screen.resolutions.Select(
            resolution => new Resolution { width = resolution.width, height = resolution.height })
            .DistinctBy(res => new { res.width, res.height })
            .ToArray();
        Array.Reverse(resolutions); // Optional: reverse to have the highest resolution at the top
        List<string> options = new List<string>();
        var currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            var resolution = resolutions[i];
            var option = $"{resolution.width}x{resolution.height}";
            options.Add(option);

            if (resolution.width == Screen.currentResolution.width && resolution.height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropwdon.ClearOptions();
        resolutionDropwdon.AddOptions(options);
        resolutionDropwdon.value = currentResolutionIndex;
        resolutionDropwdon.RefreshShownValue();
    }

    /// <summary>
    /// Toggles the visibility of the options menu UI.
    /// </summary>
    /// <param name="isVisible">Whether the options menu should be visible.</param>
    public void ToggleOptionsMenuVisibility(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }
    #endregion
}