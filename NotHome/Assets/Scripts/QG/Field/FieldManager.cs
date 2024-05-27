using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FieldManager : MonoBehaviour
{
    public List<Seed> _seedTimers = new();
    public static FieldManager Instance;
    [SerializeField] private Material _materialGreen;
    public Material _materialBrown;

    [SyncVar] public List<Plant> _plantList = new();
    public int _plantNb = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        CreatePlants();
    }


    void CreatePlants()
    {
        for (int i = 0; i < 13; i++)
        {
            Plant plant = new Plant(i.ToString(), null);
            _plantList.Add(plant);
        }
    }

    public void UpdatePlants(List<Transform> imgs)
    {
        for (int i = 0; i < imgs.Count; i++)
        {
            if (_plantList[i]._img != null) 
            {
                imgs[i].GetComponent<Image>().sprite = _plantList[i]._img;
            }
        }
    }

    private void Update()
    {
        if (GetComponentInChildren<BuildInterractable>().usedPlayer != null)
        {
            UpdatePlants(GetComponentInChildren<BuildInterractable>().usedPlayer.GetComponentInChildren<PlayerFieldSlot>()._listSlots);
        }
    }
    public IEnumerator StartTimer(int index, float seedTime, int seed)
    {
        if (seedTime > 0)
        {
            seedTime -= 1;
            _seedTimers[index].currentTimer = seedTime;
            //GetComponent<BuildInterractable>().usedPlayer.GetComponentInChildren<PlayerFieldSlot>()._listTexts
            yield return new WaitForSeconds(1);
            StartCoroutine(StartTimer(index, seedTime, seed));
        }
        else
        {
            ListSlotField.Instance._listSeed[seed - 1].GetComponent<Button>().enabled = true;
            ListSlotField.Instance._listPlant[index].GetComponent<MeshRenderer>().material = _materialGreen;
            yield return null;
        }
    }

    public void StartCo(int index, float seedTime, int seed)
    {
        StartCoroutine(StartTimer(index, seedTime, seed));
    }
}
