using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeHomeManager : MonoBehaviour
{
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private TextMeshProUGUI _textLevel;
    [SerializeField] private TextMeshProUGUI _ressourcesNeeded;
    public List<DictionnaryElement<string, List<DictionnaryElement<string, int>>>> _upgarde;
    [SerializeField] private InventoryManager _inventoryManager;
    [SerializeField] private string _nameBuilding;

    private void Start()
    {
        _textLevel.text = "Level 1";
    }

    private void OnEnable()
    {
        UpdateRessourcesText();
    }

    private void UpdateRessourcesText()
    {
        _ressourcesNeeded.text = "";
        int index = 0;
        if (_nameBuilding == "TDC")
            index = 1;
        if (UpgradeBuilding.Instance._levelBuildingList[index]._levelBuilding - 1 < _upgarde.Count)
        {
            for (int i = 0; i < _upgarde[UpgradeBuilding.Instance._levelBuildingList[index]._levelBuilding - 1].Value.Count; i++)
            {
                _ressourcesNeeded.text += _upgarde[UpgradeBuilding.Instance._levelBuildingList[index]._levelBuilding - 1].Value[i].Value + " X " + _upgarde[UpgradeBuilding.Instance._levelBuildingList[index]._levelBuilding - 1].Value[i].Key + "\n\n";
            }
        }
    }

    
    public void UpdateBuilding(GameObject button)
    {
        int index = 0;
        if (_nameBuilding == "TDC")
            index = 1;
        if (_upgarde.Count >= UpgradeBuilding.Instance._levelBuildingList[index]._levelBuilding)
        {
            int number = 0;
            List<int> listIndex = new();
            List<int> listMat = new();
            for (int i = 0; i < _inventoryManager._slotList.Count; i++)
            {
                if (_inventoryManager._slotList[i].GetComponent<InventorySlot>().ItemContained() != null)
                {
                    for (int j = 0; j < _upgarde[UpgradeBuilding.Instance._levelBuildingList[index]._levelBuilding - 1].Value.Count; j++)
                    {
                        if (_inventoryManager._slotList[i].GetComponent<InventorySlot>().ItemContained().ItemName() == _upgarde[UpgradeBuilding.Instance._levelBuildingList[index]._levelBuilding - 1].Value[j].Key)
                        {
                            number++;
                            if (_inventoryManager._slotList[i].GetComponent<InventorySlot>().Number() < _upgarde[UpgradeBuilding.Instance._levelBuildingList[index]._levelBuilding - 1].Value[j].Value)
                            {
                                Debug.Log("Pas assez de materiaux");
                                return;
                            }
                            else
                            {
                                listIndex.Add(i);
                                listMat.Add(_upgarde[UpgradeBuilding.Instance._levelBuildingList[index]._levelBuilding - 1].Value[j].Value);
                            }
                        }
                    }
                }
            }
            if (number == _upgarde[UpgradeBuilding.Instance._levelBuildingList[index]._levelBuilding - 1].Value.Count)
            {
                UpgradeBuilding.Instance._levelBuildingList[index]._levelBuilding++;
                UpgradeBuilding.Instance.SetEffectBuilding(_nameBuilding);
                UpdateRessourcesText();
                _textLevel.text = "Level " + UpgradeBuilding.Instance._levelBuildingList[index]._levelBuilding.ToString();
                for (int i = 0; i < listIndex.Count; i++)
                {
                    _inventoryManager._slotList[listIndex[i]].GetComponent<InventorySlot>().
                        SetNumberAndName(_inventoryManager._slotList[listIndex[i]].GetComponent<InventorySlot>().Number() - listMat[i], 
                        _inventoryManager._slotList[listIndex[i]].GetComponent<InventorySlot>()._name);
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
