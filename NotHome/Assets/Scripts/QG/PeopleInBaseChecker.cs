using UnityEngine;

public class PeopleInBaseChecker : MonoBehaviour
{
    public int _numberOfPlayers;
    public int _numberOfPlayersInBase;
    public NetworkLobbyManager _lobbyManager;

    private void Start()
    {
        _lobbyManager = GameObject.Find("NetworkManager").GetComponent<NetworkLobbyManager>();
        _numberOfPlayers = _lobbyManager._gamePlayers.Count;
        _numberOfPlayersInBase = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _numberOfPlayersInBase++;
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _numberOfPlayersInBase--;
        }
            
    }

    public bool Check() { return _numberOfPlayersInBase == _numberOfPlayers;}

}
