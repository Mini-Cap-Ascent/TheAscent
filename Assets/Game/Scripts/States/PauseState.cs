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
        Debug.Log("Entering Paused State");
        // Show pause menu UI here
    }

    public void UpdateState()
    {
        // Handle pause menu logic, such as button clicks to resume the game
    }

    public void ExitState()
    {
        Debug.Log("Exiting Paused State");
        // Hide pause menu UI here
    }

    public void ResumeState()
    {
        Debug.Log("Resuming Paused State");
        // Show pause menu UI again if coming back from another state
    }
}
