using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static event GameEvent OnGameStart;
    public static event GameEvent OnGameEnd;
    public delegate void GameEvent();
    private DateTime _sessionStartTime; 
    private DateTime _sessionEndTime;
    private Stack<IGameState> stateHistory = new Stack<IGameState>();
    public IGameState currentState;
    private List<SessionData> sessions = new List<SessionData>();
    public Canvas canvas;

    public bool IsPaused { get; set; }

    public static new GameManager Instance { get; private set; }

    [SerializeField] private GameObject optionsMenu;

    private new void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        _sessionStartTime = DateTime.Now;
        ChangeState(new MainMenuState(this));

        Debug.Log($"Game session start at: {_sessionStartTime}");

        EventBus.Instance.Subscribe<PlayPressedEvent>(e => StartGame());
        EventBus.Instance.Subscribe<ExitPressedEvent>(e => EndGame());
        EventBus.Instance.Subscribe<PauseGameEvent>(e => PauseGame());
        EventBus.Instance.Subscribe<ResumeGameEvent>(e => ResumeGame());
    }

    private void OnDestroy()
    {
        EventBus.Instance.Unsubscribe<PlayPressedEvent>(e => StartGame());
        EventBus.Instance.Unsubscribe<ExitPressedEvent>(e => EndGame());
        EventBus.Instance.Unsubscribe<PauseGameEvent>(e => PauseGame());
        EventBus.Instance.Unsubscribe<ResumeGameEvent>(e => ResumeGame());
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
    public void ToggleOptionsMenu()
    {

        EventBus.Instance.Publish(new ShowOptionsMenuEvent());

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
        // Start the game or transition to the gameplay scene
        ChangeState(new NextSceneState(this));
        OnGameStart?.Invoke();
        EventBus.Instance.Publish(new PlayPressedEvent()); // Publish the event
    }

    public void EndGame()
    {
        // Exit the game or return to the main menu
        OnGameEnd?.Invoke();
        Application.Quit();
        EventBus.Instance.Publish(new ExitPressedEvent()); // Publish the event
    }

    public void PauseGame()
    {
        IsPaused = true;
        Time.timeScale = 0f;
        EventBus.Instance.Publish(new PauseGameEvent()); // Publish the event
    }

    public void ResumeGame()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        EventBus.Instance.Publish(new ResumeGameEvent()); // Publish the event
    }
    private void Update()
    {
        //currentState?.UpdateState();

        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    StartGame();
        //}
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    EndGame();
        //}
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