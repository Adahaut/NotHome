using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Transform _camera;

    private PlayerInputs _playerInputs;
    private InputAction _moveAction;

    private Rigidbody _rigidbodyPlayer;
    private bool _isGrounded;

    [SerializeField] private float _speed;
    private float _initSpeed;

    [SerializeField] private float _jumpForce;
    [SerializeField] private float _sprintValue;
    [SerializeField] private float _sensitivity = 1f;
    [SerializeField] private float _maxSpeed;

    private Vector2 _rotation = Vector2.zero;
    [Range(0f, 90f)][SerializeField] float yRotationLimit = 88f;
    private Vector2 _moveDir;

    [SerializeField] private float _inertia = 0.97f;
    private void Awake()
    {
        _playerInputs = new PlayerInputs();
        _rigidbodyPlayer = GetComponent<Rigidbody>();
        _initSpeed = _speed;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        _moveAction = _playerInputs.Player.Move;
        _moveAction.Enable();

        _playerInputs.Player.Sprint.performed += SprintPlayer;
        _playerInputs.Player.Sprint.Enable();

        _playerInputs.Player.Jump.performed += OnJump;
        _playerInputs.Player.Jump.Enable();
    }
    private void OnDisable()
    {
        _moveAction.Disable();

        _playerInputs.Player.Sprint.performed -= SprintPlayer;
        _playerInputs.Player.Sprint.Disable();

        _playerInputs.Player.Jump.performed -= OnJump;
        _playerInputs.Player.Jump.Disable();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (_isGrounded)
            _rigidbodyPlayer.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }
    private void SprintPlayer(InputAction.CallbackContext context)
    {
        _speed = _initSpeed * _sprintValue;
    }

    private void FixedUpdate()
    {
        _isGrounded = Physics.Raycast(_groundCheck.position, Vector3.down, 0.05f);
        if (!_playerInputs.Player.Sprint.inProgress)
            _speed = _initSpeed;

    }
    void Update()
    {
        RotateCamera();
        MovePlayer();
    }

    public void GetMouseDelta(InputAction.CallbackContext ctx)
    {
        _rotation = ctx.ReadValue<Vector2>();
        
    }
    private void RotateCamera()
    {
        transform.rotation *= Quaternion.AngleAxis(_rotation.x * Time.deltaTime * _sensitivity, Vector3.up);
        if (_camera.eulerAngles.x < 90 || _camera.eulerAngles.x > 270)
        {
            _camera.rotation *= Quaternion.AngleAxis(_rotation.y * Time.deltaTime * -_sensitivity, Vector3.right);
        }
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
            _rigidbodyPlayer.AddForce(_moveDir.y * transform.forward * _speed * Time.deltaTime);
            _rigidbodyPlayer.AddForce(_moveDir.x * transform.right * _speed * Time.deltaTime);
            _rigidbodyPlayer.velocity = Vector3.ClampMagnitude(_rigidbodyPlayer.velocity, _maxSpeed);
        }
    }
}
