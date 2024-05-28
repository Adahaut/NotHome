using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NewFieldManager : NetworkBehaviour
{
    public static NewFieldManager instance;

    public List<Seed> _seedPrefabs;

    [SyncVar] public List<int> _allPlants;

    [SerializeField] private List<Transform> _plantPositons;

    public bool _panelOpen;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _allPlants = new List<int>(new int[_plantPositons.Count]);
    }

    [Command]
    public void CmdAddPlant(int index, int seedId)
    {
        _allPlants[index] = seedId;
        RpcAddPlant(index, seedId);
    }

    [ClientRpc]
    public void RpcAddPlant(int index, int seedTypeId)
    {
        Seed newSeed = InstantiateSeedById(seedTypeId);
        newSeed._index = index;
        newSeed.StartGrow(_plantPositons[index], index);
    }

    private Seed InstantiateSeedById(int seedTypeId)
    { 
        Seed newSeed = Instantiate(_seedPrefabs[seedTypeId]);
        return newSeed;
    }
}
