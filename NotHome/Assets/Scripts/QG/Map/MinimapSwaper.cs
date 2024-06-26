using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MinimapSwaper : MonoBehaviour
{
    [SerializeField] private GameObject _minimapGameObject;
    [SerializeField] private RawImage _miniMapUI;

    [SerializeField] private List<Material> _miniMapsMaterials;
    [SerializeField] private List<RenderTexture> _miniMapsSprites;
    [SerializeField] private List<string> _usbKeysName;

    [SerializeField] private Material _noMinimapMaterial;
    [SerializeField] private RenderTexture _noMinimapSprite;

    private InventorySlot _usbKeySlot;
    private PlayerController _playerController;
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private GraphicRaycaster _raycaster;
    private GameObject _result;
    private bool _draging;

    [SerializeField] private GameObject _dragAndDrop;

    private void Awake()
    {
        _usbKeySlot = GetComponentInChildren<InventorySlot>();
        _playerController = GetComponentInParent<PlayerController>();
    }

    private void OnEnable()
    {
        if(_minimapGameObject == null)
        {
            _minimapGameObject = GameObject.Find("Minimap");
        }

        if(_playerController == null)
            _playerController = GetComponentInParent<PlayerController>();

        _playerController.SetInventoryActive(true);
        _playerController.SetInventoryPosition(true);
    }

    private void OnDisable()
    {
        _playerController.SetInventoryActive(false);
        _playerController.SetInventoryPosition(false);
    }

    private void Update()
    {
        DragAndDrop();
    }

    private void DragAndDrop()
    {
        Vector3 MousePos = Input.mousePosition;

        PointerEventData pointerEventData = new PointerEventData(_eventSystem);
        pointerEventData.position = MousePos;
        _dragAndDrop.transform.position = MousePos;

        List<RaycastResult> results = new List<RaycastResult>();

        _raycaster.Raycast(pointerEventData, results);

        if (results.Count > 0)
        {
            if(Input.GetMouseButtonUp(0) && _draging)
            {
                _draging = false;
                if(_result.CompareTag("ItemContainer") && results[0].gameObject.CompareTag("BaseItemContainer")
                    && results[0].gameObject.GetComponent<InventorySlot>().ItemContained().ItemName() == "None")
                {
                    AddUSBKey(_result.GetComponent<InventorySlot>());
                }
                else if (_result.CompareTag("BaseItemContainer") && results[0].gameObject.CompareTag("ItemContainer")
                    && results[0].gameObject.GetComponent<InventorySlot>().ItemContained().ItemName() == "None")
                {
                    RemoveUSBKey(results[0].gameObject.GetComponent<InventorySlot>());
                }
                ReturnImage(_result.GetComponent<InventorySlot>());
            }
            if (!_draging && Input.GetMouseButtonDown(0) && results[0].gameObject.TryGetComponent<InventorySlot>(out InventorySlot _inventorySlot) && _inventorySlot.ItemContained().ItemName() != "None")
            {
                _result = results[0].gameObject;
                _draging = true;
                _inventorySlot._itemImage.transform.SetParent(_dragAndDrop.transform);
            }
        }
    }

    private void AddUSBKey(InventorySlot _playerInventorySlot)
    {
        if (!_usbKeysName.Contains(_playerInventorySlot.ItemContained().ItemName()))
            return;
        _usbKeySlot.ChangeItem(_playerInventorySlot.ItemContained().ItemName(), _playerInventorySlot.ItemContained().ItemSprite(), false);
        _playerInventorySlot.ResetItem();
        EnableMap(_usbKeySlot.ItemContained().ItemName());
    }

    private void RemoveUSBKey(InventorySlot _playerInventorySlot)
    {
        _playerInventorySlot.ChangeItem(_usbKeySlot.ItemContained().ItemName(), _usbKeySlot.ItemContained().ItemSprite(), false);
        _usbKeySlot.ResetItem();
        DisableMap();
    }

    private void EnableMap(string mapName)
    {
        int _index = _usbKeysName.IndexOf(mapName);
        print(_index);
        _minimapGameObject.GetComponent<MeshRenderer>().material = _miniMapsMaterials[_index];
        _miniMapUI.texture = _miniMapsSprites[_index];
    }

    private void DisableMap()
    {
        _minimapGameObject.GetComponent<MeshRenderer>().material = _noMinimapMaterial;
        _miniMapUI.texture = _noMinimapSprite;
    }

    private void ReturnImage(InventorySlot _inventorySlot)
    {
        _inventorySlot._itemImage.transform.SetParent(_inventorySlot.transform);
        _inventorySlot._itemImage.transform.localPosition = Vector3.zero;
    }

}
