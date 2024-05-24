using System.Collections;
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

    [Header("Inventory")]
    [SerializeField] private GameObject _inventory;
    [SerializeField] private string _itemTag;
    [SerializeField] private int _itemPickRange;

    [Header("HotBar")]
    [SerializeField] private GameObject _hotBar;

    private bool _isRunning;
    private bool _isJump;
    private CharacterController _characterController;
    private float _timer;
    private bool _isInBaseInventory;

    private Vector3 _moveDirection = Vector3.zero;
    private Vector2 _rotation = Vector2.zero;
    private Vector2 _rotation2 = Vector2.zero;
    private Vector2 _moveDir;
    private Vector2 _scrollDir;
    
    [Range(0f, 90f)][SerializeField] float yRotationLimit = 88f;
    private Transform _transform;

    public Vector2 Rotation { get { return _rotation2; } set {  _rotation2 = value; } }

    public void Start()
    {
        _transform = transform;
        _characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        RotateCamera();
        MovePlayer();
        Timer();
    }

    public void OpenInventory(InputAction.CallbackContext ctx)
    {
        _inventory.SetActive(!_inventory.activeInHierarchy);
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
    public void OpenMenuPause(InputAction.CallbackContext ctx)
    {
        Debug.Log("OpenMenuPause");
    }
    public void Interaction(InputAction.CallbackContext ctx)
    {
        Debug.Log("Interaction");
        QG_Manager.Instance.OpenUi(this);
        OfficeManager.Instance.MouvToChair();
        if(_timer <= 0)
        {
            PickUpObject();
            _timer = 0.05f;
        }
        //if (AnimationManager.Instance._doorIsOpen)
        //    AnimationManager.Instance.CloseDoor();
        //else
        //    AnimationManager.Instance.OpenDoor();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump");
        print(!QG_Manager.Instance._isOpen);
        if (context.performed && _characterController.isGrounded && !QG_Manager.Instance._isOpen)
            _isJump = true;
    }
    public void SprintPlayer(InputAction.CallbackContext context)
    {
        Debug.Log("Sprint");
        _isRunning = true;
        if (context.canceled)
            _isRunning = false;
    }
    
    private void Timer()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
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
}
