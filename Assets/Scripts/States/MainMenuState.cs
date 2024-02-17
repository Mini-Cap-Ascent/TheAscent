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
        Debug.Log("Entering Main Menu State");
        // Show main menu UI here
    }

    public void UpdateState()
    {
        // Handle main menu logic, such as button clicks to start the game
    }

    public void ExitState()
    {
        Debug.Log("Exiting Main Menu State");
        // Hide main menu UI here
    }

    public void ResumeState()
    {
        Debug.Log("Resuming Main Menu State");
        // Show main menu UI again if coming back from another state
    }
}

