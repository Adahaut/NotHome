using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private GameObject _inventorySlotPrefab;

    [SerializeField] private int _inventorySlotStartNumber;

    private List<GameObject> _slotList = new List<GameObject>();


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

    public void AddItem(string _ItemName, Sprite _itemSprite)
    {
        TryAddItem(_ItemName, _itemSprite);
    }

    private void TryAddItem(string _ItemName, Sprite _itemSprite)
    {
        for (int i = 0; i < _slotList.Count; i++)
        {
            if (_slotList[i].GetComponent<InventorySlot>().ItemContained().ItemName() == "None")
            {
                _slotList[i].GetComponent<InventorySlot>().SetNumber(1);
                _slotList[i].GetComponent<InventorySlot>().ChangeItem(_ItemName, _itemSprite);
                break;
            }
            else if (_slotList[i].GetComponent<InventorySlot>().ItemContained().ItemName() == _ItemName)
            {
                _slotList[i].GetComponent<InventorySlot>().AddNumber();
                break;
            }
        }
    }

    /*private bool CheckInSlots()
    {
        for(int i = 0; i < _inventorySlotStartNumber; i++)
        {

        }
    }

    public void RemoveItems(string _itemName, int _number)
    {
        if ()
        {
            
        }
        else
        {
            
        }
    }*/

}
