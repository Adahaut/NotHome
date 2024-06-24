using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnerManager : MonoBehaviour
{
    public List<List<SpawnItem>> _spawnItems = new List<List<SpawnItem>>();
    public List<SpawnItem> _desertSpawnItems = new List<SpawnItem>();
    public List<SpawnItem> _mountainSpawnItems = new List<SpawnItem>();
    public List<SpawnItem> _forestSpawnItems = new List<SpawnItem>();
    public bool _canSpawn;
    public bool _desertSpawn;
    public bool _mountainSpawn;
    public bool _forestSpawn;


    private void Start()
    {
        _spawnItems.Add(_desertSpawnItems);
        _spawnItems.Add(_mountainSpawnItems);
        _spawnItems.Add(_forestSpawnItems);
    }

    [Server]
    public void DestroyAndSpawnItems(int _zone)
    {
        if (!_canSpawn)
            return;

        for (int i = 0; i < _spawnItems.Count - 1; i++)
        {
            print("iteration " + i);
            _spawnItems[_zone][i].DestroyAndSpawn();
        }
        print("avant _canSpawn = false");
        _canSpawn = false;
        print("fin DestroyAndSpawnItems");
    }

    [Server]
    public void SpawnItems(int _zone)
    {
        if (!_canSpawn)
            return;

        for (int i = 0; i < _spawnItems.Count; i++)
        {
            _spawnItems[_zone][i].Spawn();
        }
        _canSpawn = false;
    }

    public void DestroyAllItems(int _zone)
    {
        for (int i = 0; i < _spawnItems.Count; i++)
        {
            _spawnItems[_zone][i].DeleteResources();
        }
    }
}
