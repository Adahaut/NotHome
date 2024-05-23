using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private GameObject _inventorySlotPrefab;

    [SerializeField] private int _inventorySlotStartNumber;

    public List<GameObject> _slotList = new List<GameObject>();


    private void CreateCase()
    {
        GameObject _newInventorySlot = Instantiate(_inventorySlotPrefab);
        _newInventorySlot.transform.SetParent(_inventoryPanel.transform);
        _slotList.Add(_newInventorySlot);
    }

    public InventorySlot GetInventorySlot(int _index)
    {
        return _slotList[_index].GetComponent<InventorySlot>();
    } 

    public int InventorySlotNumber() { return _inventorySlotStartNumber; }

    public void InventoryInitialisation()
    {
        for (int i = 0;  i < _inventorySlotStartNumber; i++)
        {
            CreateCase();
        }
    }

    private void Start()
    {
        InventoryInitialisation();
        gameObject.SetActive(false);
    }

    public void AddItem(string _ItemName, Sprite _itemSprite, bool _isAnEquipement, int _number = 1)
    {
        TryAddItem(_ItemName, _itemSprite, _isAnEquipement, _number);
    }

    private void TryAddItem(string _ItemName, Sprite _itemSprite, bool _isAnEquipement, int _number)
    {
        for (int i = 0; i < _slotList.Count; i++)
        {
            if (_slotList[i].GetComponent<InventorySlot>().ItemContained().ItemName() == "None")
            {
                _slotList[i].GetComponent<InventorySlot>().SetNumber(_number);
                _slotList[i].GetComponent<InventorySlot>().ChangeItem(_ItemName, _itemSprite, _isAnEquipement);
                return;
            }
            else if (_slotList[i].GetComponent<InventorySlot>().ItemContained().ItemName() == _ItemName)
            {
                _slotList[i].GetComponent<InventorySlot>().SetNumber(_slotList[i].GetComponent<InventorySlot>().Number() + _number);
                return;
            }
        }
    }

    public void RemoveItems(string _itemName, int _number)
    {
        for (int i = 0; i < _inventorySlotStartNumber; i++)
        {
            if (_slotList[i].GetComponent<InventorySlot>().ItemContained().ItemName() == _itemName)
            {
                if (_slotList[i].GetComponent<InventorySlot>().Number() > _number)
                {
                    _slotList[i].GetComponent<InventorySlot>().SetNumber(_slotList[i].GetComponent<InventorySlot>().Number() - _number);
                    _number = _slotList[i].GetComponent<InventorySlot>().Number() - _number;
                    print(_number);
                }
                else
                {
                    _slotList[i].GetComponent<InventorySlot>().ResetItem();
                }
            }
            
        }
    }
}
