using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkRoomPlayerLobby : NetworkBehaviour
{

    [Header("UI")]
    [SerializeField] private GameObject _lobbyUI = null;
    [SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[4];
    [SerializeField] private TMP_Text[] playerReadyTexts = new TMP_Text[4];
    [SerializeField] private Button startGameButton = null;

    [SyncVar(hook = nameof(HandleDisplayNameChanged))]
    public string _displayName = "Loading...";
    [SyncVar(hook = nameof(HandleReadyStatusChanged))]
    public bool _isReady = false;

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
        CmdSetDisplayName(PlayerNameInput.DisplayName);

        _lobbyUI.SetActive(true);
    }

    public override void OnStartClient()
    {
        Room._roomPlayers.Add(this);

        UpdateDisplay();
    }

    public override void OnStopClient()
    {
        Room._roomPlayers.Remove(this);

        UpdateDisplay();
    }

    public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();
    public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();

    private void UpdateDisplay()
    {
        if(!isOwned)
        {
            foreach(var player in Room._roomPlayers)
            {
                if(player.isOwned)
                {
                    player.UpdateDisplay();
                    break;
                }
            }

            return;
        }

        for (int i = 0; i < playerNameTexts.Length; i++)
        {
            playerNameTexts[i].text = "Waiting For Player...";
            playerReadyTexts[i].text = string.Empty;
        }

        for(int i = 0; i < Room._roomPlayers.Count; i++)
        {
            playerNameTexts[i].text = Room._roomPlayers[i]._displayName;
            if(i == 0)
                playerNameTexts[i].text += " (host)";
            playerReadyTexts[i].text = Room._roomPlayers[i]._isReady ?
                "<color=green>Ready</color>" :
                "<color=red>Not Ready</color>";
        }
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

        Room.StartGame();
    }
}
