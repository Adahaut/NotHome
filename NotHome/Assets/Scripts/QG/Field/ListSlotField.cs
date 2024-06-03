using System.Collections.Generic;
using UnityEngine;

public class ListSlotField : MonoBehaviour
{
    //public List<Transform> _listPosSlot = new();
    [HideInInspector] public List<bool> _listIsPlant = new();
    public List<GameObject> _listPlant = new();
    public List<GameObject> _listSeed = new();
    public static ListSlotField Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        for (int i = 0; i < _listPlant.Count; i++)
        {
            _listIsPlant.Add(false);
        }
    }
}
