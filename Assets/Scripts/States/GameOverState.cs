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
        Debug.Log("Entering Game Over State");
        // Show game over UI here
    }

    public void UpdateState()
    {
        // Handle game over logic, such as button clicks to restart the game
    }

    public void ExitState()
    {
        Debug.Log("Exiting Game Over State");
        // Hide game over UI here
    }

    public void ResumeState()
    {
        Debug.Log("Resuming Game Over State");
        // Show game over UI again if coming back from another state
    }
}
