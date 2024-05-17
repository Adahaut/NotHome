using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryBaseManager : InventoryManager
{
    [SerializeField] private Dictionary<string, int> _baseInventory = new Dictionary<string, int>();

    [SerializeField] private PlayerController _playerController;

    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] GraphicRaycaster _raycaster;
    [SerializeField] private GameObject _dragNDrop;

    [SerializeField] private string _itemContainerTag;

    private bool _draging = false;
    private GameObject _itemImage;

    private void OnEnable()
    {
        OpenCloseInventory(true);
    }

    private void OnDisable()
    {
        OpenCloseInventory(false);
    }

    private void OpenCloseInventory(bool _open)
    {
        _playerController.SetInventoryActive(_open);
        _playerController.SetIsInBaseInventory(_open);
    }

    private void Start()
    {
        InventoryInitialisation();
    }

    public void AddItemInBase(string _name, int _number, GameObject _slot, GameObject _oldSlot)
    {
        if (_baseInventory.ContainsKey(_name))
        {
            _baseInventory[_name] += _number;
        }
        else
        {
            _baseInventory.Add(_name, _number);
            _slot.GetComponent<InventorySlot>().ChangeItem(_name, _oldSlot.GetComponent<InventorySlot>().ItemContained().ItemSprite());
        }
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
                if (results[0].gameObject.CompareTag(_itemContainerTag) && CheckIfParentsNotAreSame(_itemImage, results[0].gameObject))
                {
                    print("change l'item");
                    AddItemInBase(_itemImage.GetComponent<InventorySlot>().ItemContained().ItemName(), _itemImage.GetComponent<InventorySlot>().Number(), results[0].gameObject, _itemImage);
                }
            }
            if (Input.GetMouseButtonDown(0) && !_draging)
            {
                _itemImage = results[0].gameObject;
                if (_itemImage.CompareTag(_itemContainerTag))
                {
                    _draging = true;
                    ChangeChildParent(_itemImage.transform, _dragNDrop.transform);
                }
            }
        }

    }

    private bool CheckIfParentsNotAreSame(GameObject _gameobject1, GameObject _gameobject2)
    {
        return _gameobject1.transform.parent.gameObject != _gameobject2.transform.parent.gameObject;
    }


    private void ChangeChildParent(Transform _gameObject, Transform _newParent)
    {
        _gameObject.GetChild(0).SetParent(_newParent);
        _newParent.GetChild(0).transform.localPosition = Vector3.zero;
    }

}
