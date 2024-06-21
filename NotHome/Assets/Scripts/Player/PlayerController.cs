using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{
    [Header("Transform" + "\n")]
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _startPointRaycast;

    [Header("Value" + "\n")]
    public float _sensitivity;
    [SerializeField] private float _sensitivityController;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _jumpPower;
    [SerializeField] private float _gravity;
    public bool _canMove = true;

    [Header("Player UI")]
    [SerializeField] private GameObject playerUiCanvas;
    [SerializeField] private GameObject droneUI;
    [SerializeField] private Image fillDroneBar;
    [SerializeField] private float _distRayCast;
    [SerializeField] private TextMeshProUGUI _textPress;
    [SerializeField] private List<GameObject> _uiPlayer;
    private bool _canOpen;
    private bool _isOpen;

    [Header("Inventory")]
    [SerializeField] public GameObject _inventory;
    [SerializeField] private string _itemTag;
    [SerializeField] private int _itemPickRange;
    private InventorySlot _slotSelected;
    private bool _isInInventory;
    private Vector3 _inventoryInitialPosition;
    [SerializeField] private Vector3 _inventoryBasePosition;

    [Header("HotBar")]
    [SerializeField] private GameObject _hotBar;

    [Header("PlayerManager")]
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private float _staminaTimer;
    [SerializeField] private float _currentStaminaTime;
    [SerializeField] private bool _staminaRegenStarted;
    [SerializeField] private bool _runningStaminaLose;


    [Header("Book")]
    [SerializeField] private GameObject _book;
    public bool _isInBook;


    public bool _canAttack = true;
    private Rigidbody _rigidbodyPlayer;
    private bool _isGrounded;
    private float _initSpeed;
    private bool _isRunning;
    private bool _isJump;
    private CharacterController _characterController;
    private float _timer;
    private bool _isInBaseInventory;
    private Animator _animator;


    private Vector3 _moveDirection = Vector3.zero;
    private Vector2 _rotation = Vector2.zero;
    private Vector2 _rotation2 = Vector2.zero;
    private Vector2 _moveDir;
    private Vector2 _scrollDir;

    private Farts _farts;
    private float _fartCooldown;

    private bool _canUseTorch;
    [SerializeField] private GameObject _torch;
    public GameObject _weapon;
    [SerializeField] private GameObject _animCam;
    [SerializeField] private float _speedAnimWeapon;
    [SerializeField] private float _speedAnimCam;

    [Range(0f, 90f)][SerializeField] float yRotationLimit = 88f;
    private Transform _transform;

    public Vector2 Rotation { get { return _rotation2; } set { _rotation2 = value; } }

    public GameObject playerMesh;
    public GameObject gunMesh;
    public GameObject machette;
    public GameObject cameraGunMesh;

    public bool IsDead;
    bool _canJump;

    [SerializeField] private ChangeControl _changeControl;
    [SerializeField] private GameObject[] _setActiveFalse;
    public PauseManager _pauseManager;

    private void Start()
    {
        StartCoroutine(DisableControlPanelOnStart());
    }

    private IEnumerator DisableControlPanelOnStart()
    {
        yield return new WaitForSeconds(0.01f);

        for (int i = 0; i < _setActiveFalse.Length; i++)
        {
            _setActiveFalse[i].SetActive(false);
        }
        _pauseManager.Resume();
    }
    public override void OnStartAuthority()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        _animCam.GetComponent<Animator>().speed = _speedAnimCam;
        _weapon.GetComponent<Animator>().speed = _speedAnimWeapon;
        _animator = GetComponentInChildren<Animator>();
        _characterController = GetComponent<CharacterController>();
        _playerManager = GetComponent<PlayerManager>();
        _farts = GetComponent<Farts>();
        _transform = transform;
        Cursor.lockState = CursorLockMode.Locked;

        _camera.gameObject.SetActive(true);
        enabled = true;

        if(isOwned)
        {
            playerUiCanvas.SetActive(true);
            GetComponent<AudioListener>().enabled = true;

            playerMesh.SetActive(false);
            gunMesh.SetActive(false);
            machette.SetActive(false);
        }

        _inventoryInitialPosition = _inventory.transform.localPosition;
    }

    void CmdSendPositionToServer(Vector3 position, Quaternion cameraRotation)
    {
        transform.position = position;
        _camera.rotation = cameraRotation;

        RpcUpdatePositionOnClients(position, cameraRotation);
    }

    [ClientRpc]
    void RpcUpdatePositionOnClients(Vector3 position, Quaternion cameraRotation)
    {
        if (!isOwned)
        {
            transform.position = position;
            _camera.rotation = cameraRotation;
        }
    }

    public void OpenInventory(InputAction.CallbackContext ctx)
    {
        if(isOwned)
        {
            if (ctx.started)
            {
                _inventory.SetActive(true);
                _isInInventory = true;
            }
            if (ctx.canceled)
            {
                _inventory.SetActive(false);
                _isInInventory = false;
            }  
        }
    }
    public void SetIsInBaseInventory(bool _isIn)
    {
        _isInBaseInventory = _isIn;
    }

    public InventoryManager GetInventory()
    {
        return _inventory.GetComponent<InventoryManager>();
    }

    public void SetInventoryActive(bool _active)
    {
        _inventory.SetActive(_active);
    }

    public void SetInventoryPosition(bool _openning)
    {
        if (_openning)
            _inventory.transform.localPosition = _inventoryBasePosition;
        else
            _inventory.transform.localPosition = _inventoryInitialPosition;
    }

    public void OpenMenuPause(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            PauseManager.Instance.Resume();
    }
    public void Interaction(InputAction.CallbackContext ctx)
    {
        if(isOwned)
        {
            DoorExit.Instance.OpenDoor(_startPointRaycast, _distRayCast);
            if (ctx.performed)
            {
                StartUi();
                if (Ladder.Instance != null)
                    Ladder.Instance.TpLadder(_startPointRaycast, _distRayCast, this);
            }
            //OfficeManager.Instance.MouvToChair();
            if (_timer <= 0)
            {
                CmdPickUpObject();
                _timer = 0.05f;
            }
        }
        
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(isOwned)
        {
            if (_characterController.isGrounded && _playerManager.Stamina >= 10 && context.performed && !_isOpen)
            {
                StartCoroutine(AnimOneTime("StartJump"));
                ChangeStamina(-10);
                _currentStaminaTime = _staminaTimer;
                _canJump = true;
                _isJump = true;
            }
        }
        
    }
    void Update()
    {
        if(isOwned)
        {
            RotateCamera();
            MovePlayer();
            Timer();
            CmdSendPositionToServer(transform.position, _camera.rotation);

            if (!_staminaRegenStarted && CanRegenStamina())
            {
                StartCoroutine(RegenStamina());
            }

            if (Physics.Raycast(_startPointRaycast.position, _startPointRaycast.forward, out RaycastHit hit, _distRayCast) && (hit.collider.gameObject.layer == 8 || hit.collider.gameObject.layer == 6
                || hit.collider.CompareTag("Decompression") || hit.collider.CompareTag("Ladder")))
            {
                _textPress.text = "Press " + _changeControl._control.ToUpper() + " to interact";

                _canOpen = true;
            }
            else
            {
                _canOpen = false;
                _textPress.text = "";
            }
        }
    }

    public void SprintPlayer(InputAction.CallbackContext context)
    {
        if(isOwned)
        {

            _isRunning = true;
            if (context.performed)
            {
            _weapon.GetComponent<Animator>().speed *= 2;
            _animCam.GetComponent<Animator>().speed *= 2;
            }
            if (_moveDir != Vector2.zero)
                _animator.SetBool("Run", true);
            if (context.canceled || _playerManager.Stamina <= 0)
            {
            _weapon.GetComponent<Animator>().speed = 1f;
            _animCam.GetComponent<Animator>().speed = 1f;
                _isRunning = false;
                _animator.SetBool("Run", false);
            }
        }
        
    }

    private void Timer()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
        if (_currentStaminaTime > 0)
        {
            _currentStaminaTime -= Time.deltaTime;
        }
    }

    public void GetMouseDelta(InputAction.CallbackContext ctx)
    {
        if(isOwned)
        {
            if (ctx.control.name == "rightStick")
                _rotation = ctx.ReadValue<Vector2>() * _sensitivityController;
            else
                _rotation = ctx.ReadValue<Vector2>() * _sensitivity;
        }
        
    }
    private void RotateCamera()
    {
        _rotation2.x += _rotation.x * Time.deltaTime;
        _rotation2.y -= _rotation.y * Time.deltaTime;
        _rotation2.y = Mathf.Clamp(_rotation2.y, -yRotationLimit, yRotationLimit);
        if (_transform == null)
            _transform = transform;
        _transform.localEulerAngles = new Vector3(0, _rotation2.x, 0);
        _camera.localEulerAngles = new Vector3(_rotation2.y, 0, 0);
    }
    public void GetInputPlayer(InputAction.CallbackContext ctx)
    {
        if(isOwned)
        {
            _moveDir = ctx.ReadValue<Vector2>();
            if (_moveDir != Vector2.zero)
            {
                _animCam.GetComponent<Animator>().enabled = true;
                _weapon.GetComponent<Animator>().enabled = true;
                if (GetComponentInChildren<RangeWeapon>() != null)
                    if (GetComponentInChildren<RangeWeapon>()._isAiming)
                        _weapon.GetComponent<Animator>().enabled = false;

            }
            else
            {
            _animCam.GetComponent<Animator>().enabled = false;
            _weapon.GetComponent<Animator>().playbackTime = 0;
            _weapon.GetComponent<Animator>().enabled = false;
            }
        }
        
    }
    private void MovePlayer()
    {
        Vector3 forward = _transform.TransformDirection(Vector3.forward);
        Vector3 right = _transform.TransformDirection(Vector3.right);

        float curSpeedX = _canMove ? (_isRunning ? _runSpeed : _walkSpeed) * _moveDir.y : 0;
        float curSpeedY = _canMove ? (_isRunning ? _runSpeed : _walkSpeed) * _moveDir.x : 0;

        _animator.SetFloat("y", curSpeedX / 10f);
        _animator.SetFloat("x", curSpeedY / 10f);

        float movementDirectionY = _moveDirection.y;
        _moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (_canJump && _canMove && _characterController.isGrounded)
        {
            _moveDirection.y = _jumpPower;
            _canJump = false;
        }
        else
        {
            if (_isRunning)
            {
                if (!_runningStaminaLose && _moveDir != Vector2.zero)
                {
                    StartCoroutine(RunningStamina());
                }
                if (_playerManager.Stamina <= 0)
                {
                    _isRunning = false;
                }
            }
            _moveDirection.y = movementDirectionY;

        }

        if (!_characterController.isGrounded)
        {
            _animator.SetBool("Falling", true);
            _moveDirection.y -= _gravity * Time.deltaTime;
        }
        else
        {
            _animator.SetBool("Falling", false);
            if (_isJump)
            {
                StartCoroutine(AnimOneTime("EndJump"));
                _isJump = false;
            }
        }
        if (_canMove)
            _characterController.Move(_moveDirection * Time.deltaTime);
    }

    public IEnumerator AnimOneTime(string name)
    {
        _animator.SetBool(name, true);
        yield return new WaitForSeconds(0.25f);
        _animator.SetBool(name, false);
    }

    public void SetAnimation(string name, bool state)
    {
        _animator.SetBool(name, state);
    }

    public void MouseScrollY(InputAction.CallbackContext ctx)
    {
        if(isOwned)
        {
            if (_timer > 0)
                return;
            _scrollDir = ctx.ReadValue<Vector2>();
            int _indexAddition = 0;
            if (_scrollDir.y > 0) _indexAddition = 1;
            else if (_scrollDir.y < 0) _indexAddition = -1;

            if (_isInInventory)
            {
                if(_slotSelected == null)
                    _slotSelected = _inventory.GetComponent<InventoryManager>().SelectAt(0);
                else
                {
                    _slotSelected = _inventory.GetComponent<InventoryManager>().SelectAt(_inventory.GetComponent<InventoryManager>().IndexOfSlot(_slotSelected) + _indexAddition);
                }
            }
            else
            {
                CheckIfHotBarIsShowed();
                ChangeToHotBarSlot(UpdateHotBarIndex(_hotBar.GetComponent<HotBarManager>()._hotBarSlotIndex, _indexAddition));
                
            }
            _timer = 0.1f;
        }
        
    }

    public void HotBarSelection1(InputAction.CallbackContext ctx)
    {
        if(isOwned)
        {
            CheckIfHotBarIsShowed();
            ChangeToHotBarSlot(0);
        }
        
    }

    public void HotBarSelection2(InputAction.CallbackContext ctx)
    {
        if(isOwned)
        {
            CheckIfHotBarIsShowed();
            ChangeToHotBarSlot(1);
        }
        
    }

    public void HotBarSelection3(InputAction.CallbackContext ctx)
    {
        if(isOwned)
        {
            CheckIfHotBarIsShowed();
            ChangeToHotBarSlot(2);
        }
        
    }

    private void CheckIfHotBarIsShowed()
    {
        if (!_hotBar.GetComponent<HotBarManager>().IsOpen())
        {
            _hotBar.GetComponent<HotBarManager>().StartFadeInFadeOut();
        }
        _hotBar.GetComponent<HotBarManager>().ResetTimer();
    }

    private void ChangeToHotBarSlot(int _newIndex)
    {
        //_hotBar.GetComponent<HotBarManager>()._hotBarSlotIndex = _newIndex;
        SetNewIndex(_newIndex);
        _hotBar.GetComponent<HotBarManager>().UpdateSelectedHotBarSlot();
    }

    [Command]
    void SetNewIndex(int i)
    {
        _hotBar.GetComponent<HotBarManager>()._hotBarSlotIndex = i;
    }

    private int UpdateHotBarIndex(int _index, int _indexAddition)
    {
        _index -= _indexAddition;

        if (_index < 0)
        {
            _index = 2;
        }
        else if (_index > 2)
        {
            _index = 0;
        }

        CmdModifyIndex(_index);
        return _index;
    }

    [Command]
    void CmdModifyIndex(int newIndex)
    {
        _hotBar.GetComponent<HotBarManager>()._hotBarSlotIndex = newIndex;
    }

    private void CmdPickUpObject()
    {
        if(Physics.Raycast(_startPointRaycast.position, _startPointRaycast.forward, out RaycastHit hit, _distRayCast) && hit.collider.CompareTag(_itemTag))
        {
            if (!_inventory.GetComponent<InventoryManager>().HasRemainingPlace(hit.collider.GetComponent<Item>().ItemName()))
            {
                return;
            }
            if (hit.collider.GetComponent<Item>().ItemName() == "Metal")
                QuestManager.Instance.SetQuestMetal();
            _inventory.GetComponent<InventoryManager>().AddItem(hit.collider.GetComponent<Item>().ItemName(), hit.collider.GetComponent<Item>().ItemSprite(), false);
            CmdDestroyItem(hit.collider.gameObject);
        }
    }

    [Command]
    private void CmdDestroyItem(GameObject item)
    {
        print("destroy " +  item.name);
        NetworkServer.Destroy(item);
    }


    private void ChangeStamina(float _value)
    {
        _playerManager.Stamina += _value;
        _playerManager.SetStaminaBar();
    }

    private IEnumerator RegenStamina()
    {
        _staminaRegenStarted = true;
        while (CanRegenStamina())
        {
            ChangeStamina(_playerManager.MaxStamina / 100);
            if (_playerManager.Stamina > _playerManager.MaxStamina)
            {
                _playerManager.SetMaxStamina(_playerManager.MaxStamina);
            }
            yield return new WaitForSeconds(0.05f);
        }
        _staminaRegenStarted = false;
    }

    private IEnumerator RunningStamina()
    {
        _runningStaminaLose = true;
        while (_isRunning && _playerManager.Stamina > 0)
        {
            _currentStaminaTime = _staminaTimer;
            ChangeStamina(-0.05f);
            yield return new WaitForSeconds(0.01f);
        }
        _runningStaminaLose = false;
    }


    private bool CanRegenStamina()
    {
        return _currentStaminaTime <= 0 && _playerManager.Stamina <= _playerManager.MaxStamina;
    }

    // Ui Player
    public void StartUi()
    {
        if (_canOpen && Physics.Raycast(_startPointRaycast.position, _startPointRaycast.forward, out RaycastHit hit, _distRayCast))
        {

            if(hit.collider.GetComponent<BuildInterractable>())
            {
                if (hit.collider.GetComponentInChildren<ParticleSystem>() != null && hit.collider.GetComponent<BuildInterractable>()._index < 9)
                {
                    _uiPlayer[hit.collider.GetComponent<BuildInterractable>()._index].GetComponent<UpgradeHomeManager>()._particleLevelUp = hit.collider.GetComponentInChildren<ParticleSystem>();
                }
                OpenUi(hit.collider.GetComponent<BuildInterractable>()._index);
                hit.collider.GetComponent<BuildInterractable>()._isOpen = true;
                return;
            }
            
            if(hit.collider.GetComponent<DroneManager>() && DroneManager._canUseDrone)
            {
                SetAuthorityToDrone(hit.collider.gameObject);
                DroneManager drone = hit.collider.GetComponent<DroneManager>();
                drone.StartDrone(GetComponentInChildren<FollowCamera>().cam, GetComponentInChildren<PlayerInput>(), this.gameObject, fillDroneBar);
                if(isOwned)
                {
                    playerUiCanvas.SetActive(false);
                    droneUI.SetActive(true);
                    playerMesh.SetActive(true);
                    cameraGunMesh.SetActive(false);
                }
            }
        }
    }

    public void EnableCanvasAfterUsingDrone()
    {
        if (isOwned)
        {
            playerUiCanvas.SetActive(true);
            droneUI.SetActive(false);
            playerMesh.SetActive(false);
            cameraGunMesh.SetActive(true);
        }
    }

    [Command]
    void SetAuthorityToDrone(GameObject c)
    {
        BuildingManager.instance.AssignAuthority(connectionToClient, c);
    }

    private void FixedUpdate()
    {
        if (_fartCooldown > 0)
            _fartCooldown -= Time.deltaTime;
    }

    public void OpenUi(int index)
    {
        _uiPlayer[index].SetActive(!_uiPlayer[index].activeSelf);
        DisablePlayer(_uiPlayer[index].activeSelf);
    }

    public void DisablePlayer(bool active)
    {
        if (active)
        {
            _canAttack = false;
            Cursor.lockState = CursorLockMode.None;
            GetComponentInChildren<PlayerInput>().actions.actionMaps[0].Disable();
            _isOpen = true;
        }
        else
        {
            _canAttack = true;
            Cursor.lockState = CursorLockMode.Locked;
            GetComponentInChildren<PlayerInput>().actions.actionMaps[0].Enable();
            _isOpen = false;
        }
    }

    public void StartFart(InputAction.CallbackContext ctx)
    {
        if(isOwned)
        {
            if (_fartCooldown > 0)
                return;

            _fartCooldown = 1f;
            _farts.PlayRandomFartSound();
        }
        
    }

    public void AlightTorch(InputAction.CallbackContext ctx)
    {
        if (isOwned && _canUseTorch && ctx.performed)
        {
            _torch.SetActive(!_torch.activeSelf);
        }
    }
    public Vector2 GetMoveDir()
    {
        return _moveDir;
    }

    public void OpenBook(InputAction.CallbackContext ctx)
    {
        if(isOwned)
        {
            //_book.SetActive(!_book.activeInHierarchy);
            //if (_book.activeInHierarchy)
            //{
            //    Cursor.lockState = CursorLockMode.Confined;
            //    _isInBook = true;
            //}
            //else
            //{
            //    Cursor.lockState = CursorLockMode.Locked;
            //    _isInBook = false;
            //}
        }
        
    }

    public void DropItem(InputAction.CallbackContext ctx)
    {
        if(_slotSelected == null || _slotSelected.ItemContained().ItemName() == "None" || _timer > 0)
            return;
        for(int i = 0; i < _slotSelected.Number(); i++)
        {
            print(_inventory.GetComponent<InventoryManager>().GetItemPrefab(_slotSelected.ItemContained().ItemName()));
            GameObject _droppedItem = Instantiate(_inventory.GetComponent<InventoryManager>().GetItemPrefab(_slotSelected.ItemContained().ItemName()), 
                GenerateRandomSpawnPoint(-1.5f, 1.5f), Quaternion.identity);
            NetworkServer.Spawn(_droppedItem);
        }

        _slotSelected.ResetItem();
        _timer = 0.01f;
    }

    private Vector3 GenerateRandomSpawnPoint(float _minFaxtor, float _maxFactor)
    {
        Vector3 _pos = _transform.position;
        return new Vector3(_pos.x + Random.Range(_minFaxtor, _maxFactor), _pos.y, _pos.z + Random.Range(_minFaxtor, _maxFactor));
    }


}
