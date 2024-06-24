using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : NetworkBehaviour
{
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private GameObject _inventorySlotPrefab;

    [SerializeField] private int _inventorySlotStartNumber;

    public List<GameObject> _slotList = new List<GameObject>();

    [SerializeField] private List<GameObject> _itemsPrefabs = new List<GameObject>();
    [SerializeField] private Color _selectedColor = Color.yellow;
    private HotBarManager _hotBarManager;


    public GameObject GetItemPrefab(string _name)
    {
        for(int i = 0;  i < _itemsPrefabs.Count; ++i)
        {
            if (_itemsPrefabs[i].name == _name + "Item")
                return _itemsPrefabs[i];
        }
        return null;
    }

    private void OnEnable()
    {
        UnSelectAll();
    }

    private void CreateCase()
    {
        GameObject _newInventorySlot = Instantiate(_inventorySlotPrefab);
        _newInventorySlot.transform.SetParent(_inventoryPanel.transform);
        _newInventorySlot.transform.rotation = Quaternion.Euler(0, 0, 0);
        _slotList.Add(_newInventorySlot);
    }

    public void UnSelectAll()
    {
        for(int i = 0; i < _slotList.Count; i++)
        {
            _slotList[i].GetComponent<Image>().color = Color.white;
        }
    }

    public InventorySlot SelectAt(int _index)
    {
        print(_index);
        if (_index < 0) _index = _slotList.Count - 1;
        else if (_index > _slotList.Count - 1) _index = 0;
        print(_index);
        UnSelectAll();
        _slotList[_index].GetComponent<Image>().color = _selectedColor;
        return _slotList[_index].GetComponent<InventorySlot>();
    }

    public int IndexOfSlot(InventorySlot _slot)
    {
        return _slotList.IndexOf(_slot.gameObject);
    }

    public InventorySlot GetInventorySlot(int _index)
    {
        return _slotList[_index].GetComponent<InventorySlot>();
    } 

    public int GetIndexOfSlotByName(string _name)
    {
        for(int i = 0; i<_slotList.Count; i++)
        {
            if (_slotList[i].GetComponent<InventorySlot>().ItemContained().ItemName() == _name)
                return i;
        }
        return -1;
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
        if(_isAnEquipement)
        {
            _hotBarManager.AddTalkieWalkie();
        }
        else
        {
            for (int i = 0; i < _slotList.Count; i++)
            {
                if (_slotList[i].GetComponent<InventorySlot>().ItemContained().ItemName() == _ItemName)
                {
                    _slotList[i].GetComponent<InventorySlot>().SetNumberAndName(_slotList[i].GetComponent<InventorySlot>().Number() + _number, _ItemName);
                    return;
                }
            }
            for (int i = 0; i < _slotList.Count; i++)
            {
                if (_slotList[i].GetComponent<InventorySlot>().ItemContained().ItemName() == "None")
                {
                    _slotList[i].GetComponent<InventorySlot>().SetNumberAndName(_number, _ItemName);
                    _slotList[i].GetComponent<InventorySlot>().ChangeItem(_ItemName, _itemSprite, _isAnEquipement);
                    return;
                }
            }
        }
    }

    public bool HasRemainingPlace(string _ItemName = "None")
    {
        for (int i = 0; i < _slotList.Count; i++)
        {
            if (_slotList[i].GetComponent<InventorySlot>().ItemContained().ItemName() == "None" 
                || _slotList[i].GetComponent<InventorySlot>().ItemContained().ItemName() == _ItemName)
            {
                return true;
            }
        }
        return false;
    }

    public bool ContainItem(string _ItemName)
    {
        for (int i = 0; i < _slotList.Count; i++)
        {
            if (_slotList[i].GetComponent<InventorySlot>().ItemContained().ItemName() == _ItemName)
            {
                return true;
            }
        }
        return false;
    }

    public void RemoveItems(string _itemName, int _number)
    {
        for (int i = 0; i < _inventorySlotStartNumber; i++)
        {
            if (_slotList[i].GetComponent<InventorySlot>().ItemContained().ItemName() == _itemName)
            {
                if (_slotList[i].GetComponent<InventorySlot>().Number() > _number)
                {
                    _slotList[i].GetComponent<InventorySlot>().SetNumberAndName(_slotList[i].GetComponent<InventorySlot>().Number() - _number, _itemName);
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
