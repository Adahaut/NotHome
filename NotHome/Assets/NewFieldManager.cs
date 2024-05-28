using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NewFieldManager : NetworkBehaviour
{
    public static NewFieldManager instance;

    public List<Seed> _allPlants;

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
    public void CmdAddPlant()
    {
        //RpcAddPlant();
        
    }

    public void RpcAddPlant(int index, Seed type)
    {
        _allPlants[index] = type;
        _allPlants[index].StartGrow(_plantPositons[index], index);
    }
}
