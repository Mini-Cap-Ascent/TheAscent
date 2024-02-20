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


    
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Button backButton;
    private bool isPaused = false;
    private Resolution[] resolutions;

    private void Start()
    {
        InitializeUIComponents();
        InitializeUIState(); // Ensure this method is called to set initial UI state.
    }

    private void InitializeUIComponents()
    {
        backButton.onClick.AddListener(() =>
        {

           EventManager.TriggerHideOptionsMenu();
            

        });

        fullscreenToggle.onValueChanged.AddListener(isOn =>
        {
            EventManager.TriggerFullscreenToggled(isOn);
        });

        masterVolumeSlider.onValueChanged.AddListener(volume =>
        {
            EventManager.TriggerAudioSettingsChanged(volume, musicVolumeSlider.value, sfxVolumeSlider.value);
        });

        musicVolumeSlider.onValueChanged.AddListener(volume =>
        {
            EventManager.TriggerAudioSettingsChanged(masterVolumeSlider.value, volume, sfxVolumeSlider.value);
        });

        sfxVolumeSlider.onValueChanged.AddListener(volume =>
        {
            EventManager.TriggerAudioSettingsChanged(masterVolumeSlider.value, musicVolumeSlider.value, volume);
        });

        resolutionDropdown.onValueChanged.AddListener(index =>
        {
            Resolution selectedResolution = Screen.resolutions[index];
            EventManager.TriggerResolutionChanged(selectedResolution.width, selectedResolution.height, fullscreenToggle.isOn);
        });

        qualityDropdown.onValueChanged.AddListener(index =>
        {
            EventManager.TriggerQualityLevelChanged(index);
        });
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (GameManager.Instance != null)
        {
            if (isPaused) GameManager.Instance.PauseGame();
            else GameManager.Instance.ResumeGame();
        }

        ToggleOptionsMenuVisibility(isPaused);
    }

    private void ToggleOptionsMenuVisibility(bool isVisible)
    {
        gameObject.SetActive(isVisible);
        // Ensure you find the Canvas only if needed to avoid overhead.
        if (isVisible)
        {
            Canvas canvas = FindObjectOfType<Canvas>(); // Consider caching this reference.
            if (canvas != null) canvas.gameObject.SetActive(isVisible);
        }
    }

    /// <summary>
    /// Sets the initial state of the UI based on current game settings.
    /// </summary>
    private void InitializeUIState()
    {
        LoadResolutions();
      
    }

    private void UpdateUIWithCurrentSettings()
    {
        // Ensure SettingsManager instance is available
        if (SettingsManager.Instance != null)
        {
            // Update sliders with the values from SettingsManager
            masterVolumeSlider.value = SettingsManager.Instance.MasterVolume;
            musicVolumeSlider.value = SettingsManager.Instance.MusicVolume;
            sfxVolumeSlider.value = SettingsManager.Instance.SfxVolume;

            // Update fullscreen toggle
            fullscreenToggle.isOn = SettingsManager.Instance.IsFullScreen;

            // Ensure the resolutions array is populated before trying to access it
            if (resolutions == null || resolutions.Length == 0)
            {
                Debug.LogWarning("Resolutions array is not initialized. Make sure it's populated before updating UI.");
                LoadResolutions(); // Make sure this method populates the 'resolutions' array from Screen.resolutions
            }

            // Update resolution dropdown - find the current resolution index
            int currentResolutionIndex = Array.FindIndex(resolutions, r => r.width == SettingsManager.Instance.GameResolution.width && r.height == SettingsManager.Instance.GameResolution.height);
            if (currentResolutionIndex != -1) // Check if the current resolution was found in the array
            {
                resolutionDropdown.value = currentResolutionIndex;
            }
            else
            {
                Debug.LogWarning("Current resolution not found in resolutions array.");
            }

            // Update quality dropdown
            qualityDropdown.value = SettingsManager.Instance.QualityLevel;

            // Refresh dropdowns to display the current values
            resolutionDropdown.RefreshShownValue();
            qualityDropdown.RefreshShownValue();
        }
        else
        {
            Debug.LogWarning("SettingsManager instance not found. Make sure it's initialized before accessing it.");
        }
    }
 



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


    //#region Event Subscriptions
    ///// <summary>
    ///// Subscribes to necessary events on the event bus.
    ///// </summary>
    //private void SubscribeToEvents()
    //{
    //    if (GameManager.Instance != null)
    //    {
    //        GameManager.Instance.Subscribe<ShowOptionsMenuEvent>(OnShowOptionsMenu);
    //        GameManager.Instance.Subscribe<HideOptionsMenuEvent>(OnHideOptionsMenu);
    //    }
    //}

    ///// <summary>
    ///// Unsubscribes from events on the event bus.
    ///// </summary>
    //private void UnsubscribeFromEvents()
    //{
    //    if (GameManager.Instance != null)
    //    {
    //        GameManager.Instance.Unsubscribe<ShowOptionsMenuEvent>(OnShowOptionsMenu);
    //        GameManager.Instance.Unsubscribe<HideOptionsMenuEvent>(OnHideOptionsMenu);
    //    }
    //}
    //#endregion

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
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    /// <summary>
    /// Toggles the visibility of the options menu UI.
    /// </summary>
    /// <param name="isVisible">Whether the options menu should be visible.</param>
   
    #endregion
}