using UnityEngine;
//using static GameSessionManager;
//using static Player;


/// <summary>
/// Event to signal the showing of the Options Menu.
/// This can be used to pause the game or prepare the UI for user interaction.
/// </summary>
public struct ShowOptionsMenuEvent
{
    // Additional properties can be added here if there's specific data needed when showing the menu.
}

/// <summary>
/// Event to signal the hiding of the Options Menu.
/// This can be used to resume the game or clean up the UI after user interaction.
/// </summary>
public struct HideOptionsMenuEvent
{
    // Additional properties can be added here if there's specific data needed when hiding the menu.
}

/// <summary>
/// Event to notify changes in audio settings like master volume, music volume, and SFX volume.
/// </summary>
public struct AudioSettingsChangedEvent
{
    public float MasterVolume; ///< The master volume level, typically between 0.0 (mute) and 1.0 (max volume).
    public float MusicVolume;  ///< The music volume level, typically between 0.0 (mute) and 1.0 (max volume).
    public float SfxVolume;    ///< The SFX (Sound Effects) volume level, typically between 0.0 (mute) and 1.0 (max volume).
}

/// <summary>
/// Event to notify changes in the game's resolution and fullscreen mode.
/// </summary>
public struct ResolutionChangedEvent
{
    public int Width;         ///< The width of the resolution.
    public int Height;        ///< The height of the resolution.
    public bool IsFullScreen; ///< Indicates whether the game should be in fullscreen mode.
}


// Maybe we can bring the rest of the events to this file
/*
public struct SoulstoneUpdatedEvent
{
    public SoulstoneCache UpdatedStones { get; private set; }

    public SoulstoneUpdatedEvent(SoulstoneCache updatedStones)
    {
        UpdatedStones = updatedStones;
    }
}

public struct NewSessionAddedEvent
{
    public MatchData SessionData { get; }

    public NewSessionAddedEvent(MatchData sessionData)
    {
        SessionData = sessionData;
    }
}
*/

// Add any other game-specific events here
