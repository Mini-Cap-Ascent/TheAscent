using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using UnityEditor;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor.Rendering;



public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{
    public static NetworkRunner runnerInstance;
    public string lobbyName = "default";
    public Transform[] spawnPoints;

    public Transform sessionListContentParent;
    public GameObject sessionListEntryPrefab;
    public Dictionary<string, GameObject> sessionListUiDictionary = new Dictionary<string, GameObject>();

    [SerializeField] private NetworkPrefabRef _playerPrefab;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
    public TMP_InputField roomNameInput;
    public GameObject lobbyUI;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        lobbyUI.SetActive(true);
        runnerInstance = gameObject.GetComponent<NetworkRunner>();

        if (runnerInstance == null)
        {
            runnerInstance = gameObject.AddComponent<NetworkRunner>();
        }
    }

    private void Start()
    {
        runnerInstance.JoinSessionLobby(SessionLobby.Shared, lobbyName);
    }

    public static void ReturnToLobby()
    {
        runnerInstance.Despawn(runnerInstance.GetPlayerObject(runnerInstance.LocalPlayer));
        runnerInstance.Shutdown(true, ShutdownReason.Ok);

    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void CreateSession()
    {
        string sessionName = roomNameInput.text;
        if (string.IsNullOrEmpty(sessionName))
        {
            Debug.LogError("Session name cannot be empty.");
            return;
        }

        Debug.Log($"Creating session as host: {sessionName}");

        StartGame(GameMode.Host, sessionName);

        GameObject newEntry = Instantiate(sessionListEntryPrefab, sessionListContentParent);
        SessionListEntry entryScript = newEntry.GetComponent<SessionListEntry>();
        entryScript.roomName.text = sessionName;
        entryScript.playerCount.text = "1/10"; // Assuming 1 player (the host) and a max of 10 players
        entryScript.joinButton.interactable = true;
        newEntry.SetActive(true);

        sessionListUiDictionary.Add(sessionName, newEntry);
    }

    async void StartGame(GameMode mode, string sessionName)
    {

        runnerInstance.ProvideInput = true;

        // Assuming the current scene is the GameScene, since everything is integrated now
        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();

        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        await runnerInstance.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = sessionName,
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });


        if (mode == GameMode.Host)
        {
            lobbyUI.SetActive(true); // Keep the lobby UI active for the host
        }

    }

    public void JoinSession(string sessionName)
    {
        Debug.Log($"Joining session as client: {sessionName}");

        StartGame(GameMode.Client, sessionName);
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"Player joined: {player}");
        if (runner.IsServer)
        {
            // Check if the player is the first player (host), usually PlayerRef=1 in Fusion
            if (player != runner.LocalPlayer)
            {
                // Spawn the player character
                NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, Vector3.zero, Quaternion.identity, player);
                _spawnedCharacters.Add(player, networkPlayerObject);
            }
            // Update the player count in the lobby UI
            UpdatePlayerCountInUI(runner.SessionInfo);
        }
    }

    private void UpdatePlayerCountInUI(SessionInfo sessionInfo)
    {
        if (sessionListUiDictionary.TryGetValue(sessionInfo.Name, out GameObject sessionEntry))
        {
            SessionListEntry entryScript = sessionEntry.GetComponent<SessionListEntry>();
            entryScript.playerCount.text = $"{sessionInfo.PlayerCount}/{sessionInfo.MaxPlayers}";
        }
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        Debug.Log($"Session list updated: {sessionList.Count} sessions");

        // Clear the existing UI entries
        foreach (Transform child in sessionListContentParent)
        {
            Destroy(child.gameObject);
        }

        // Create new UI entries for each session
        foreach (var session in sessionList)
        {
            Debug.Log($"Session: {session.Name}, PlayerCount: {session.PlayerCount}");

            GameObject newEntry = Instantiate(sessionListEntryPrefab, sessionListContentParent);
            SessionListEntry entryScript = newEntry.GetComponent<SessionListEntry>();
            entryScript.roomName.text = session.Name;
            entryScript.playerCount.text = $"{session.PlayerCount}/{session.MaxPlayers}";
            entryScript.joinButton.interactable = session.IsOpen && session.PlayerCount < session.MaxPlayers;
            newEntry.SetActive(true);

            // Add a listener to the join button
            entryScript.joinButton.onClick.AddListener(() => JoinSession(session.Name));
        }
    }
    private void CompareLists(List<SessionInfo> sessionList)
    {
        foreach (SessionInfo session in sessionList)
        {
            if (sessionListUiDictionary.ContainsKey(session.Name))
            {
                UpdateEntryUI(session);
            }
            else
            {
                CreateEntryUI(session);
                Debug.Log($"Added new entry to dictionary for session: {session.Name}");
            }
        }    
    }

    private void CreateEntryUI(SessionInfo session)
    {
        GameObject newEntry = GameObject.Instantiate(sessionListEntryPrefab);
        Debug.Log($"Created new entry for session: {session.Name}");


        newEntry.transform.SetParent(sessionListContentParent, false);
        SessionListEntry entryScript = newEntry.GetComponent<SessionListEntry>();
        sessionListUiDictionary.Add(session.Name, newEntry);

        entryScript.roomName.text = session.Name;
        entryScript.playerCount.text = session.PlayerCount.ToString() + "/" + session.MaxPlayers.ToString();
        entryScript.joinButton.interactable = session.IsOpen;

        newEntry.SetActive(session.IsVisible);
    }

    private void UpdateEntryUI(SessionInfo session)
    {
        sessionListUiDictionary.TryGetValue(session.Name, out GameObject newEntry);

        SessionListEntry entryScript = newEntry.GetComponent<SessionListEntry>();

        entryScript.roomName.text = session.Name;
        entryScript.playerCount.text = session.PlayerCount.ToString() + "/" + session.MaxPlayers.ToString();
        entryScript.joinButton.interactable = session.IsOpen;

        newEntry.SetActive(session.IsVisible);
    }

    private void DeleteOldSessionsFromUI(List<SessionInfo> sessionList)
    {
        bool isContained = false;
        GameObject uiToDelete = null;

        foreach (KeyValuePair<string, GameObject> kvp in sessionListUiDictionary)
        {
            string sessionKey = kvp.Key;

            foreach (SessionInfo sessionInfo in sessionList)
            {
                if (sessionInfo.Name == sessionKey)
                {
                    isContained = true;
                    break;
                }
            }

            if (!isContained)
            {
                uiToDelete = kvp.Value;
                sessionListUiDictionary.Remove(sessionKey);
                Destroy(uiToDelete);
            }
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();
        data.jumpPressed = Input.GetKeyDown(KeyCode.Space);
        data.attackPressed = Input.GetKeyDown(KeyCode.Keypad1);
        data.direction = new Vector2(
         Input.GetAxis("Horizontal"),
         Input.GetAxis("Vertical")
     );

        input.Set(data);
    }

    public void HideLobbyUI()
    {
        if (lobbyUI != null)
        {
            lobbyUI.SetActive(false);
        }
        else
        {
            Debug.LogError("Lobby UI GameObject is not assigned or is null.");
        }
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
    }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }



    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }


    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }




    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }
}
