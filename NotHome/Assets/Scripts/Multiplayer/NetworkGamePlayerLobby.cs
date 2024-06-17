using Mirror;
using Steamworks;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine.SceneManagement;

public class NetworkGamePlayerLobby : NetworkBehaviour
{

    [SyncVar]
    private string _displayName = "Loading...";

    [SyncVar(hook = nameof(HandleSteamIdUpdated))]
    private ulong steamId;

    private NetworkLobbyManager room;
    public NetworkLobbyManager Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as NetworkLobbyManager;
        }
    }

    public string GetDisplayName() { return _displayName; }

    public void HandleSteamIdUpdated(ulong oldSteamId, ulong newSteamId)
    {
        var cSteamId = new CSteamID(newSteamId);

        CmdSetDisplayName(SteamFriends.GetFriendPersonaName(cSteamId));
    }

    [Command]
    private void CmdSetDisplayName(string displayName)
    {
        _displayName = displayName;
    }

    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);

        Room._gamePlayers.Add(this);
    }

    public override void OnStopClient()
    {
        Room._gamePlayers.Remove(this);
    }

    [Server]
    public void SetDisplayName(string displayName)
    {
        this._displayName = displayName;
    }

    public void SetSteamId(ulong steamId)
    {
        this.steamId = steamId;
    }
}
