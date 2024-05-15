
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
    [SerializeField] private float _sensitivity = 1f;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _inertia = 0.97f;

    private Rigidbody _rigidbodyPlayer;
    private bool _isGrounded;
    private float _initSpeed;

    private Vector2 _rotation = Vector2.zero;
    private Vector2 _moveDir;

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
        Debug.Log("OpenInventory");
    }
    public void OpenMenuPause(InputAction.CallbackContext ctx)
    {
        Debug.Log("OpenMenuPause");
    }
    public void Interaction(InputAction.CallbackContext ctx)
    {
        Debug.Log("Interaction");
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump");
        if (_isGrounded && context.performed)
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
    }

    public void GetMouseDelta(InputAction.CallbackContext ctx)
    {
        _rotation.x += ctx.ReadValue<Vector2>().x * _sensitivity * Time.deltaTime;
        _rotation.y -= ctx.ReadValue<Vector2>().y * _sensitivity * Time.deltaTime;
    }
    private void RotateCamera()
    {
        _rotation.y = Mathf.Clamp(_rotation.y, -yRotationLimit, yRotationLimit);
        transform.localEulerAngles = new Vector3(0, _rotation.x, 0);
        _camera.localEulerAngles = new Vector3(_rotation.y, 0, 0);
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
            _rigidbodyPlayer.velocity = Vector3.ClampMagnitude(_rigidbodyPlayer.velocity, _maxSpeed);
        }
    }
}
