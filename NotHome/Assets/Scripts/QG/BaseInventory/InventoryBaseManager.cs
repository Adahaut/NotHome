using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct _itemSlot
{
    [SyncVar] public string _name;
    [SyncVar] public int _number;
    //public Sprite _sprite;
}

public class InventoryBaseManager : InventoryManager
{
    public static InventoryBaseManager instance;

    public List<Item> _allItems;

    public Dictionary<string, int> _baseInventory = new Dictionary<string, int>();

    [SerializeField] public SyncList<_itemSlot> _inventoryItems = new SyncList<_itemSlot>();
    [SyncVar] public int _inventorySize;

    

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        
    }

    public bool CheckForMaterial(string _itemName)
    {
        for (int i = 0; i < _inventoryItems.Count; i++)
        {
            if (_inventoryItems[i]._name == _itemName)
                return true;
        }
        return false;
    }

    private int GetIndexOfItem(string _itemName)
    {
        int _index = 0;
        for (int i = 0; i < _inventoryItems.Count; i++)
        {
            if (_inventoryItems[i]._name == _itemName)
            {
                _index = i;
                break;
            }
        }
        return _index;
    }

    public bool CheckForNumber(string _itemName, int _number) 
    { 
        return _inventoryItems[GetIndexOfItem(_itemName)]._number >= _number;
    }

    public int NumberOfMaterial(string _itemName)
    {
        return _inventoryItems[GetIndexOfItem(_itemName)]._number;
    }

    public new void RemoveItems(string _itemName, int _number)
    {
        //if (_baseInventory[_itemName] > _number)
        //{
        //    _baseInventory[_itemName] -= _number;
        //    for (int i = 0; i < _inventoryItems.Count; i++)
        //    {
        //        if (_inventoryItems[i]._name == _itemName)
        //        {
        //            _inventoryItems[i].SetNumber(_inventoryItems[i].Number() - _number);
        //        }
        //    }
        //}
        //else
        //{
        //    _baseInventory.Remove(_itemName);
        //    for (int i = 0; i < _inventoryItems.Count; i++)
        //    {
        //        if (_inventoryItems[i]._name == _itemName)
        //        {
        //            _inventoryItems[i].ResetItem();
        //        }
        //    }
        //}
    }

}
