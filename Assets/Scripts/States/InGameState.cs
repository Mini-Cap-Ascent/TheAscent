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
        // Initialize InGame state
    }

    public void UpdateState()
    {
        // Handle InGame logic
    }

    public void ExitState()
    {
        // Cleanup InGame state
    }

    public void ResumeState()
    {
        // Logic to resume the InGame state
    }
}