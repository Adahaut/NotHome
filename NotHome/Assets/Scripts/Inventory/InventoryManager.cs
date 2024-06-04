using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static InventoryBaseManager;

public class InventoryManager : NetworkBehaviour
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

    public void UnSelectionAll()
    {
        for(int i = 0; i < _slotList.Count; i++)
        {
            _slotList[i].GetComponent<Image>().color = Color.black;
        }
    }

    public InventorySlot GetInventorySlot(int _index)
    {
        return _slotList[_index].GetComponent<InventorySlot>();
    } 

    public int InventorySlotNumber() { return _inventorySlotStartNumber; }

    public void SetInventorySlotNumber(int number) { _inventorySlotStartNumber += number; }

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
        if(isServer)
        {
            Debug.Log("Init base inventory on server");
            Init();
        }
        
    }

    #region Base Inventory

    [Command]
    private void Init()
    {
        InventoryBaseManager.instance._inventorySize = 15;
        for (int i = 0; i < InventoryBaseManager.instance._inventorySize; i++)
        {
            InventoryBaseManager.instance._inventoryItems.Add(initItemSlot());
        }
    }

    // init
    private _itemSlot initItemSlot()
    {
        _itemSlot slot = new _itemSlot();
        slot._name = "None";
        slot._number = 0;
        //slot._sprite = null;
        return slot;
    }

    #endregion



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
