using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{

    [SerializeField] private NetworkPrefabRef _playerPrefab;
    [SerializeField] private NetworkPrefabRef _weaponPrefab; // Assign this in the inspector
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
    [SerializeField] private Transform[] _weaponSpawnPoints;

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            // Check if the player is the first player (host), usually PlayerRef=1 in Fusion
            if (player != runner.LocalPlayer)
            {
                // Spawn the player character
                NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, Vector3.zero, Quaternion.identity, player);
                _spawnedCharacters.Add(player, networkPlayerObject);
            }
        }
    }


    //private void EquipPlayerWithWeapon(NetworkRunner runner, NetworkObject playerObject)
    //{
    //    // Check if the playerObject has a WeaponManager component
    //    if (playerObject.TryGetComponent<WeaponManager>(out var weaponManager))
    //    {
    //        // Determine the weapon name to equip
    //        string weaponName = "Sword"; // Example default weapon name

    //        // Use the EquipWeaponByName method to equip the weapon to the player
    //        // Make sure the weaponName matches one of the keys in the WeaponManager's weaponPrefabs dictionary
    //        weaponManager.EquipWeaponByName(weaponName, playerObject);
    //    }
    //}

    // ... other methods ...

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) 
    {
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }
    public void OnInput(NetworkRunner runner, NetworkInput input) 
    {
        var data = new NetworkInputData();
        data.jumpPressed = Input.GetKeyDown(KeyCode.Space);
        data.direction = new Vector2(
         Input.GetAxis("Horizontal"),
         Input.GetAxis("Vertical")
     );

        input.Set(data);
    }

    public async void CreateAndHostRoom(string roomName)
    {
        if (_runner == null)
        {
            _runner = gameObject.AddComponent<NetworkRunner>();
            _runner.ProvideInput = true;

            var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);

            await _runner.StartGame(new StartGameArgs
            {
                GameMode = GameMode.Host,
                SessionName = roomName,
                Scene = scene,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
                // You may want to set additional StartGameArgs properties as needed
            });

            // The rest of your networking code to set up the room...
        }
        else
        {
            Debug.LogWarning("NetworkRunner already exists, cannot create another room.");
        }
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner runner) {
        SpawnWeapons(runner);

        // Activate all weapon spawn points
        foreach (var spawnPoint in _weaponSpawnPoints)
        {
            spawnPoint.gameObject.SetActive(true);
        }
    }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }

    private NetworkRunner _runner;

    async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        // Create the NetworkSceneInfo from the current scene
        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }
    public void SpawnWeapons(NetworkRunner runner)
    {
        if (runner.IsServer)
        {
            // Use the assigned spawn points from the inspector
            foreach (var spawnPoint in _weaponSpawnPoints)
            {
                // Check if there's no weapon already spawned here
                if (spawnPoint.childCount == 0)
                {
                    runner.Spawn(_weaponPrefab, spawnPoint.position, Quaternion.identity, null);
                }
            }
        }
    }

    private void OnGUI()
    {
        if (_runner == null)
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
            {
                StartGame(GameMode.Host);
            }
            if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
            {
                StartGame(GameMode.Client);
            }
        }
    }
}