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