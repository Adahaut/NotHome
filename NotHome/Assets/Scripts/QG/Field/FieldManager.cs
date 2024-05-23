using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FieldManager : MonoBehaviour
{
    public List<TextMeshProUGUI> _timerText = new();
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
            print("oui");
            seedTime -= 1;
            _timerText[index].text = ((int)seedTime / 60).ToString("00") + ":" + ((int)seedTime % 60).ToString("00");
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
