using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;


public class DroneManager : NetworkBehaviour
{
    [SerializeField] private Transform _camera;
    public static bool _canUseDrone;

    private static Vector2 _moveDir;
    private static Vector2 _rotation = Vector2.zero;
    private static Vector3 _moveDirection = Vector3.zero;
    private static Vector2 _rotation2 = Vector2.zero;
    private static Vector3 _initPos = Vector3.zero;

    [SerializeField] private float _sensitivity;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _upPower;

    public static bool _canMove = false;
    private static bool _isUp;
    private static bool _isDown;

    private static CharacterController _characterController;
    public static PlayerInput _playerInput;
    public static Camera _cameraPlayer;
    [SerializeField] private GameObject _uiDrone;

    [Range(0f, 90f)][SerializeField] float _yRotationLimit = 88f;
    private static Transform _transform;
    public static DroneManager Instance;

    [SyncVar(hook = nameof(OnPositionChanged))]
    private Vector3 _syncedPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Start()
    {
        _canUseDrone = true;
        _transform = transform;
        _initPos = _transform.position;
        _syncedPosition = _initPos;
        _characterController = GetComponent<CharacterController>();
        //_playerInput.actions.actionMaps[1].Disable();
    }
    private void Update()
    {
        if (_canMove)
        {
            MoveDrone();
            RotateCameraDrone();
            if (isOwned)
            {
                CmdUpdatePosition(transform.position);
            }
        }
        if (!isOwned)
        {
            transform.position = _syncedPosition;
        }
    }

    [Command]
    void CmdUpdatePosition(Vector3 newPosition)
    {
        _syncedPosition = newPosition;
        RpcUpdatePosition(newPosition);
    }

    [ClientRpc]
    void RpcUpdatePosition(Vector3 newPosition)
    {
        if (!isOwned)
        {
            _syncedPosition = newPosition;
            transform.position = newPosition;
        }
    }
    void OnPositionChanged(Vector3 oldPos, Vector3 newPos)
    {
        if (!isOwned)
        {
            transform.position = newPos;
        }
    }

    public void GetInputDrone(InputAction.CallbackContext ctx)
    {
        _moveDir = ctx.ReadValue<Vector2>();
    }
    public void GetMouseDeltaDrone(InputAction.CallbackContext ctx)
    {
        _rotation = ctx.ReadValue<Vector2>() * _sensitivity;
    }
    public void GetInputSpace(InputAction.CallbackContext ctx)
    {
        _isUp = true;
        if (ctx.canceled)
        {
            _isUp = false;
        }
    }
    public void GetInputShift(InputAction.CallbackContext ctx)
    {
        _isDown = true;
        if (ctx.canceled)
            _isDown= false;
    }
    private void MoveDrone()
    {
        Vector3 forward = _transform.TransformDirection(Vector3.forward);
        Vector3 right = _transform.TransformDirection(Vector3.right);

        float curSpeedX = _canMove ? _walkSpeed * _moveDir.y : 0;
        float curSpeedY = _canMove ? _walkSpeed * _moveDir.x : 0;
        float movementDirectionY = _moveDirection.y;

        _moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        _moveDirection.y = 0;
        if (_isUp && _canMove)
        {
            _moveDirection.y = _upPower;
        }
        if (_isDown && _canMove)
        {
            _moveDirection.y = -_upPower;
        }
        if (_isDown && _canMove && _isUp)
        {
            _moveDirection.y = 0;
        }
        _characterController.Move(_moveDirection * Time.deltaTime);
    }
    private void RotateCameraDrone()
    {
        _rotation2.x += _rotation.x * Time.deltaTime;
        _rotation2.y -= _rotation.y * Time.deltaTime;
        _rotation2.y = Mathf.Clamp(_rotation2.y, -_yRotationLimit, _yRotationLimit);
        _transform.localEulerAngles = new Vector3(0, _rotation2.x, 0);
        _camera.localEulerAngles = new Vector3(_rotation2.y, 0, 0);
    }
    public void StartDrone(Camera playerCam, PlayerInput playerInput)
    {
        if (_canUseDrone)
        {
            _cameraPlayer = playerCam;
            _playerInput = playerInput;
            _canUseDrone = false;
            QuestManager.Instance.QuestComplete(10);
            _canMove = true;
            _characterController.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            //_uiDrone.SetActive(false);
            playerCam.enabled = false;
            playerInput.actions.actionMaps[0].Disable();
            playerInput.actions.actionMaps[1].Enable();
        }
    }
    public void ExitDrone(InputAction.CallbackContext ctx)
    {
        //CmdUpdatePosition(_initPos);

        _canUseDrone = true;
        _canMove = false;

        _characterController.enabled = false;
        _playerInput.actions.actionMaps[1].Disable();
        _playerInput.actions.actionMaps[0].Enable();
        Cursor.lockState = CursorLockMode.Locked;
        _cameraPlayer.enabled = true;
        _characterController.enabled = false;
        _transform.eulerAngles = Vector3.zero;
        _camera.eulerAngles = Vector3.zero;
    }
}
