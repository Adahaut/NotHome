
using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using System.Reflection;

public class InventoryBaseManager : InventoryManager
{
    public static InventoryBaseManager instance;

    public Dictionary<string, int> _baseInventory = new Dictionary<string, int>();
    //public List<InventorySlot> _inventorySlots = new List<InventorySlot>();

    //[SerializeField] private PC _playerController;
    // using System;
    // using System.Collections.Generic;
    // using UnityEngine;
    // using UnityEngine.EventSystems;
    // using UnityEngine.InputSystem;
    // using UnityEngine.UI;

    // public class InventoryBaseManager : InventoryManager
    // {
    //[SerializeField] private Dictionary<string, int> _baseInventory = new Dictionary<string, int>();

    //public InventorySlot _highlightSlot;
    //public InventorySlot _SelectedSlot;
    //private bool _hasOneSlotSelected;
    //public bool _inventoryBaseSelected;
    //[SerializeField] private PC _playerController;

    //[SerializeField] private EventSystem _eventSystem;
    //[SerializeField] GraphicRaycaster _raycaster;
    //[SerializeField] private GameObject _dragNDrop;
    //[SerializeField] private Color selectedColor;

    //private float _cooldown;

    [SerializeField] public List<_itemSlot> _inventoryItems = new List<_itemSlot>();
    public int _inventorySize;
    [System.Serializable]
    public struct _itemSlot
    {
        public string _name;
        public int _number;
        public Sprite _sprite;
        public string Name { set { _name = value; } }
        public int Number { set { _number = value; } }
        public Sprite Sprite { set { _sprite = value; } }
    }

