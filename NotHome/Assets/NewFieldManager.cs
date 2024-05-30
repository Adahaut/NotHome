using Mirror;
using System.Collections.Generic;
using UnityEngine;

public struct Test
{
    public string testname;
}

public class NewFieldManager : NetworkBehaviour
{
    public static NewFieldManager instance;

    public List<Seed> _seedPrefabs;

    public SyncList<Seed> _allPlants = new SyncList<Seed>();
    public readonly SyncList<Test> aboubou = new SyncList<Test>();


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

        _allPlants.Callback += OnAllPlantsChanged;
    }

    private void OnAllPlantsChanged(SyncList<Seed>.Operation op, int index, Seed oldItem, Seed newItem)
    {
        if (op == SyncList<Seed>.Operation.OP_ADD || op == SyncList<Seed>.Operation.OP_SET)
        {
            if (newItem != null)
            {
                newItem.StartGrow(_plantPositons[index], index);
            }
        }

        PlayerFieldUI.UpdateAllUIs();
    }

}
