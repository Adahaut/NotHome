using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnerManager : MonoBehaviour
{
    [SerializeField] private List<SpawnItem> _spawnItems = new List<SpawnItem>();


    public void DestroyAndSpawnItems()
    {
        for(int i = 0; i < _spawnItems.Count; i++)
        {
            _spawnItems[i].DestroyAndSpawn();
        }
    }

    public void DestroyAllItems()
    {
        for (int i = 0; i < _spawnItems.Count; i++)
        {
            _spawnItems[i].DeleteResources();
        }
    }
}
