using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static event GameEvent OnPause;
    public static event GameEvent OnResume;
    private DateTime _sessionStartTime;
    private DateTime _sessionEndTime;
    private Stack<IGameState> stateHistory = new Stack<IGameState>();
    public IGameState currentState;
    private List<SessionData> sessions = new List<SessionData>();

    public delegate void GameEvent();
    public static event GameEvent OnGameStart;
    public static event GameEvent OnGameEnd;


    
    private void Start()
    {
        _sessionStartTime = DateTime.Now;
        ChangeState(new MainMenuState(this));
        
        Debug.Log($"Game session start at: {_sessionStartTime}");

        EventManager.OnPlayPressed += StartGame;
        EventManager.OnExitPressed += EndGame;
    }

    public void OnDestroy()
    {
        EventManager.OnPlayPressed -= StartGame;
        EventManager.OnExitPressed -= EndGame;
    
    }

    private void OnApplicationQuit()
    {
        _sessionEndTime = DateTime.Now;
        TimeSpan timeDifference = _sessionEndTime.Subtract(_sessionStartTime);
        RecordSession();
        SaveSessionsToJson();
        Debug.Log($"Game session ended at: {_sessionEndTime}");
        Debug.Log($"Game session lasted: {timeDifference}");
    }

    public void ChangeState(IGameState newState)
    {
        currentState?.ExitState();
        currentState = newState;
        currentState.EnterState();
    }

    public void TransitionState(IGameState newState, bool isTemporaryTransition = false)
    {
        if (!isTemporaryTransition)
        {
            currentState?.ExitState();
        }
        else
        {
            stateHistory.Push(currentState);
        }

        currentState = newState;
        currentState.EnterState();
    }

    public void ResumePreviousState()
    {
        if (stateHistory.Count > 0)
        {
            currentState.ExitState();
            currentState = stateHistory.Pop();
            currentState.ResumeState();
        }
    }

    private void RecordSession()
    {
        TimeSpan sessionDuration = _sessionEndTime - _sessionStartTime;
        sessions.Add(new SessionData { startTime = _sessionStartTime, endTime = _sessionEndTime, duration = sessionDuration });
    }

    private void SaveSessionsToJson()
    {
        string json = JsonUtility.ToJson(new SessionList { sessions = sessions }, true);
        File.WriteAllText(Application.persistentDataPath + "/sessions.json", json);
    }

    public void StartGame()
    {
        ChangeState(new NextSceneState(this));
        OnGameStart?.Invoke();
       
    }

    public void EndGame()
    {
        OnGameEnd?.Invoke();
        Application.Quit();
    }

    public void PauseGame()
    {
        OnPause?.Invoke();
    }

    public void ResumeGame()
    {
        OnResume?.Invoke();
    }

    private void Update()
    {
        currentState?.UpdateState();

        if (Input.GetKeyDown(KeyCode.S))
        {
            StartGame();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            EndGame();
        }
    }
}

[Serializable]
public class SessionData
{
    public DateTime startTime;
    public DateTime endTime;
    public TimeSpan duration;
}

[Serializable]
public class SessionList
{
    public List<SessionData> sessions;
}