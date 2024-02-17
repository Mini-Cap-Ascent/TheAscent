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
    }

    public void UpdateState()
    {
    }

    public void ExitState()
    {
    }
    public void ResumeState()
    {
    }
}
