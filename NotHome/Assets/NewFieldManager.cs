using Mirror;
using Mirror.Examples.BenchmarkIdle;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class NewFieldManager : NetworkBehaviour
{
    public static NewFieldManager instance;

    public List<Seed> _seedPrefabs;

    public List<Seed> _allPlants = new List<Seed>();

    public List<Transform> _plantPositons;

    public bool _panelOpen;

    [SyncVar]public int t = 0;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < _plantPositons.Count; i++)
        {
            _allPlants.Add(null);
        }

        //_allPlants.Callback += OnAllPlantsChanged;
    }

    //private void OnAllPlantsChanged(SyncList<Seed>.Operation op, int index, Seed oldItem, Seed newItem)
    //{
    //    //if (op == SyncList<Seed>.Operation.OP_ADD || op == SyncList<Seed>.Operation.OP_SET)
    //    //{
    //    //    if (newItem != null)
    //    //    {
    //    //        newItem.StartGrow(_plantPositons[index], index);
    //    //    }
    //    //}

    //    PlayerFieldUI.UpdateAllUIs();
    //}

}