    private void Awake()
    {
        instance = this;
        //_playerController = GetComponentInParent<PC>();
        //_eventSystem = FindObjectsOfType<EventSystem>()[0];
        //_raycaster = GetComponentInParent<GraphicRaycaster>();
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

    //private void OnEnable()
    //{
    //    OpenCloseInventory(true);
    //}

    //private void OnDisable()
    //{
    //    OpenCloseInventory(false);
    //}
    // private void OpenCloseInventory(bool _open)
    // {
    //     _playerController.SetInventoryActive(_open);
    //     _playerController.SetIsInBaseInventory(_open);
    // }

    // private void Start()
    // {
    //     InventoryInitialisation();
    //     for (int i = 0; i < InventorySlotNumber(); i++)
    //     {
    //         _inventorySlots.Add(GetInventorySlot(i));
    //     }
    // }

    //public void AddItemInBase(string _name, int _number, GameObject _slot, GameObject _oldSlot)
    //{
    //    if (ListContain(_name))
    //    {
    //        _baseInventory[_name] += _number;
    //        _slot.GetComponent<InventorySlot>().SetNumber(_baseInventory[_name]);
    //    }
    //    else if (_slot.GetComponent<InventorySlot>().ItemContained() == null)
    //    {
    //        _baseInventory.Add(_name, _number);
    //        _slot.GetComponent<InventorySlot>().ChangeItem(_name, _oldSlot.GetComponent<InventorySlot>().ItemContained().ItemSprite(), false);
    //        _slot.GetComponent<InventorySlot>().SetNumber(_baseInventory[_name]);
    //    }
    //    else
    //    {
    //        _baseInventory.Add(_name, _number);
    //        AddItem(_name, _oldSlot.GetComponent<InventorySlot>().ItemContained().ItemSprite(), false, _number);
    //    }
    //    _oldSlot.GetComponent<InventorySlot>().ResetItem();
    //}

    //public void RemoveItemFromBase(string _name, GameObject _slot, GameObject _oldSlot)
    //{
    //    _slot.GetComponent<InventorySlot>().ChangeItem(_name, _oldSlot.GetComponent<InventorySlot>().ItemContained().ItemSprite(), false);
    //    _slot.GetComponent<InventorySlot>().SetNumber(NumberOfMaterial(_name));
    //    _oldSlot.GetComponent<InventorySlot>().ResetItem();
    //    _baseInventory.Remove(_name);
    //}


    //private void CheckIfASlotIsSelected()
    //{
    //    for (int i = 0; i < _inventorySlots.Count; i++)
    //    {
    //        if (_inventorySlots[i]._isSeleceted)
    //        {
    //            SetSelectedSlot(_inventorySlots[i], true);
    //            return;
    //        }
    //    }
    //    SetSelectedSlot(_inventorySlots[0], true);
    //    _inventorySlots[0]._isSeleceted = true;
    //}

    //private void SetSelectedSlot(InventorySlot _slot, bool _isInBaseInventory)
    //{
    //    _hasOneSlotSelected = true;
    //    _highlightSlot = _slot;
    //    SetUnselectedColorForAll();
    //    _playerController._inventory.GetComponent<InventoryManager>().UnSelectionAll();
    //    _highlightSlot.GetComponent<Image>().color = selectedColor;
    //    if(_SelectedSlot != null)
    //        _SelectedSlot.GetComponent<Image>().color = Color.red;
    //    _inventoryBaseSelected = _isInBaseInventory;
    //}

    //private void SetUnselectedColorForAll()
    //{
    //    for(int i = 0; i < _inventorySlots.Count; i++)
    //    {
    //        _inventorySlots[i].GetComponent<Image>().color = Color.black;
    //    }
    //}

    //private Vector2 NormalizeDirectionalVector(Vector2 _direction)
    //{
    //    if(_direction.y > 0)
    //    {
    //        if(Math.Abs(_direction.y) > Math.Abs(_direction.x))
    //        {
    //            return Vector2.up;
    //        }
    //        else
    //        {
    //            if(_direction.x < 0)
    //            {
    //                return Vector2.left;
    //            }
    //            else
    //            {
    //                return Vector2.right;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        if (Math.Abs(_direction.y) > Math.Abs(_direction.x))
    //        {
    //            return Vector2.down;
    //        }
    //        else
    //        {
    //            if (_direction.x < 0)
    //            {
    //                return Vector2.left;
    //            }
    //            else
    //            {
    //                return Vector2.right;
    //            }
    //        }
    //    }
    //}

    //private void TestDebug(Vector2 _dir)
    //{
    //    if(_dir == Vector2.left)
    //    {
    //        print("left");
    //    }
    //    else if (_dir == Vector2.right)
    //    {
    //        print("right");
    //    }
    //    else if (_dir == Vector2.up)
    //    {
    //        print("up");
    //    }
    //    else if (_dir == Vector2.down)
    //    {
    //        print("down");
    //    }
    //}

    //public void Selection(InputAction.CallbackContext ctx)
    //{
    //    print("a");
    //    SwitchWithManette();
    //}

    //private void SwitchWithManette()
    //{
    //    if (_cooldown > 0)
    //        return;
    //    _cooldown = .2f;
    //    if (_inventoryBaseSelected && _SelectedSlot && _SelectedSlot.tag != _highlightSlot.tag)
    //    {
    //        print("b");
    //        AddItemInBase(_SelectedSlot._itemContained.ItemName(), _SelectedSlot.Number(), _highlightSlot.gameObject, _SelectedSlot.gameObject);
    //        _SelectedSlot = null;
    //        SetSelectedSlot(_highlightSlot, _inventoryBaseSelected);
    //    }
    //    else if (!_inventoryBaseSelected && _SelectedSlot && _SelectedSlot.tag != _highlightSlot.tag)
    //    {
    //        print("c");
    //        RemoveItemFromBase(_SelectedSlot._itemContained.ItemName(), _highlightSlot.gameObject, _SelectedSlot.gameObject);
    //        _SelectedSlot = null;
    //        SetSelectedSlot(_highlightSlot, _inventoryBaseSelected);
    //    }
    //    else if (_highlightSlot && _highlightSlot.ItemContained().ItemName() != "None")
    //    {
    //        print("d");
    //        _SelectedSlot = _highlightSlot;
    //    }
    //}

    //public void InventoryBaseManagerManette(InputAction.CallbackContext ctx)
    //{
    //    if (!_playerController._isInBaseInventory || _cooldown > 0)
    //        return;

    //    if (!_hasOneSlotSelected)
    //        CheckIfASlotIsSelected();

    //    _cooldown = .1f;
    //    print(ctx.ReadValue<Vector2>());


    //    Vector2 _direction = NormalizeDirectionalVector(ctx.ReadValue<Vector2>());
    //    print(_direction);
    //    TestDebug(_direction);
    //    PointerEventData pointerEventData = new PointerEventData(_eventSystem);

    //    pointerEventData.position = (Vector2)_highlightSlot.transform.position + (_direction * 150);

    //    List<RaycastResult> results = new List<RaycastResult>();
    //    _raycaster.Raycast(pointerEventData, results);

    //    if (results.Count > 0)
    //    {
    //        if (_inventoryBaseSelected)
    //        {
    //            //ChangeInventorySlotSeleceted(results, _direction, _itemBaseContainerTag, true);
    //        }
    //        else
    //        {
    //            //ChangeInventorySlotSeleceted(results, _direction, _itemContainerTag, false);
    //        }
    //    }
    //    else
    //    {
    //        ChangeInventory(_direction, _inventoryBaseSelected);
    //    }
    //}

    //private void ChangeInventorySlotSeleceted(List<RaycastResult> _hits, Vector2 _direction, string _tag, bool _isInBaseInventory)
    //{
    //    print(_inventoryBaseSelected);
    //    for (int i = 0; i < _hits.Count; i++)
    //    {
    //        print(_hits[i].gameObject.tag + " == " + _tag + " ? " + _hits[i].gameObject.CompareTag(_tag));
    //        if (_hits[i].gameObject.CompareTag(_tag))
    //        {
    //            SetSelectedSlot(_hits[i].gameObject.GetComponent<InventorySlot>(), _isInBaseInventory);
    //            return;
    //        }
    //    }
    //    ChangeInventory(_direction, _isInBaseInventory);
    //}

    //private void ChangeInventory(Vector2 _direction, bool _isInBaseInventory)
    //{
    //    print("pas trouver");
    //    Vector2 _changeInventoryDirection = _isInBaseInventory == true ? Vector2.left : Vector2.right;
    //    InventorySlot _nextSlot = _isInBaseInventory == true ? _playerController._inventory.GetComponent<InventoryManager>()._slotList[0].GetComponent<InventorySlot>() : _inventorySlots[0];

    //    if (_direction == _changeInventoryDirection)
    //    {
    //        print("change");
    //        SetSelectedSlot(_nextSlot, !_isInBaseInventory);
    //    }
    //}

    //private void Update()
    //{
    //    if(_cooldown > 0)
    //        _cooldown -= Time.deltaTime;

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

    private bool CheckIfParentsNotAreSame(GameObject _gameobject1, GameObject _gameobject2)
    {
        return _gameobject1.transform.parent.gameObject != _gameobject2.transform.parent.gameObject;
    }

    //private bool CheckIfHasGoodTag(GameObject _check)
    //{
    //    //return _check.CompareTag(_itemBaseContainerTag) || _check.CompareTag(_itemContainerTag);
    //}

    private void ChangeChildParent(Transform _gameObject, Transform _newParent)
    {
        _gameObject.GetChild(0).SetParent(_newParent);
        _newParent.GetChild(0).transform.localPosition = Vector3.zero;
    }
}
