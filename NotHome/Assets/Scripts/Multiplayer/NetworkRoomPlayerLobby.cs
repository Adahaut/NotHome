using Mirror;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class NetworkRoomPlayerLobby : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _lobbyUI = null;
    [SerializeField] private RawImage[] _playerImages = new RawImage[4];
    [SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[4];
    [SerializeField] private TMP_Text[] playerReadyTexts = new TMP_Text[4];
    [SerializeField] private Button startGameButton = null;
    [SerializeField] private Button readyButton = null;
    [SerializeField] private Button[] _leaveKickButtons = new Button[4];
    [SerializeField] private GameObject uiMainMenu;
   
    [SyncVar(hook = nameof(HandleSteamIdUpdated))]
    private ulong steamId;

    [SyncVar(hook = nameof(HandleDisplayNameChanged))]
    public string _displayName = "Loading...";
    [SyncVar(hook = nameof(HandleReadyStatusChanged))]
    public bool _isReady = false;

    public Texture2D _displayImage;
    public Texture2D _waitImage;

    protected Callback<AvatarImageLoaded_t> _avatarImageLoaded;

    public GameObject loading;


    private bool _isLeader;
    public bool IsLeader
    {
        set
        {
            _isLeader = value;
            startGameButton.gameObject.SetActive(value);
        }
    }

    private NetworkLobbyManager room;
    public NetworkLobbyManager Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as NetworkLobbyManager;
        }
    }

    
    public override void OnStartAuthority()
    {
        readyButton.gameObject.SetActive(true);
        _lobbyUI.SetActive(true);
    }

    public override void OnStartClient()
    {
        _avatarImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnAvatarImageLoaded);

        Room._roomPlayers.Add(this);
        Room.NotifyPlayersOfReadyState();

    }

    public override void OnStopClient()
    {
        Room._roomPlayers.Remove(this);
    }

    public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();
    public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();
    public void HandleSteamIdUpdated(ulong oldSteamId, ulong newSteamId)
    {
        var cSteamId = new CSteamID(newSteamId);

        CmdSetDisplayName(SteamFriends.GetFriendPersonaName(cSteamId));

        int imageId = SteamFriends.GetLargeFriendAvatar(cSteamId);


        if (imageId == -1) { return; }

        _displayImage = GetSteamImageAsTexture(imageId);


    }

    private Texture2D GetSteamImageAsTexture(int iImage)
    {
        Texture2D texture = null;

        bool isValid = SteamUtils.GetImageSize(iImage, out uint width, out uint height);

        if (isValid)
        {
            byte[] image = new byte[width * height * 4];

            isValid = SteamUtils.GetImageRGBA(iImage, image, (int)(width * height * 4));

            if(isValid)
            {
                texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false, true);
                texture.LoadRawTextureData(image);
                texture.Apply();
            }
        }

        return texture;
    }

    private void OnAvatarImageLoaded(AvatarImageLoaded_t callback)
    {
        if(callback.m_steamID.m_SteamID != steamId) { return; }

        _displayImage = GetSteamImageAsTexture(callback.m_iImage);
    }

    public void SetSteamId(ulong steamId)
    {
        this.steamId = steamId;
    }

    public void UpdateDisplay()
    {
        for (int i = 0; i < Room._roomPlayers.Count; i++)
        {
            if (i >= playerNameTexts.Length)
            {
                break;
            }

            playerNameTexts[i].text = Room._roomPlayers[i]._displayName;
            if (i == 0)
                playerNameTexts[i].text += " (host)";
            playerReadyTexts[i].text = Room._roomPlayers[i]._isReady ?
                "<color=green>Ready</color>" :
                "<color=red>Not Ready</color>";

            _playerImages[i].texture = Room._roomPlayers[i]._displayImage;


            //if (Room._roomPlayers[i] == this && isOwned)
            //{
            //    _leaveKickButtons[i].gameObject.SetActive(true);
            //    _leaveKickButtons[i].GetComponentInChildren<TMP_Text>().text = "Leave";
            //}

            //if (_isLeader)
            //{
            //    if (Room._roomPlayers[i] != this && isOwned)
            //    {
            //        _leaveKickButtons[i].gameObject.SetActive(true);
            //        _leaveKickButtons[i].GetComponentInChildren<TMP_Text>().text = "Kick";
            //    }
            //}

        }

        for (int j = Room._roomPlayers.Count; j < playerNameTexts.Length; j++)
        {
            playerNameTexts[j].text = "Waiting For Player...";
            playerReadyTexts[j].text = string.Empty;
            _playerImages[j].texture = _waitImage;
        }
    }

    [Command]
    public void LeaveLobby()
    {
        if (NetworkClient.isConnected)
        {
            if (NetworkServer.active)
            {
                // Si le joueur est l'hôte, arrêter le serveur
                NetworkManager.singleton.StopHost();
            }
            else
            {
                // Si le joueur est un client, arrêter le client
                NetworkManager.singleton.StopClient();
            }
        }

        // Charger la scène de menu principal
        SceneManager.LoadScene("Scene_Lobby");
    
}

    public void HandleReadyToStart(bool readyToStart)
    {
        if(!_isLeader) { return; }
        startGameButton.interactable = readyToStart;
    }

    [Command]
    private void CmdSetDisplayName(string displayName)
    {
        _displayName = displayName;
    }

    [Command]
    public void CmdReadyUp()
    {
        _isReady = !_isReady;
        Room.NotifyPlayersOfReadyState();
    }

    [Command]
    public void CmdStartGame()
    {
        if (Room._roomPlayers[0].connectionToClient != connectionToClient) { return; }

        loading.SetActive(true);
        Room.StartGame();
    }
}
