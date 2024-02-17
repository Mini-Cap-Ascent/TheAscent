using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverState : IGameState
{
    private GameManager _gameManager;

    public GameOverState(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void EnterState()
    {
        // Initialize GameOver state
    }

    public void UpdateState()
    {
        // Handle GameOver logic
    }

    public void ExitState()
    {
        // Cleanup GameOver state
    }
    public void ResumeState()
    {
        // Logic to resume MainMenu state
    }
}
