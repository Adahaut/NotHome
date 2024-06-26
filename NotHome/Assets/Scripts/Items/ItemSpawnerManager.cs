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

    public void SetAllBoolFalse()
    {
        _desertSpawn = false;
        _mountainSpawn = false;
        _forestSpawn = false;
}

    [Server]
    public void DestroyAndSpawnItems(int _zone)
    {
        if (!_canSpawn)
            return;

        for (int i = 0; i < _spawnItems[_zone].Count; i++)
        {
            _spawnItems[_zone][i].DestroyAndSpawn();
        }
        //_canSpawn = false;
    }

    public bool NoZoneSpawned() { return !_desertSpawn && !_mountainSpawn && !_forestSpawn; }

    [Server]
    public void SpawnItems(int _zone)
    {
        if (!_canSpawn)
            return;

        for (int i = 0; i < _spawnItems.Count; i++)
        {
            _spawnItems[_zone][i].Spawn();
        }
        //_canSpawn = false;
    }

    public void DestroyAllItems(int _zone)
    {
        for (int i = 0; i < _spawnItems[_zone].Count; i++)
        {
            _spawnItems[_zone][i].DeleteResources();
        }
    }

    public void DestroyAll()
    {
        for(int i = 0; i < _spawnItems.Count; i++)
        {
            DestroyAllItems(i);
        }
    }
}
