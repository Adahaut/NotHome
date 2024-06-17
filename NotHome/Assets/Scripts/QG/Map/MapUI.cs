using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MapUI : MonoBehaviour
{
    private List<GameObject> _mapsList = new List<GameObject>();
    private List<ItemObject> _usbKeysList = new List<ItemObject>();
    private List<GameObject> _Buttons = new List<GameObject>();
    [SerializeField] private List<Camera> _minimapsCamera = new List<Camera>();
    [SerializeField] private RawImage _mapImage;
    [SerializeField] private List<RenderTexture> _minimapsRenderSprite = new List<RenderTexture>();

    [SerializeField] private int _cameraMaxZoom, _cameraMinZoom;
    private Vector3 _dir;
    [SerializeField] private int _currentMapIndex;
    private float _timer;
    bool _camActive = false;

    private Vector3 _camZone;
    private Vector3 _camLastPos;

    Vector3 _initialMousePos;


    private InventoryManager _playerInventory;

    private void Awake()
    {
        _playerInventory = GetComponentInParent<PlayerController>()._inventory.GetComponent<InventoryManager>();
    }

    private void OnEnable()
    {
        DesactiveAllMaps();
        SetActiveButtons();
        GetComponentInParent<PlayerController>().gameObject.GetComponentInChildren<PlayerInput>().actions.actionMaps[3].Enable();
        _currentMapIndex = _minimapsRenderSprite.IndexOf((RenderTexture)_mapImage.texture);
    }

    private void OnDisable()
    {
        GetComponentInParent<PlayerController>().gameObject.GetComponentInChildren<PlayerInput>().actions.actionMaps[3].Disable();
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

        _timer = 0.01f;
        Camera _currentCamera = _minimapsCamera[_currentMapIndex];
        float _scrollValue = ctx.ReadValue<Vector2>().y;
        if(_scrollValue != 0)
            _scrollValue = _scrollValue > 0f ? -1f : 1f;

        if (_currentCamera.fieldOfView >= _cameraMinZoom && _currentCamera.fieldOfView <= _cameraMaxZoom)
            _currentCamera.fieldOfView += _scrollValue;
            

        if (_currentCamera.fieldOfView < _cameraMinZoom)
        {
            _currentCamera.fieldOfView = _cameraMinZoom;
        }

        else if (_currentCamera.fieldOfView > _cameraMaxZoom)
        {
            _currentCamera.fieldOfView = _cameraMaxZoom;
        }

        CalculateCamDeplacementZone(_currentCamera.fieldOfView);
        if (!CamIsInZone(_currentCamera.transform.localPosition))
        {
            if (_currentCamera.fieldOfView == _cameraMaxZoom)
            {
                _currentCamera.transform.localPosition = Vector3.zero;
                return;
            }

            Vector2 _axeOut = CheckWichAxeIsOut(_currentCamera.transform.position);
            Vector3 _newPos = _currentCamera.transform.localPosition;

            if (_axeOut.x == -1)
            {
                _newPos.x = _camZone.x;
            }
            else if (_axeOut.x == 1)
            {
                _newPos.x = -(_camZone.x);
            }

            if (_axeOut.y == -1)
            {
                _newPos.z = _camZone.z;
            }
            else if (_axeOut.y == 1)
            {
                _newPos.z = -(_camZone.z);
            }
            
            _currentCamera.transform.localPosition = _newPos;
        }
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
        if (_currentCamera == null)
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
        Vector3 _camPos = _currentCamera.transform.localPosition;
        if(_dir != Vector3.zero)
        {
            _currentCamera.transform.localPosition = new Vector3(_camPos.x - _dir.x, _camPos.y, _camPos.z - _dir.y);
            _initialMousePos = Input.mousePosition;
            _dir = Vector3.zero;
        }
        if (!CamIsInZone(_currentCamera.transform.localPosition))
        {
            _currentCamera.transform.localPosition = _camLastPos;
        }
        _camLastPos = _currentCamera.transform.localPosition;
    }

    private void CalculateCamDeplacementZone(float _camFOV)
    {
        _camZone = 15f * new Vector3
        {
            x = _cameraMaxZoom - _camFOV,
            y = 0,
            z = _cameraMaxZoom - _camFOV
        };
    }

    private bool CamIsInZone(Vector3 _camPosition)
    {
        return (_camPosition.x <= _camZone.x && _camPosition.x >= -(_camZone.x)) && (_camPosition.z <= _camZone.z && _camPosition.z >= -(_camZone.z));
    }

    private Vector2 CheckWichAxeIsOut(Vector3 _camPosition)
    {
        Vector2 _axeOut = Vector2.zero;
        if (_camPosition.x > _camZone.x)
            _axeOut.x = 1;
        else if (_camPosition.x < -(_camZone.x))
            _axeOut.x = -1;

        if (_camPosition.z > _camZone.z)
            _axeOut.y = 1;
        else if (_camPosition.z < -(_camZone.z))
            _axeOut.y = -1;

        return _axeOut;
    }

    private void ResetCamera()
    {
        Camera _currentCamera = _minimapsCamera[_currentMapIndex];
        _currentCamera.fieldOfView = _cameraMaxZoom;
        _currentCamera.transform.localPosition = Vector3.zero;
    }

    public void OpenMapByIndex(int _index)
    {
        if ((_currentMapIndex < _minimapsCamera.Count - 1))
            ResetCamera();
        DesactiveAllMaps();
        _mapsList[_index].SetActive(true);
        _currentMapIndex = _index;
        _camActive = true;
    }

}
