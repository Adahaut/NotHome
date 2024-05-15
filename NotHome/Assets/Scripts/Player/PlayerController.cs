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

    private Vector2 rotation = Vector2.zero;
    const string xAxis = "Mouse X";
    const string yAxis = "Mouse Y";
    [Range(0f, 90f)][SerializeField] float yRotationLimit = 88f;
    private Vector2 _moveDir;
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
    private void RotateCamera()
    {
        rotation.x += Input.GetAxis(xAxis) * _sensitivity;
        rotation.y += Input.GetAxis(yAxis) * _sensitivity;
        rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);
        var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

        transform.localRotation = xQuat;
        _camera.localRotation = yQuat;
    }
    public void GetInputPlayer(InputAction.CallbackContext ctx)
    {
        _moveDir = ctx.ReadValue<Vector2>();
    }
    private void MovePlayer()
    {
        _rigidbodyPlayer.AddForce(_moveDir.y * transform.forward * Time.deltaTime * _speed);
        _rigidbodyPlayer.AddForce(_moveDir.x * transform.right * Time.deltaTime * _speed);
        
        _rigidbodyPlayer.velocity = new Vector3(Mathf.Clamp(_rigidbodyPlayer.velocity.x, -_maxSpeed, _maxSpeed), _rigidbodyPlayer.velocity.y,
            Mathf.Clamp(_rigidbodyPlayer.velocity.z, -_maxSpeed, _maxSpeed));
        if (_moveAction.ReadValue<Vector2>() == Vector2.zero)
        {
            _rigidbodyPlayer.velocity = new Vector3(0.25f, _rigidbodyPlayer.velocity.y, 0.25f);
        }
        print(_rigidbodyPlayer.velocity);
    }
}
