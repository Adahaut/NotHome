using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeHomeManager : MonoBehaviour
{
    private int _levelBuilding = 1;
    [SerializeField] private TextMeshProUGUI _textLevel;
    public List<DictionnaryElement<string, List<DictionnaryElement<string, int>>>> _upgarde;
    [SerializeField] private InventoryManager _inventoryManager;
    [SerializeField] private string _nameBuilding;
    [SerializeField] private ListSlotField _fieldManager;
    [SerializeField] private GameObject _alarm;
    private bool _getAlarm;

    private void Start()
    {
        _textLevel.text = "Level " + _levelBuilding.ToString();
    }
    public void SetEffectBuilding()
    {
        switch (_nameBuilding)
        {
            case "Field":
                EffectField();
                break;
            case "TDC":
                EffectTDC();
                break;
            default:
                Debug.Log("No name building");
                break;
        }
    }
    public void EffectTDC()
    {
        _getAlarm = true;
    }
    private IEnumerator StopAlarm(float second)
    {
        _alarm.SetActive(true);
        yield return new WaitForSeconds(second);
        _alarm.SetActive(false);
    }
    private void Update()
    {
        if (_nameBuilding == "TDC")
        {
            if (Input.GetKeyUp(KeyCode.G) && !_alarm.activeSelf && _getAlarm)
            {
                StartCoroutine(StopAlarm(4.0f));
            }
        }
    }
    public void EffectField()
    {
        for (int i = 0; i < _fieldManager._listSeed.Count; i++)
        {
            _fieldManager._listSeed[i].GetComponent<UseField>()._seedTime *= 0.80f;
        }
        ListSlotField.Instance._listPosSlot[ListSlotField.Instance._listPosSlot.Count - _upgarde.Count - 1 + _levelBuilding - 1].gameObject.SetActive(true);
    }
    public void UpdateBuilding()
    {
        if (_upgarde.Count >= _levelBuilding)
        {
            int number = 0;
            List<int> listIndex = new();
            List<int> listMat = new();
            for (int i = 0; i < _inventoryManager._slotList.Count; i++)
            {
                if (_inventoryManager._slotList[i].GetComponent<InventorySlot>().ItemContained() != null)
                {
                    for (int j = 0; j < _upgarde[_levelBuilding - 1].Value.Count; j++)
                    {
                        if (_inventoryManager._slotList[i].GetComponent<InventorySlot>().ItemContained().ItemName() == _upgarde[_levelBuilding - 1].Value[j].Key)
                        {
                            number++;
                            if (_inventoryManager._slotList[i].GetComponent<InventorySlot>().Number() < _upgarde[_levelBuilding - 1].Value[j].Value)
                            {
                                Debug.Log("Pas assez de materiaux");
                                return;
                            }
                            else
                            {
                                listIndex.Add(i);
                                listMat.Add(_upgarde[_levelBuilding - 1].Value[j].Value);
                            }
                        }
                    }
                }
            }
            if (number == _upgarde[_levelBuilding - 1].Value.Count)
            {
                _levelBuilding++;
                SetEffectBuilding();
                _textLevel.text = "Level " + _levelBuilding.ToString();
                for (int i = 0; i < listIndex.Count; i++)
                {
                    _inventoryManager._slotList[listIndex[i]].GetComponent<InventorySlot>().
                        SetNumber(_inventoryManager._slotList[listIndex[i]].GetComponent<InventorySlot>().Number() - listMat[i]);
                }
            }
            else
            {
                Debug.Log("Pas assez de materiaux");
            }
        }
        else
        {
            Debug.Log("Level max");
        }
    }
}
