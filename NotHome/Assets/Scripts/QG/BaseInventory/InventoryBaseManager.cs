using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBaseManager : InventoryManager
{
    public static InventoryBaseManager instance;

    public List<Item> _allItems;

    public Dictionary<string, int> _baseInventory = new Dictionary<string, int>();

    [SerializeField] public SyncList<_itemSlot> _inventoryItems = new SyncList<_itemSlot>();
    public int _inventorySize;

    [System.Serializable]
    public struct _itemSlot
    {
        public string _name;
        public int _number;
        //public Sprite _sprite;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        
    }

    public bool CheckForMaterial(string _itemName)
    {
        return _baseInventory.ContainsKey(_itemName);
    }

    public bool CheckForNumber(string _itemName, int _number) 
    { 
        return _baseInventory[_itemName] >= _number;
    }

    public int NumberOfMaterial(string _itemName)
    {
        return _baseInventory[_itemName];
    }

    public new void RemoveItems(string _itemName, int _number)
    {
        //if (_baseInventory[_itemName] > _number)
        //{
        //    _baseInventory[_itemName] -= _number;
        //    for (int i = 0; i < _inventorySlots.Count; i++)
        //    {
        //        if (_inventorySlots[i].ItemContained().ItemName() == _itemName)
        //        {
        //            _inventorySlots[i].SetNumber(_inventorySlots[i].Number() - _number);
        //        }
        //    }
        //}
        //else
        //{
        //    _baseInventory.Remove(_itemName);
        //    for (int i = 0; i < _inventorySlots.Count; i++)
        //    {
        //        if (_inventorySlots[i].ItemContained().ItemName() == _itemName)
        //        {
        //            _inventorySlots[i].ResetItem();
        //        }
        //    }
        //}
    }

}
