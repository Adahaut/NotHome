using Mirror;
using Steamworks;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine.SceneManagement;

public class NetworkGamePlayerLobby : NetworkBehaviour
{

    [SyncVar]
    private string _displayName = "Loading...";

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


    [Command]
    private void CmdSetDisplayName(string displayName)
    {
        _displayName = displayName;
    }

    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);

        Room._gamePlayers.Add(this);
        if(SteamManager.Initialized)
        {
            SteamFriends.SetRichPresence("status", "InLobby");
            print("steam manager initialized");
            SteamFriends.ClearRichPresence();
        }
        
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
}
