using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NewFieldManager : MonoBehaviour
{
    public static NewFieldManager instance;


    public List<Seed> _allPlants;

    [SerializeField] private List<Transform> _plantPositons;

    [Header("UI")]
    public GameObject fieldPlayerCanvas;

    public bool _panelOpen;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _allPlants = new List<Seed>(_plantPositons.Count);
    }

    public void PlantSeed(int index, Seed type)
    {
        _allPlants[index] = type;
        _allPlants[index].StartGrow(_plantPositons[index], index);
    }
}
