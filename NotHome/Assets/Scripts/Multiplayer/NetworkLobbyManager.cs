using Mirror;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct DisconnectMessage : NetworkMessage { }

public class NetworkLobbyManager : NetworkManager
{
    [SerializeField] private int _minPlayer = 2;
    public GameObject _uiMainMenu;
    [Scene] [SerializeField] private string menuScene;

    [Header("Room")]
    [SerializeField] private NetworkRoomPlayerLobby roomPlayerPrefab = null;

    [Header("Game")]
    [SerializeField] private NetworkGamePlayerLobby _gamePlayerPrefab = null;
    [SerializeField] private GameObject _playerSpawnSystem = null;

    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;
    public static event Action<NetworkConnection> OnServerReadied;

    public List<NetworkRoomPlayerLobby> _roomPlayers { get; } = new List<NetworkRoomPlayerLobby>();
    public List<NetworkGamePlayerLobby> _gamePlayers { get; } = new List<NetworkGamePlayerLobby>();


    GameObject playerSpawnSystemInstance = null;


    #region UnityFunctions

    private void Update()
    {
        foreach (var player in _roomPlayers)
        {
            player.UpdateDisplay();
        }
    }

    #endregion

    #region Server

    public override void OnStartServer()
    {
        spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        if (numPlayers >= maxConnections)
        {
            conn.Disconnect();
            return;
        }

        if (SceneManager.GetActiveScene().path != menuScene && SceneManager.GetActiveScene().name.StartsWith("Scene_Map"))
        {
            //Allow connection
            return;
        }

        if (SceneManager.GetActiveScene().path != menuScene)
        {
            conn.Disconnect();
            return;
        }
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (SceneManager.GetActiveScene().path == menuScene)
        {
            bool isLeader = _roomPlayers.Count == 0;

            NetworkRoomPlayerLobby roomPlayerInstance = Instantiate(roomPlayerPrefab);
            roomPlayerInstance.IsLeader = isLeader;
            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
        }
        //else
        //{
        //    var gamePlayerInstance = Instantiate(_gamePlayerPrefab);
        //    NetworkServer.Spawn(gamePlayerInstance.gameObject);
        //    NetworkServer.AddPlayerForConnection(conn, gamePlayerInstance.gameObject);

        //    if (playerSpawnSystemInstance != null)
        //    {
        //        playerSpawnSystemInstance.GetComponent<PlayerSpawnSystem>().SpawnPlayerFromNewConnection(conn);
        //    }

        //    _gamePlayers.Add(gamePlayerInstance);
        //}

        CSteamID steamId = SteamMatchmaking.GetLobbyMemberByIndex(
            SteamLobby._lobbyId,
            numPlayers - 1);

        var playerInfosDisplay = conn.identity.GetComponent<NetworkRoomPlayerLobby>();

        if (playerInfosDisplay != null) 
            playerInfosDisplay.SetSteamId(steamId.m_SteamID);
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        if(SceneManager.GetActiveScene().path == menuScene)
        {
            if (conn.identity != null)
            {
                var player = conn.identity.GetComponent<NetworkRoomPlayerLobby>();
                if (player != null)
                {
                    _roomPlayers.Remove(player);
                }

                var gamePlayer = conn.identity.GetComponent<NetworkGamePlayerLobby>();
                if (gamePlayer != null)
                {
                    _gamePlayers.Remove(gamePlayer);
                }

                NotifyPlayersOfReadyState();
            }
        }
        else
        {
            InformClientsToDisconnect();
        }

        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer()
    {
        _roomPlayers.Clear();
        _gamePlayers.Clear();
    }
    void InformClientsToDisconnect()
    {
        DisconnectMessage msg = new DisconnectMessage();
        NetworkServer.SendToAll(msg);
    }

    public override void ServerChangeScene(string newSceneName)
    {

        if (SceneManager.GetActiveScene().path == menuScene && newSceneName.StartsWith("Scene_Map"))
        {
            for (int i = _roomPlayers.Count - 1; i >= 0; i--)
            {
                var conn = _roomPlayers[i].connectionToClient;

                var gamePlayerInstance = Instantiate(_gamePlayerPrefab);
                gamePlayerInstance.SetDisplayName(_roomPlayers[i]._displayName);
                

                NetworkServer.ReplacePlayerForConnection(conn, gamePlayerInstance.gameObject);
            }
        }

        base.ServerChangeScene(newSceneName);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if (sceneName.StartsWith("Scene_Map") && !GameObject.FindAnyObjectByType<PlayerSpawnSystem>())
        {
            playerSpawnSystemInstance = Instantiate(_playerSpawnSystem);
            NetworkServer.Spawn(playerSpawnSystemInstance);
        }
    }

    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        base.OnServerReady(conn);

        OnServerReadied?.Invoke(conn);
    }


    #endregion

    #region Client

    public override void OnStartClient()
    {
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

        foreach (var prefab in spawnablePrefabs)
        {
            NetworkClient.RegisterPrefab(prefab);
        }
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();

        OnClientConnected?.Invoke();
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();

        OnClientDisconnected?.Invoke();
    }

    #endregion


    public void NotifyPlayersOfReadyState()
    {
        foreach(var player in _roomPlayers)
        {
            player.HandleReadyToStart(IsReadyToStart());
        }
    }

    private bool IsReadyToStart()
    {
        if (numPlayers < _minPlayer) { return false; }

        foreach (var player in _roomPlayers)
        {
            if (!player._isReady) { return false; }
        }

        return true;
    }

    public void StartGame()
    {
        if(SceneManager.GetActiveScene().path == menuScene)
        {
            if(!IsReadyToStart()) { return; }

            ServerChangeScene("Scene_Map_02");
        }
    }


}
