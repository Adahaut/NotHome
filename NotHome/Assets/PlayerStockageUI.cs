using Org.BouncyCastle.Crypto.Macs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerStockageUI : MonoBehaviour
{
    private EventSystem _eventSystem;
    [SerializeField] GraphicRaycaster _raycaster;
    [SerializeField] private GameObject _dragNDrop;

    [SerializeField] private string _itemContainerTag;
    [SerializeField] private string _itemBaseContainerTag;

    private bool _draging = false;
    private GameObject _itemImage;

    [SerializeField] private GameObject _inventorySlotPrefab;
    [SerializeField] private GameObject _inventoryPanel;
    public List<GameObject> _slotList = new List<GameObject>();

    public int _baseInventorySlotCount = 10;

    private void OnEnable()
    {
        _eventSystem = GameObject.FindObjectOfType<EventSystem>();

        for (int i = 0; i < _baseInventorySlotCount; i++)
        {
            GameObject _newInventorySlot = Instantiate(_inventorySlotPrefab, _inventoryPanel.transform);
            _slotList.Add(_newInventorySlot);
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
                if (CheckIfHasGoodTag(results[0].gameObject) && CheckIfParentsNotAreSame(_itemImage, results[0].gameObject))
                {
                    if (results[0].gameObject.CompareTag(_itemBaseContainerTag))
                    {
                        //AddItemInBase(_itemImage.GetComponent<InventorySlot>().ItemContained().ItemName(), _itemImage.GetComponent<InventorySlot>().Number(), results[0].gameObject, _itemImage);
                    }
                    else
                    {
                        //print(_baseInventory[_itemImage.GetComponent<InventorySlot>().ItemContained().ItemName()]);
                        //RemoveItemFromBase(_itemImage.GetComponent<InventorySlot>().ItemContained().ItemName(), results[0].gameObject, _itemImage);
                    }
                }
            }
            if (!_draging && Input.GetMouseButtonDown(0) && results[0].gameObject.TryGetComponent<InventorySlot>(out InventorySlot _inventorySlot) && _inventorySlot.ItemContained().ItemName() != "None")
            {
                _itemImage = results[0].gameObject;
                if (CheckIfHasGoodTag(_itemImage))
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
    }

}
