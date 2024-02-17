using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuState : IGameState
{
    private GameManager _gameManager;

    public MainMenuState(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void EnterState()
    {
        // Setup MainMenu state
    }

    public void UpdateState()
    {
        // Handle MainMenu logic
    }

    public void ExitState()
    {
        // Cleanup MainMenu state
    }

    public void ResumeState()
    {
        // Logic to resume MainMenu state
    }
}

