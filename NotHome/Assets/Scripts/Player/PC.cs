using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PC : MonoBehaviour
{
    [Header("Transform" + "\n")]
    [SerializeField] private Transform _camera;

    [Header("Value" + "\n")]
    [SerializeField] private float _sensitivity;
    [SerializeField] private float _sensitivityController;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _jumpPower;
    [SerializeField] private float _gravity;
    public bool _canMove = true;

    [Header("Player UI")]
    [SerializeField] private float _distRayCast;
    [SerializeField] private TextMeshProUGUI _textPress;
    [SerializeField] private List<GameObject> _uiPlayer;
    private bool _canOpen;
    private bool _isOpen;

    [Header("Inventory")]
    [SerializeField] public GameObject _inventory;
    [SerializeField] private string _itemTag;
    [SerializeField] private int _itemPickRange;

    [Header("HotBar")]
    [SerializeField] private GameObject _hotBar;

    [Header("Book")]
    [SerializeField] private GameObject _book;

    [Header("PlayerManager")]
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private float _staminaTimer;
    [SerializeField] private float _currentStaminaTime;
    [SerializeField] private bool _staminaRegenStarted;
    [SerializeField] private bool _runningStaminaLose;

    private Farts _farts;

    private float _fartCooldown;
    private bool _isInBook;
    private bool _isDead;
    private Rigidbody _rigidbodyPlayer;
    private bool _isGrounded;
    private float _initSpeed;
    private bool _isRunning;
    private bool _isJump;
    private bool _canUseTorch;
    private CharacterController _characterController;
    private float _timer;
    private bool _isInBaseInventory;
    [SerializeField] private GameObject _torch;

    private Vector3 _moveDirection = Vector3.zero;
    private Vector2 _rotation = Vector2.zero;
    private Vector2 _rotation2 = Vector2.zero;
    private Vector2 _moveDir;
    private Vector2 _scrollDir;
    
    [Range(0f, 90f)][SerializeField] float yRotationLimit = 88f;
    private Transform _transform;

    public Vector2 Rotation { get { return _rotation2; } set {  _rotation2 = value; } }

    public bool IsDead {  get { return _isDead; } set {  _isDead = value; } }

    public bool IsInBook { get { return  _isInBook; } }

    public static PC Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }


    public void Start()
    {
        _playerManager = GetComponent<PlayerManager>();
        _transform = transform;
        _characterController = GetComponent<CharacterController>();
        _farts = GetComponent<Farts>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OpenInventory(InputAction.CallbackContext ctx)
    {
        _inventory.SetActive(!_inventory.activeInHierarchy);
    }
    public void SetIsInBaseInventory(bool _isIn)
    {
        _isInBaseInventory = _isIn;
    }

    public void StartFart(InputAction.CallbackContext ctx)
    {
        if (_fartCooldown > 0)
            return;
        _fartCooldown = 1f;
        _farts.PlayRandomFartSound();
    }

    public InventoryManager GetInventory()
    {
        return _inventory.GetComponent<InventoryManager>();
    }

    public void OpenBook(InputAction.CallbackContext ctx)
    {
        _book.SetActive(!_book.activeInHierarchy);
        if (_book.activeInHierarchy)
        {
            Cursor.lockState = CursorLockMode.Confined;
            _isInBook = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            _isInBook = false;
        }
    }

    public void SetInventoryActive(bool _active)
    {
        _inventory.SetActive(_active);
    }
    public void OpenMenuPause(InputAction.CallbackContext ctx)
    {
        Debug.Log("OpenMenuPause");
    }
    public void Interaction(InputAction.CallbackContext ctx)
    {
        Debug.Log("Interaction");
        if (ctx.performed)
        {
            StartUi();
            DoorExit.Instance.OpenDoor(_camera);
        }
        //OfficeManager.Instance.MouvToChair();
        if(_timer <= 0)
        {
            PickUpObject();
            _timer = 0.05f;
        }
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (_characterController.isGrounded && _playerManager.Stamina >= 10 && context.performed && !_isOpen)
        {
            ChangeStamina(-10);
            _currentStaminaTime = _staminaTimer;
            _isJump = true;
        } 
    }
    public void AlightTorch(InputAction.CallbackContext ctx)
    {
        if (_canUseTorch && ctx.performed)
        {
            _torch.SetActive(!_torch.activeSelf);
        }
    }
    void Update()
    {
        if (!_isDead && !_isInBook)
        {
            RotateCamera();
            MovePlayer();
        }
        Timer();

        if (!_staminaRegenStarted && CanRegenStamina())
        {
            StartCoroutine(RegenStamina());
        }

        if (Physics.Raycast(_camera.position, _camera.forward, out RaycastHit hit, _distRayCast) && hit.collider.gameObject.layer == 8)
        {
            _textPress.text = "Press E for interact";
            _canOpen = true;
        }
        else
        {
            _canOpen = false;
            _textPress.text = "";
        }

    }
    public void SprintPlayer(InputAction.CallbackContext context)
    {
        Debug.Log("Sprint");
        _isRunning = true;
        if (context.canceled || _playerManager.Stamina <= 0)
            _isRunning = false;
    }
    
    private void Timer()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
        if(_currentStaminaTime > 0)
        {
            _currentStaminaTime -= Time.deltaTime;
        }
        if(_fartCooldown > 0)
        {
            _fartCooldown -= Time.deltaTime;
        }
    }

    public void GetMouseDelta(InputAction.CallbackContext ctx)
    {
        if (ctx.control.name == "rightStick")
            _rotation = ctx.ReadValue<Vector2>() * _sensitivityController;
        else
            _rotation = ctx.ReadValue<Vector2>() * _sensitivity;
    }
    private void RotateCamera()
    {
        _rotation2.x += _rotation.x * Time.deltaTime;
        _rotation2.y -= _rotation.y * Time.deltaTime;
        _rotation2.y = Mathf.Clamp(_rotation2.y, -yRotationLimit, yRotationLimit);
        _transform.localEulerAngles = new Vector3(0, _rotation2.x, 0);
        _camera.localEulerAngles = new Vector3(_rotation2.y, 0, 0);
    }
    public void GetInputPlayer(InputAction.CallbackContext ctx)
    {
        _moveDir = ctx.ReadValue<Vector2>();
    }
    private void MovePlayer()
    {
        Vector3 forward = _transform.TransformDirection(Vector3.forward);
        Vector3 right = _transform.TransformDirection(Vector3.right);

        float curSpeedX = _canMove ? (_isRunning ? _runSpeed : _walkSpeed) * _moveDir.y : 0;
        float curSpeedY = _canMove ? (_isRunning ? _runSpeed : _walkSpeed) * _moveDir.x : 0;
        float movementDirectionY = _moveDirection.y;
        _moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (_isJump && _canMove && _characterController.isGrounded)
        {
            _moveDirection.y = _jumpPower;
            _isJump = false;
        }
        else
        {
            if (_isRunning)
            {
                if(!_runningStaminaLose && _moveDir != Vector2.zero)
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
            _moveDirection.y -= _gravity * Time.deltaTime;
        }

        _characterController.Move(_moveDirection * Time.deltaTime);
    }

    public void MouseScrollY(InputAction.CallbackContext ctx)
    {
        if(_timer <= 0)
        {
            CheckIfHotBarIsShowed();
            _scrollDir = ctx.ReadValue<Vector2>();
            int _indexAddition = 0;
            if (_scrollDir.y > 0) _indexAddition = 1;
            else if (_scrollDir.y < 0) _indexAddition = -1;
            ChangeToHotBarSlot(UpdateHotBarIndex(_hotBar.GetComponent<HotBarManager>()._hotBarSlotIndex, _indexAddition));
            _timer = 0.01f;
        }
    }

    public void HotBarSelection1(InputAction.CallbackContext ctx)
    {
        CheckIfHotBarIsShowed();
        ChangeToHotBarSlot(0);
    }

    public void HotBarSelection2(InputAction.CallbackContext ctx)
    {
        CheckIfHotBarIsShowed();
        ChangeToHotBarSlot(1);
    }

    public void HotBarSelection3(InputAction.CallbackContext ctx)
    {
        CheckIfHotBarIsShowed();
        ChangeToHotBarSlot(2);
    }

    public void HotBarSelection4(InputAction.CallbackContext ctx)
    {
        CheckIfHotBarIsShowed();
        ChangeToHotBarSlot(3);
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
        _hotBar.GetComponent<HotBarManager>()._hotBarSlotIndex = _newIndex;
        _hotBar.GetComponent<HotBarManager>().UpdateSelectedHotBarSlot();
    }

    private int UpdateHotBarIndex(int _index, int _indexAddition)
    {
        _index += _indexAddition;

        if(_index < 0)
        {
            _index = 3;
        }
        else if (_index > 3)
        {
            _index = 0;
        }
        return _index;
    }

    // Methode to add an object to the inventory
    private void PickUpObject()
    {
        RaycastHit[] _hits = Physics.SphereCastAll(_transform.position, _itemPickRange, _transform.up);

        if (_hits.Length > 0)
        {
            for (int i = 0; i < _hits.Length; i++)
            {
                if (_hits[i].collider.CompareTag(_itemTag))
                {
                    _inventory.GetComponent<InventoryManager>().AddItem(_hits[i].collider.GetComponent<Item>().ItemName(), _hits[i].collider.GetComponent<Item>().ItemSprite(), false);
                    Destroy(_hits[i].collider.gameObject);
                }
            }
        }
    }

    private void ChangeStamina(float _value)
    {

        _playerManager.Stamina += _value;
        _playerManager.SetStaminaBar();
    }

    private IEnumerator RegenStamina()
    {
        _staminaRegenStarted = true;
        while(CanRegenStamina())
        {
            ChangeStamina(_playerManager.MaxStamina / 10);
            if(_playerManager.Stamina > _playerManager.MaxStamina)
            {
                _playerManager.SetMaxStamina(_playerManager.MaxStamina);
            }
            yield return new WaitForSeconds(1f);
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
    public void SetUseTorch(bool useTorch)
    {
        _canUseTorch = useTorch;
    }
    public float GetDistRayCast()
    {
        return _distRayCast;
    }

    // Ui Player
    public void StartUi()
    {
        if (_canOpen && Physics.Raycast(_camera.position, _camera.forward, out RaycastHit hit, _distRayCast))
        {
            OpenUi(hit.collider.GetComponent<BuildInterractable>()._index);
        } 
    }
    
    public void OpenUi(int index)
    {
        print(_uiPlayer[index].activeSelf);
        _uiPlayer[index].SetActive(!_uiPlayer[index].activeSelf);
        print(_uiPlayer[index].activeSelf);
        DisablePlayer(_uiPlayer[index].activeSelf);
    }
    private void DisablePlayer(bool active)
    {
        if (active)
        {
            Cursor.lockState = CursorLockMode.None;
            GetComponentInChildren<PlayerInput>().actions.actionMaps[0].Disable();
            _isOpen = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            GetComponentInChildren<PlayerInput>().actions.actionMaps[0].Enable();
            _isOpen = false;
        }
    }
}
