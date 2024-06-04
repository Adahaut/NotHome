using Mirror;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static InventoryBaseManager;

public class PlayerStockageUI : NetworkBehaviour
{
    private EventSystem _eventSystem;
    [SerializeField] GraphicRaycaster _raycaster;
    [SerializeField] private GameObject _dragNDrop;
    [SerializeField] private Sprite _defaultSprite;

    [SerializeField] private string _itemContainerTag;
    [SerializeField] private string _itemBaseContainerTag;

    private bool _draging = false;
    private InventorySlot _itemImage;

    [SerializeField] private GameObject _inventorySlotPrefab;
    [SerializeField] private GameObject _inventoryBasePanel;
    [SerializeField] private GameObject _inventoryPanel;
    public List<GameObject> _slotList = new List<GameObject>();

    public int _baseInventorySlotCount = 10;

    public TMP_Text debug;

    private void OnEnable()
    {
        UpdateItemList();
        UpdateStockageUI();

        if (_eventSystem == null )
            _eventSystem = FindObjectOfType<EventSystem>();

        //if (InventoryBaseManager.instance._inventoryItems.Count == 0)
        //    Init();

        _inventoryPanel.gameObject.SetActive(true);
    }

    private void Update()
    {

        Vector3 MousePos = Input.mousePosition;

        PointerEventData pointerEventData = new PointerEventData(_eventSystem);
        pointerEventData.position = MousePos;
        _dragNDrop.transform.position = MousePos;

        List<RaycastResult> results = new List<RaycastResult>();

        _raycaster.Raycast(pointerEventData, results);

        if (results.Count > 0)
        {
            if (_draging && Input.GetMouseButtonUp(0))
            {
                ChangeChildParent(_dragNDrop.transform, _itemImage.transform);
                _draging = false;
                if (CheckIfHasGoodTag(results[0].gameObject) && CheckIfParentsNotAreSame(_itemImage.gameObject, results[0].gameObject))
                {
                    if (results[0].gameObject.CompareTag(_itemBaseContainerTag))
                    {
                        AddItemInBase(_itemImage.ItemContained().ItemName(), _itemImage.Number(), 
                            GetIndexOf(results[0].gameObject.GetComponent<InventorySlot>().ItemContained().ItemName()), _itemImage);
                    }
                    else
                    {
                        RemoveItemFromBase(_itemImage.ItemContained().ItemName(), _itemImage.Number(), null/*_itemImage.ItemContained().ItemSprite()*/,
                            GetIndexOf(_itemImage.ItemContained().ItemName()), results[0].gameObject.GetComponent<InventorySlot>());
                    }
                    UpdateItemList();
                }
            }
            if (!_draging && Input.GetMouseButtonDown(0)
                && results[0].gameObject.TryGetComponent<InventorySlot>(out InventorySlot _inventorySlot) && _inventorySlot.ItemContained().ItemName() != "None")
            {
                _itemImage = results[0].gameObject.GetComponent<InventorySlot>();
                if (CheckIfHasGoodTag(_itemImage.gameObject))
                {
                    _draging = true;
                    ChangeChildParent(_itemImage.transform, _dragNDrop.transform);
                }
                UpdateItemList();
            }
        }
        UpdateStockageUI();

        debug.text = "";
        for (int i = 0; i < InventoryBaseManager.instance._inventoryItems.Count; i++)
        {
            debug.text += i + " " + InventoryBaseManager.instance._inventoryItems[i]._name + "\n";
        }
    }

    private void UpdateStockageUI()
    {
        for (int i = 0; i < InventoryBaseManager.instance._inventorySize; ++i)
        {
            if (_slotList[i].name != "None")
            {
                _slotList[i].GetComponent<InventorySlot>().UpdateItemVisuel();
                print(_slotList[i].GetComponent<InventorySlot>()._itemContained._sprite);
            }
            else
            {

            }
        }
    }

    private bool CheckIfParentsNotAreSame(GameObject _gameobject1, GameObject _gameobject2)
    {
        return _gameobject1.transform.parent.gameObject != _gameobject2.transform.parent.gameObject;
    }

    private bool CheckIfHasGoodTag(GameObject _check)
    {
        return _check.CompareTag(_itemBaseContainerTag) || _check.CompareTag(_itemContainerTag);
    }

    private void ChangeChildParent(Transform _gameObject, Transform _newParent)
    {
        _gameObject.GetChild(0).SetParent(_newParent);
        _newParent.GetChild(0).transform.localPosition = Vector3.zero;
    }

    public void DisableStockageUI()
    {
        GetComponentInParent<PlayerController>().DisablePlayer(false);
        this.gameObject.SetActive(false);
        _inventoryPanel.gameObject.SetActive(false);

    }



    //------------------------------------------------------------------------------------------------------------------------------------

    ///////////////////////////////////////
    ///                                 ///
    ///      Base inventory Manager     /// 
    ///                                 ///
    ///////////////////////////////////////

