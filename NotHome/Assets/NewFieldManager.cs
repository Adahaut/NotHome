using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NewFieldManager : NetworkBehaviour
{
    public static NewFieldManager instance;

    [SerializeField] private List<Seed> _seedPrefabs;

    [SyncVar] public List<Seed> _allPlants;

    [SerializeField] private List<Transform> _plantPositons;

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
    }

    [Command]
    public void CmdAddPlant(int index, int seedId)
    {
        RpcAddPlant(index, seedId);
    }

    [ClientRpc]
    public void RpcAddPlant(int index, int seedTypeId)
    {
        Seed newSeed = InstantiateSeedById(seedTypeId);
        _allPlants[index] = newSeed;
        _allPlants[index].StartGrow(_plantPositons[index], index);
    }

    private Seed InstantiateSeedById(int seedTypeId)
    { 
        Seed newSeed = Instantiate(_seedPrefabs[seedTypeId]);
        return newSeed;
    }
}
