using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneState : IGameState
{
    private GameManager _gameManager;
    private int _nextSceneName = 1; // Replace with your actual scene name

    public NextSceneState(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void EnterState()
    {
        Debug.Log("Entering Next Scene State");
        SceneManager.LoadScene(_nextSceneName);
    }

    public void UpdateState()
    {
        Debug.Log("Updating Next Scene State");
    }

    public void ExitState()
    {
        Debug.Log("Exiting Next Scene State");
    }

    public void NextScene()
    {
        _gameManager.ChangeState(new NextSceneState(_gameManager));
    }


    public void ResumeState()
    {




    }   
}
