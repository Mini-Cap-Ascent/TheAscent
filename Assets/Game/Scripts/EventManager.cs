using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    // Define your events here
    public static event Action OnPlayPressed;
    public static event Action OnExitPressed;
    public static event Action OnShowOptionsMenu;
    public static event Action<float, float, float> OnAudioSettingsChanged;
    public static event Action<int> OnQualityLevelChanged;
    public static event Action<int, int, bool> OnResolutionChanged;
    public static event Action<bool> OnFullscreenToggled;

    public static event Action OnPauseGame;
    public static event Action OnResumeGame;

    // Define trigger methods for each event
    public static void TriggerAudioSettingsChanged(float master, float music, float sfx) => OnAudioSettingsChanged?.Invoke(master, music, sfx);
    public static void TriggerQualityLevelChanged(int qualityLevel) => OnQualityLevelChanged?.Invoke(qualityLevel);
    public static void TriggerResolutionChanged(int width, int height, bool fullscreen) => OnResolutionChanged?.Invoke(width, height, fullscreen);
    public static void TriggerFullscreenToggled(bool isFullscreen) => OnFullscreenToggled?.Invoke(isFullscreen);

    public static void TriggerShowOptionsMenu() => OnShowOptionsMenu?.Invoke();

    // Methods to trigger events
    public static void TriggerPlayPressed() => OnPlayPressed?.Invoke();
    public static void TriggerExitPressed() => OnExitPressed?.Invoke();


    public static void TriggerPauseGame() => OnPauseGame?.Invoke();
    public static void TriggerResumeGame() => OnResumeGame?.Invoke();

    internal static void TriggerHideOptionsMenu()
    {
        throw new NotImplementedException();
    }
}