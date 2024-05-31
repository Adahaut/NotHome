using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSpawnSystem : NetworkBehaviour
{
    [SerializeField] private GameObject _playerPrefab = null;

    private static List<Transform> _spawnPoints = new List<Transform>();

    public int playerCount;
    private int _nextIndex = 0;

    public static void AddSpawnPoint(Transform transform)
    {
        _spawnPoints.Add(transform);
        _spawnPoints = _spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
    }

    public static void RemoveSpawnPoint(Transform transform) => _spawnPoints.Remove(transform);

    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkLobbyManager.OnServerReadied += StartSpawningPlayers;
    }

    [ServerCallback]
    private void OnDestroy() => NetworkLobbyManager.OnServerReadied -= StartSpawningPlayers;

    [Server]
    private void StartSpawningPlayers(NetworkConnection conn)
    {
        StartCoroutine(WaitForSpawnPointsAndSpawn(conn));
    }

    [Server]
    private IEnumerator WaitForSpawnPointsAndSpawn(NetworkConnection conn)
    {
        // Attendre que les points de spawn soient enregistrés
        while (_spawnPoints.Count == 0)
        {
            yield return null;
        }

        SpawnPlayer(conn);
    }

    [Server]
    public void SpawnPlayer(NetworkConnection conn)
    {
        Transform spawnPoint = _spawnPoints.ElementAtOrDefault(_nextIndex);

        if (spawnPoint == null)
        {
            Debug.LogError($"Missing spawn point for player {_nextIndex}");
            return;
        }

        GameObject playerInstance = Instantiate(_playerPrefab, spawnPoint.position, spawnPoint.rotation);
        NetworkServer.Spawn(playerInstance, conn);

        playerCount++;
        _nextIndex++;
    }
}