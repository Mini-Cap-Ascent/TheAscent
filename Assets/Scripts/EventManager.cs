using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    // Define your events here
    public static event Action OnPlayPressed;
    public static event Action OnExitPressed;

    // Methods to trigger events
    public static void TriggerPlayPressed() => OnPlayPressed?.Invoke();
    public static void TriggerExitPressed() => OnExitPressed?.Invoke();
}