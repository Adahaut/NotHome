using Mirror;
using Steamworks;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class SteamLobby : MonoBehaviour
{
    [SerializeField] private GameObject _landingPagePanel = null;

    protected Callback<LobbyCreated_t> _lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> _gameLobbyJoinRequested;
    protected Callback<LobbyEnter_t> _lobbyEntered;

    private const string _hostAdressKey = "HostAdress";

    private NetworkManager _networkManager;

    public static CSteamID _lobbyId {  get; private set; }

    private void Start()
    {
        _networkManager = GetComponent<NetworkManager>();

        if(!SteamManager.Initialized) { return; }

        _lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        _gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        _lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);

    }

    public void HostLobby()
    {
        _landingPagePanel?.SetActive(false);

        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, _networkManager.maxConnections);
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {

        if (callback.m_eResult != EResult.k_EResultOK)
        {
            _landingPagePanel?.SetActive(true);
            return; 
        }

        _lobbyId = new CSteamID(callback.m_ulSteamIDLobby);

        _networkManager.StartHost();

        SteamMatchmaking.SetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby),
            _hostAdressKey,
            SteamUser.GetSteamID().ToString());
    }

    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        if(NetworkServer.active) { return; }

        string hostAdress = SteamMatchmaking.GetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby),
            _hostAdressKey);

        _networkManager.networkAddress = hostAdress;
        
        _networkManager.StartClient();

        _landingPagePanel?.SetActive(false);
    }
}
