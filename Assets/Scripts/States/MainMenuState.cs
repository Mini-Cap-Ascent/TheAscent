using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuState : MonoBehaviour, IGameState  
{
    private GameManager _gameManager;

    void Awake()
    {
        // Find the GameManager instance in the scene and assign it
        _gameManager = FindObjectOfType<GameManager>();
    }

    public MainMenuState(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void EnterState()
    {
        Debug.Log("Entering Main Menu State");
    }

    public void UpdateState()
    {
        Debug.Log("Updating Main Menu State");
    }

    public void ExitState()
    {
        Debug.Log("Exiting Main Menu State");
        Application.Quit();
    }

    public void PlayGame()
    {
        _gameManager.ChangeState(new NextSceneState(_gameManager));
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ResumeState()
    {
        
    }
    

    // You can call these methods from the UI button's OnClick() event.
    public void OnPlayButtonClicked()
    {
        _gameManager.ChangeState(new NextSceneState(_gameManager));
    }

 
}