    //add item in base
    public void AddItemInBase(string _name, int _number, int _slotIndex, InventorySlot _playerInventorySlot)
    {
        if (ListContain(_name))
        {
            AddNumberItem(_name, _number);
            UpdateOneItem(_slotIndex, _number);
            print("already in list");
        }
        else if (InventoryBaseManager.instance._inventoryItems[_slotIndex]._name == "None")
        {
            AddNewItem(_name, _number, _slotIndex);
            UpdateOneItem(_slotIndex, _number);
            print("new item added in list");
        }
        else
        {
            AddNewItem(_name, _number, GetIndexOf("None"));
            UpdateOneItem(_slotIndex, _number);
            print("else");
        }
        _playerInventorySlot.ResetItem();
    }

    public void RemoveItemFromBase(string _name, int _number, Sprite _sprite, int _index, InventorySlot _inventorySlot)
    {
        _inventorySlot.ChangeItem(_name, _sprite, false);
        _inventorySlot.SetNumber(_number);
        UpdateOneItem(GetIndexOf(_name), _number);
        RemoveItem(_index);
    }

    //----------------------------//
    //     Update Struct list     //
    //----------------------------//

    // init
    private _itemSlot initItemSlot()
    {
        _itemSlot slot = new _itemSlot();
        slot._name = "None";
        slot._number = 0;
        //slot._sprite = null;
        return slot;
    }

    [Command]
    private void UpdateOneItemStruct(int _index, string _name, int _number, Sprite _sprite)
    {
        _itemSlot slot = new _itemSlot();
        slot._name = _name;
        slot._number = _number;
        //slot._sprite = _sprite;
        InventoryBaseManager.instance._inventoryItems[_index] = slot;
    }

    //------------------------------------//
    //     Update InventorySlots list     //
    //------------------------------------//

    // Update All Item in list
    [Command]
    public void UpdateItemList()
    {
        for (int i = 0; i < InventoryBaseManager.instance._inventorySize; i++)
        {
            UpdateOneItem(i, InventoryBaseManager.instance._inventoryItems[i]._number);
        }
    }

    // Update one Item from list at specific index
    [Command]
    public void UpdateOneItem(int _index, int _number)
    {
        foreach (Item i in InventoryBaseManager.instance._allItems)
        {
            if (InventoryBaseManager.instance._inventoryItems[_index]._name != "None" && InventoryBaseManager.instance._inventoryItems[_index]._name == i.ItemName())
            {
                Sprite s = i.ItemSprite();
                _slotList[_index].GetComponent<InventorySlot>().UpdateItem(_number, s, InventoryBaseManager.instance._inventoryItems[_index]._name);
                print(_slotList[_index].GetComponent<InventorySlot>()._itemContained._sprite);
                return;
            }
        }

    }

    [Command]
    private void UpdateItemInList(string _name, int _number, int _index)
    {
       
        _itemSlot tempSlot = new _itemSlot();
        tempSlot._name = _name;
        tempSlot._number = _number;
        //tempSlot._sprite = _sprite;
        
        InventoryBaseManager.instance._inventoryItems[_index] = tempSlot;
    }

    // add an item that is not already in inventory
    private void AddNewItem(string _name, int _number, int _index)
    {
        if (!HasPlaceRemaining())
            throw new Exception("no remaining place");

        UpdateItemInList(_name, _number, _index);
    }

    // a a unmber of item that is in inventory
    [Command]
    private void AddNumberItem(string _name, int _number)
    {
        int _index = GetIndexOf(_name);
        _itemSlot itemSlot = InventoryBaseManager.instance._inventoryItems[_index];
        itemSlot._number += _number;
    }

    [Command]
    private void RemoveItem(int _index)
    {
        InventoryBaseManager.instance._inventoryItems[_index] = initItemSlot();
        _slotList[_index].GetComponent<InventorySlot>().ResetItem();
    }

    // check if inventory has one or more place remaining
    private bool HasPlaceRemaining()
    {
        return ListContain("None");
    }

    ///////////////////////////////////////
    ///     _inventoryItems Manager     ///
    ///////////////////////////////////////
    /// -->

    //check if item List contain _name
    private bool ListContain(string _name)
    {
        
        for (int i = 0; i < InventoryBaseManager.instance._inventorySize; i++)
        {
            if (InventoryBaseManager.instance._inventoryItems[i]._name == _name)
            {
                return true;
            }
        }
        return false;
    }

    private int GetIndexOf(string _name)
    {
        if (!ListContain(_name))
            throw new Exception("item is not is the List");

        for(int i = 0; i < InventoryBaseManager.instance._inventorySize; i++)
        {
            if (InventoryBaseManager.instance._inventoryItems[i]._name == _name)
                return i;
        }

        throw new Exception("item not found");
    }

    private _itemSlot GetFirstItemOfName(string _name)
    {
        for (int i = 0; i < InventoryBaseManager.instance._inventorySize; i++)
        {
            if (InventoryBaseManager.instance._inventoryItems[i]._name == _name)
                return InventoryBaseManager.instance._inventoryItems[i];
        }

        throw new Exception("item not found");
    }

}
