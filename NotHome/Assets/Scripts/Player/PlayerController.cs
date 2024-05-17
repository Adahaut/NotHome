using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Transform" + "\n")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Transform _camera;

    [Header("Value" + "\n")]
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _sprintValue;
    [SerializeField] private float _sensitivity;
    [SerializeField] private float _sensitivityController;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _inertia = 0.97f;

    [Header("Inventory")]
    [SerializeField] private GameObject _inventory;
    [SerializeField] private string _itemTag;
    [SerializeField] private int _itemPickRange;

    [Header("HotBar")]
    [SerializeField] private GameObject _hotBar;

    private Rigidbody _rigidbodyPlayer;
    private bool _isGrounded;
    private float _initSpeed;
    private float _timer;

    private Vector2 _rotation = Vector2.zero;
    private Vector2 _rotation2 = Vector2.zero;
    private Vector2 _moveDir;
    private Vector2 _scrollDir;

    [Range(0f, 90f)][SerializeField] float yRotationLimit = 88f;
    
    private void Awake()
    {
        _rigidbodyPlayer = GetComponent<Rigidbody>();
        _initSpeed = _speed;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void OpenInventory(InputAction.CallbackContext ctx)
    {
        _inventory.SetActive(!_inventory.activeInHierarchy);
        Cursor.lockState = _inventory.activeInHierarchy? CursorLockMode.None : CursorLockMode.Locked;
    }
    public void OpenMenuPause(InputAction.CallbackContext ctx)
    {
        Debug.Log("OpenMenuPause");
    }
    public void Interaction(InputAction.CallbackContext ctx)
    {
        Debug.Log("Interaction");
        QG_Manager.Instance.OpenUi();
        PickUpObject();
        if (AnimationManager.Instance._doorIsOpen)
            AnimationManager.Instance.CloseDoor();
        else
            AnimationManager.Instance.OpenDoor();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump");
        if (_isGrounded && context.performed && !QG_Manager.Instance._isOpen)
            _rigidbodyPlayer.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }
    public void SprintPlayer(InputAction.CallbackContext context)
    {
        Debug.Log("Sprint");
        _speed = _initSpeed * _sprintValue;
        if (context.canceled)
        {
            _speed = _initSpeed;
        }
    }
    void Update()
    {
        _isGrounded = Physics.Raycast(_groundCheck.position, Vector3.down, 0.05f);
        RotateCamera();
        MovePlayer();
        Timer();
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
        transform.localEulerAngles = new Vector3(0, _rotation2.x, 0);
        _camera.localEulerAngles = new Vector3(_rotation2.y, 0, 0);
    }
    public void GetInputPlayer(InputAction.CallbackContext ctx)
    {
        _moveDir = ctx.ReadValue<Vector2>();
    }
    private void MovePlayer()
    {
        if (_moveDir == Vector2.zero)
        {
            _rigidbodyPlayer.velocity = new Vector3(_rigidbodyPlayer.velocity.x * _inertia, _rigidbodyPlayer.velocity.y, _rigidbodyPlayer.velocity.z * _inertia);
        }
        else
        {
            _rigidbodyPlayer.AddForce(_moveDir.y * _speed * Time.deltaTime * transform.forward);
            _rigidbodyPlayer.AddForce(_moveDir.x * _speed * Time.deltaTime * transform.right);
            if (_rigidbodyPlayer.velocity.magnitude > _maxSpeed)
            {
                float velocityY = _rigidbodyPlayer.velocity.y;
                _rigidbodyPlayer.velocity = Vector3.ClampMagnitude(_rigidbodyPlayer.velocity, _maxSpeed);
                _rigidbodyPlayer.velocity = new Vector3(_rigidbodyPlayer.velocity.x, velocityY, _rigidbodyPlayer.velocity.z);
            }
        }
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
        RaycastHit[] _hits = Physics.SphereCastAll(transform.position, _itemPickRange, transform.up);

        if (_hits.Length > 0)
        {
            print("items");
            for (int i = 0; i < _hits.Length; i++)
            {
                if (_hits[i].collider.name == "TestItem")
                {
                    print(_hits[i].collider.name);
                    _inventory.GetComponent<InventoryManager>().AddItem(_hits[i].collider.GetComponent<Item>().ItemName(), _hits[i].collider.GetComponent<Item>().ItemSprite());
                    Destroy(_hits[i].collider);
                }
            }
        }
    }
}
