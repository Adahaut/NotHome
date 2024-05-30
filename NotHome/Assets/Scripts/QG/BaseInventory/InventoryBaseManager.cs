using Org.BouncyCastle.Asn1.Mozilla;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryBaseManager : InventoryManager
{
    public static InventoryBaseManager instance;

    public Dictionary<string, int> _baseInventory = new Dictionary<string, int>();
    public List<InventorySlot> _inventorySlots = new List<InventorySlot>();

    

    //[SerializeField] private PC _playerController;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //InventoryInitialisation();
        //for (int i = 0; i < InventorySlotNumber(); i++)
        //{
        //    _inventorySlots.Add(GetInventorySlot(i));
        //}
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
        if (_baseInventory[_itemName] > _number)
        {
            _baseInventory[_itemName] -= _number;
            for (int i = 0; i < _inventorySlots.Count; i++)
            {
                if (_inventorySlots[i].ItemContained().ItemName() == _itemName)
                {
                    _inventorySlots[i].SetNumber(_inventorySlots[i].Number() - _number);
                }
            }
        }
        else
        {
            _baseInventory.Remove(_itemName);
            for (int i = 0; i < _inventorySlots.Count; i++)
            {
                if (_inventorySlots[i].ItemContained().ItemName() == _itemName)
                {
                    _inventorySlots[i].ResetItem();
                }
            }
        }
    }

    //private void OnEnable()
    //{
    //    OpenCloseInventory(true);
    //}

    //private void OnDisable()
    //{
    //    OpenCloseInventory(false);
    //}

    //private void OpenCloseInventory(bool _open)
    //{
    //    _playerController.SetInventoryActive(_open);
    //    _playerController.SetIsInBaseInventory(_open);
    //}

    private void AddItemInBase(string _name, int _number, GameObject _slot, GameObject _oldSlot)
    {
        if (_baseInventory.ContainsKey(_name))
        {
            _baseInventory[_name] += _number;
            _slot.GetComponent<InventorySlot>().SetNumber(_baseInventory[_name]);
        }
        else if (_slot.GetComponent<InventorySlot>().ItemContained() == null)
        {
            _baseInventory.Add(_name, _number);
            _slot.GetComponent<InventorySlot>().ChangeItem(_name, _oldSlot.GetComponent<InventorySlot>().ItemContained().ItemSprite(), false);
            _slot.GetComponent<InventorySlot>().SetNumber(_baseInventory[_name]);
        }
        else
        {
            _baseInventory.Add(_name, _number);
            AddItem(_name, _oldSlot.GetComponent<InventorySlot>().ItemContained().ItemSprite(), false, _number);
        }
        _oldSlot.GetComponent<InventorySlot>().ResetItem();
    }

    private void RemoveItemFromBase(string _name, GameObject _slot, GameObject _oldSlot)
    {
        _slot.GetComponent<InventorySlot>().ChangeItem(_name, _oldSlot.GetComponent<InventorySlot>().ItemContained().ItemSprite(), false);
        _slot.GetComponent<InventorySlot>().SetNumber(NumberOfMaterial(_name));
        _oldSlot.GetComponent<InventorySlot>().ResetItem();
        _baseInventory.Remove(_name);
    }

    //private void Update()
    //{
    //    Vector3 MousePos = Input.mousePosition;
        
    //    PointerEventData pointerEventData = new PointerEventData(_eventSystem);
    //    pointerEventData.position = MousePos;
    //    _dragNDrop.transform.position = MousePos;

    //    List<RaycastResult> results = new List<RaycastResult>();

    //    _raycaster.Raycast(pointerEventData, results);
        
    //    if (results.Count > 0)
    //    {
    //        if (_draging && Input.GetMouseButtonUp(0))
    //        {
    //            ChangeChildParent(_dragNDrop.transform, _itemImage.transform);
    //            _draging = false;
    //            if (CheckIfHasGoodTag(results[0].gameObject) && CheckIfParentsNotAreSame(_itemImage, results[0].gameObject))
    //            {
    //                if(results[0].gameObject.CompareTag(_itemBaseContainerTag))
    //                {
    //                    AddItemInBase(_itemImage.GetComponent<InventorySlot>().ItemContained().ItemName(), _itemImage.GetComponent<InventorySlot>().Number(), results[0].gameObject, _itemImage);
    //                }
    //                else
    //                {
    //                    print(_baseInventory[_itemImage.GetComponent<InventorySlot>().ItemContained().ItemName()]);
    //                    RemoveItemFromBase(_itemImage.GetComponent<InventorySlot>().ItemContained().ItemName(), results[0].gameObject, _itemImage);
    //                }
    //            }
    //        }
    //        if (!_draging && Input.GetMouseButtonDown(0) && results[0].gameObject.TryGetComponent<InventorySlot>(out InventorySlot _inventorySlot) && _inventorySlot.ItemContained().ItemName() != "None")
    //        {
    //            _itemImage = results[0].gameObject;
    //            if (CheckIfHasGoodTag(_itemImage))
    //            {
    //                _draging = true;
    //                ChangeChildParent(_itemImage.transform, _dragNDrop.transform);
    //            }
    //        }
    //    }

    //}

    

}
