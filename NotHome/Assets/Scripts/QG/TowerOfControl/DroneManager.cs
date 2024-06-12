using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


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
    [SerializeField] private float _maxUsingTime;

    private float currentTimer;

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

    [SyncVar(hook = nameof(OnRotationChanged))]
    private Quaternion _syncedRotation;

    [SyncVar(hook = nameof(OnRotationCameraChanged))]
    private Quaternion _syncedCameraRotation;

    private static GameObject controledByPlayer;
    private static Image timeBar;

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
        _syncedRotation = _transform.rotation;
        _characterController = GetComponent<CharacterController>();
    }
    private void Update()
    {
        if (_canMove)
        {
            if (currentTimer > 0)
            {
                currentTimer -= Time.deltaTime;
                timeBar.fillAmount = currentTimer / _maxUsingTime;
                MoveDrone();
                RotateCameraDrone();
                if (isOwned)
                {
                    CmdUpdatePositionAndRotation(transform.position, transform.rotation, _camera.rotation);
                }
            }
            else
            {
                QuitDrone();
            }
        }
        else
        {
            if (isOwned)
                CmdUpdatePositionAndRotation(_initPos, Quaternion.identity, Quaternion.identity);
        }


        if (!isOwned)
        {
            transform.position = _syncedPosition;
            transform.rotation = _syncedRotation;
            _camera.rotation = _syncedCameraRotation;
        }
    }

    [Command]
    void CmdUpdatePositionAndRotation(Vector3 newPosition, Quaternion newRotation, Quaternion cameraRotation)
    {
        _syncedPosition = newPosition;
        _syncedRotation = newRotation;
        _syncedCameraRotation = cameraRotation;
        transform.position = newPosition;
        transform.rotation = newRotation;
        _camera.rotation = cameraRotation;
        RpcUpdatePositionAndRotation(newPosition, newRotation, cameraRotation);
    }

    [ClientRpc]
    void RpcUpdatePositionAndRotation(Vector3 newPosition, Quaternion newRotation, Quaternion cameraRotation)
    {
        if (!isOwned)
        {
            _syncedPosition = newPosition;
            _syncedRotation = newRotation;
            _syncedCameraRotation= cameraRotation;
            transform.position = newPosition;
            transform.rotation = newRotation;
            _camera.rotation = cameraRotation;
        }
    }

    void OnPositionChanged(Vector3 oldPos, Vector3 newPos)
    {
        if (!isOwned)
        {
            transform.position = newPos;
        }
    }

    void OnRotationChanged(Quaternion oldRot, Quaternion newRot)
    {
        if (!isOwned)
        {
            transform.rotation = newRot;
        }
    }

    void OnRotationCameraChanged(Quaternion oldRot, Quaternion newRot)
    {
        if (!isOwned)
        {
            _camera.rotation = newRot;
        }
    }


    #region Inputs

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

    #endregion

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


    public void StartDrone(Camera playerCam, PlayerInput playerInput, GameObject player, Image fillBar)
    {
        if (_canUseDrone)
        {
            timeBar = fillBar;
            currentTimer = _maxUsingTime;
            controledByPlayer = player;
            _initPos = _transform.position;
            _cameraPlayer = playerCam;
            _playerInput = playerInput;
            _canUseDrone = false;
            QuestManager.Instance.QuestComplete(10);
            _canMove = true;
            _characterController.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            playerCam.enabled = false;
            playerInput.actions.actionMaps[0].Disable();
            playerInput.actions.actionMaps[1].Enable();
        }
    }
    public void ExitDrone(InputAction.CallbackContext ctx)
    {
        QuitDrone();
    }

    private void QuitDrone()
    {
        _canUseDrone = true;
        _canMove = false;

        transform.position = _initPos;

        _characterController.enabled = false;
        _playerInput.actions.actionMaps[1].Disable();
        _playerInput.actions.actionMaps[0].Enable();
        Cursor.lockState = CursorLockMode.Locked;
        _cameraPlayer.enabled = true;
        _characterController.enabled = false;

        _transform.position = _initPos;
        _transform.rotation = Quaternion.identity;

        _transform.eulerAngles = Vector3.zero;
        _camera.eulerAngles = Vector3.zero;

        controledByPlayer.GetComponent<PlayerController>().EnableCanvasAfterUsingDrone();
    }
}
