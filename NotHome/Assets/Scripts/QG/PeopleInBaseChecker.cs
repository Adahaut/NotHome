using UnityEngine;

public class PeopleInBaseChecker : MonoBehaviour
{
    public int _numberOfPlayers;
    public int _numberOfPlayersInBase;
    public NetworkLobbyManager _lobbyManager;
    public ItemSpawnerManager _spawnerManager;

    private void Start()
    {
        _lobbyManager = GameObject.Find("NetworkManager").GetComponent<NetworkLobbyManager>();
        if(_lobbyManager != null )
        {
            _numberOfPlayers = _lobbyManager._gamePlayers.Count;
        }
        else
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            _numberOfPlayers = players.Length;
        }
        
        _numberOfPlayersInBase = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _numberOfPlayersInBase++;
            if (_numberOfPlayersInBase == _numberOfPlayers)
                _spawnerManager.SetAllBoolFalse();
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
