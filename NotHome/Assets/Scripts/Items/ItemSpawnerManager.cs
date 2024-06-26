using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnerManager : MonoBehaviour
{
    public List<SpawnItem> _spawnItems = new List<SpawnItem>();
    public bool _canSpawn;

    [Server]
    public void DestroyAndSpawnItems()
    {
        if (!_canSpawn)
            return;

        for (int i = 0; i < _spawnItems.Count; i++)
        {
            _spawnItems[i].DestroyAndSpawn();
        }
        _canSpawn = false;
    }

    [Server]
    public void SpawnItems()
    {
        if (!_canSpawn)
            return;

        for (int i = 0; i < _spawnItems.Count; i++)
        {
            _spawnItems[i].Spawn();
        }
        _canSpawn = false;
    }

    public void DestroyAllItems()
    {
        for (int i = 0; i < _spawnItems.Count; i++)
        {
            _spawnItems[i].DeleteResources();
        }
    }

    public void DestroyAll()
    {
        DestroyAllItems();
    }
}
