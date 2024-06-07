using System.Collections.Generic;
using UnityEngine;

public class UpgradePlayerManager : MonoBehaviour
{
    private int _levelPlayer = 1;
    public List<DictionnaryElement<string, List<DictionnaryElement<string, int>>>> _upgarde;
    private InventoryManager _inventoryManager;
    private PlayerManager _playerManager;

    private void Start()
    {
        _inventoryManager = GetComponent<InventoryManager>();
        _playerManager = GetComponent<PlayerManager>();
    }
    public void SetEffectLevelPlayer()
    {
        switch (_levelPlayer)
        {
            case 2:
                _inventoryManager.SetInventorySlotNumber(4);
                _playerManager.SetMaxStamina(_playerManager.MaxStamina * 1.10f);
                _playerManager.SetMaxOxygene(_playerManager.MaxOxygene * 1.10f);
                break;
            case 3:
                PC.Instance.SetUseTorch(true);
                _playerManager.SetMaxStamina(_playerManager.MaxStamina * 1.10f);
                _playerManager.SetMaxOxygene(_playerManager.MaxOxygene * 1.10f);
                break;
            case 4:
                _playerManager.SetMaxStamina(_playerManager.MaxStamina * 1.10f);
                _playerManager.SetMaxOxygene(_playerManager.MaxOxygene * 1.10f);
                break;
            default:
                break;
        }
    }
    public void UpdateBuilding()
    {
        if (_upgarde.Count >= _levelPlayer)
        {
            int number = 0;
            List<int> listIndex = new();
            List<int> listMat = new();
            for (int i = 0; i < _inventoryManager._slotList.Count; i++)
            {
                if (_inventoryManager._slotList[i].GetComponent<InventorySlot>().ItemContained() != null)
                {
                    for (int j = 0; j < _upgarde[_levelPlayer - 1].Value.Count; j++)
                    {
                        if (_inventoryManager._slotList[i].GetComponent<InventorySlot>().ItemContained().ItemName() == _upgarde[_levelPlayer - 1].Value[j].Key)
                        {
                            number++;
                            if (_inventoryManager._slotList[i].GetComponent<InventorySlot>().Number() < _upgarde[_levelPlayer - 1].Value[j].Value)
                            {
                                Debug.Log("Pas assez de materiaux");
                                return;
                            }
                            else
                            {
                                listIndex.Add(i);
                                listMat.Add(_upgarde[_levelPlayer - 1].Value[j].Value);
                            }
                        }
                    }
                }
            }
            if (number == _upgarde[_levelPlayer - 1].Value.Count)
            {
                _levelPlayer++;
                SetEffectLevelPlayer();
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
