//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.InputSystem;

//[RequireComponent(typeof(CharacterController))]
//public class NewPlayerController : MonoBehaviour
//{
//    [SerializeField] private float _walkSpeed;
//    [SerializeField] private float _runSpeed;
//    [SerializeField] private float _jumpPower;
//    [SerializeField] private float _gravity;

//    private Vector3 _moveDirection = Vector3.zero;
//    private Vector2 _moveDir = Vector2.zero;
//    public bool _canMove = true;
//    private bool _isRunning;
//    private bool _isJump;


//    CharacterController characterController;
//    void Start()
//    {
//        characterController = GetComponent<CharacterController>();
//        Cursor.lockState = CursorLockMode.Locked;
//        Cursor.visible = false;
//    }
//    public void SprintPlayer(InputAction.CallbackContext context)
//    {
//        Debug.Log("Sprint");
//        _isRunning = true;
//        if (context.canceled)
//            _isRunning = false;
//    }
//    public void GetInputPlayer(InputAction.CallbackContext ctx)
//    {
//        _moveDir = ctx.ReadValue<Vector2>();
//    }
//    public void OnJump(InputAction.CallbackContext context)
//    {
//        if (context.performed && characterController.isGrounded)
//            _isJump = true;
//    }

//    void Update()
//    {
//        Vector3 forward = transform.TransformDirection(Vector3.forward);
//        Vector3 right = transform.TransformDirection(Vector3.right);

//        print(_moveDir);
//        float curSpeedX = _canMove ? (_isRunning ? _runSpeed : _walkSpeed) * _moveDir.y : 0;
//        float curSpeedY = _canMove ? (_isRunning ? _runSpeed : _walkSpeed) * _moveDir.x : 0;
//        float movementDirectionY = _moveDirection.y;
//        _moveDirection = (forward * curSpeedX) + (right * curSpeedY);

//        if (_isJump && _canMove && characterController.isGrounded)
//        {
//            _moveDirection.y = _jumpPower;
//            _isJump = false;
//        }
//        else
//        {
//            _moveDirection.y = movementDirectionY;
//        }

//        if (!characterController.isGrounded)
//        {
//            _moveDirection.y -= _gravity * Time.deltaTime;
//        }

//        characterController.Move(_moveDirection * Time.deltaTime);

//    }
//}
