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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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
