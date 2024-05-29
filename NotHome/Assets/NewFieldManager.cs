using Mirror;
using Mirror.Examples.BenchmarkIdle;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class NewFieldManager : NetworkBehaviour
{
    public static NewFieldManager instance;

    public List<Seed> _seedPrefabs;

     public SyncList<Seed> _allPlants = new SyncList<Seed>();

    public List<Transform> _plantPositons;

    public bool _panelOpen;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < _plantPositons.Count; i++)
        {
            _allPlants.Add(new Seed());
        }

        _allPlants.Callback += OnAllPlantsChanged;
    }

    [Command]
    public void CmdAddPlant(Seed seed, int index)
    {
        NetworkServer.Spawn(seed.gameObject);
        _allPlants[index] = seed;

        RpcAddPlant(seed.gameObject.GetComponent<NetworkIdentity>().netId, index);
    }

    [ClientRpc]
    void RpcAddPlant(uint seedNetId, int index)
    {
        if (NetworkServer.spawned.TryGetValue(seedNetId, out NetworkIdentity seedIdentity))
        {
            Seed seed = seedIdentity.GetComponent<Seed>();
            seed.StartGrow(transform, index);
            if (!_allPlants.Contains(seedIdentity.gameObject.GetComponent<Seed>()))
            {
                _allPlants[index] = seedIdentity.gameObject.GetComponent<Seed>();
            }
        }
    }
    private void OnAllPlantsChanged(SyncList<Seed>.Operation op, int itemIndex, Seed oldItem, Seed newItem)
    {
        PlayerFieldUI.UpdateAllUIs();
    }
}
