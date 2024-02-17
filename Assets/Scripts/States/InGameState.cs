using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameState : IGameState
{
    private GameManager _gameManager;

    public InGameState(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void EnterState()
    {
        Debug.Log("Entering In-Game State");
        // Initialize gameplay elements here
    }

    public void UpdateState()
    {
        // Handle gameplay updates, such as player movement and game logic
    }

    public void ExitState()
    {
        Debug.Log("Exiting In-Game State");
        // Clean up gameplay elements here
    }

    public void ResumeState()
    {
        Debug.Log("Resuming In-Game State");
        // Resume gameplay if coming back from pause state
    }
}
