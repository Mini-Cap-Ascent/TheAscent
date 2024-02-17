using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausedState : IGameState
{
    private GameManager _gameManager;

    public PausedState(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void EnterState()
    {
        // Initialize Paused state
    }

    public void UpdateState()
    {
        // Handle Paused logic
    }

    public void ExitState()
    {
        // Cleanup Paused state
    }
    public void ResumeState()
    {
        // Logic to resume MainMenu state
    }
}