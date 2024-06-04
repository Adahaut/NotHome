using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> _mapsList = new List<GameObject>();
    [SerializeField] private List<ItemObject> _usbKeysList = new List<ItemObject>();
    [SerializeField] private List<GameObject> _Buttons = new List<GameObject>();
    [SerializeField] private List<Camera> _minimapsCamera = new List<Camera>();
    [SerializeField] private List<GameObject> _cameraDeplacementZone = new List<GameObject>();

    [SerializeField] private int _cameraMaxZoom, _cameraMinZoom;
    private Vector3 _savedCameraPos;
    private Vector3 _dir;
    private int _currentMapIndex;
    private float _timer;
    bool _camActive = false;

    Vector3 _initialMousePos;


    private InventoryManager _playerInventory;

    private void Awake()
    {
        _playerInventory = GetComponentInParent<PC>()._inventory.GetComponent<InventoryManager>();
    }

    private void OnEnable()
    {
        _camActive = false;
        DesactiveAllMaps();
        SetActiveButtons();
        GetComponentInParent<PC>().gameObject.GetComponentInChildren<PlayerInput>().actions.actionMaps[3].Enable();
    }

    private void OnDisable()
    {
        GetComponentInParent<PC>().gameObject.GetComponentInChildren<PlayerInput>().actions.actionMaps[3].Disable();
        ResetCamera();
    }

    private void SetActiveButtons()
    {
        for(int i = 0; i < _Buttons.Count; i++)
        {
            print(CheckForValidKey(_usbKeysList[i].ItemName()));
            _Buttons[i].SetActive(CheckForValidKey(_usbKeysList[i].ItemName()));
        }
    }

    private void DesactiveAllMaps()
    {
        for (int i = 0; i < _mapsList.Count; i++)
        {
            _mapsList[i].SetActive(false);
        }
    }

    private bool CheckForValidKey(string _keyName)
    {
        for(int i = 0; i < _playerInventory._slotList.Count; i++)
        {
            if(_playerInventory._slotList[i].GetComponent<InventorySlot>().ItemContained().ItemName() == _keyName)
            {
                return true;
            }
        }
        return false;
    }

    public void ZoomInMinimap(InputAction.CallbackContext ctx)
    {
        if (!(_currentMapIndex >= 0 && _currentMapIndex < 4) || _timer > 0)
            return;

        _timer = 0.05f;
        Camera _currentCamera = _minimapsCamera[_currentMapIndex];
        float _scrollValue = ctx.ReadValue<Vector2>().y;
        _scrollValue = _scrollValue > 0f ? -1f : 1f;

        if (_currentCamera.fieldOfView > _cameraMinZoom && _currentCamera.fieldOfView <= _cameraMaxZoom)
            _currentCamera.fieldOfView += _scrollValue;
            

        if (_currentCamera.fieldOfView < _cameraMinZoom)
        {
            print("inferieur");
            _currentCamera.fieldOfView = _cameraMinZoom;
        }

        else if (_currentCamera.fieldOfView > _cameraMaxZoom)
        {
            print("sup");
            _currentCamera.fieldOfView = _cameraMaxZoom;
        }

        print(_currentCamera.fieldOfView);
        AddCameraZoneScale(_scrollValue, _currentCamera);
    }


    private void AddCameraZoneScale(float _value, Camera _currentCamera)
    {
        if (_currentCamera.fieldOfView < _cameraMinZoom || _currentCamera.fieldOfView > _cameraMaxZoom)
            return;

        _cameraDeplacementZone[_currentMapIndex].transform.localScale = _cameraDeplacementZone[_currentMapIndex].transform.localScale - new Vector3(_value, _value, _value);

    }

    private void Timer()
    {
        if (_timer > 0) 
            _timer -= Time.deltaTime;
    }

    private void Update()
    {
        Timer();
        if (_currentMapIndex < _minimapsCamera.Count - 1)
            CamDeplacement(_minimapsCamera[_currentMapIndex]);
        
    }

    private void CamDeplacement(Camera _currentCamera)
    {
        if (!_camActive || _currentCamera == null)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            _initialMousePos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            _dir = new Vector3(Input.mousePosition.x - _initialMousePos.x,
                Input.mousePosition.y - _initialMousePos.y, Input.mousePosition.z - _initialMousePos.z);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _initialMousePos = Vector3.zero;
            _dir = Vector3.zero;
        }
        print("initial pos : " + _initialMousePos + " mouse current pos " + Input.mousePosition + " direction " + _dir);
        Vector3 _camPos = _currentCamera.transform.position;
        if(_dir != Vector3.zero)
        {
            _currentCamera.transform.position = new Vector3(_camPos.x - _dir.x, _camPos.y, _camPos.z - _dir.y);
            _initialMousePos = Input.mousePosition;
            _dir = Vector3.zero;
            _currentCamera.GetComponent<CameraMapLocker>()._lastPosition = _currentCamera.transform.position;
        }
    }

    private void ResetCamera()
    {
        Camera _currentCamera = _minimapsCamera[_currentMapIndex];
        _currentCamera.fieldOfView = _cameraMaxZoom;
        _currentCamera.transform.position = _savedCameraPos;
    }

    public void OpenMapByIndex(int _index)
    {
        if (_savedCameraPos != Vector3.zero && (_currentMapIndex < _minimapsCamera.Count - 1))
            ResetCamera();
        DesactiveAllMaps();
        _mapsList[_index].SetActive(true);
        _currentMapIndex = _index;
        if (_currentMapIndex < _minimapsCamera.Count - 1)
            _savedCameraPos = _minimapsCamera[_currentMapIndex].transform.position;
        _camActive = true;
    }

}
