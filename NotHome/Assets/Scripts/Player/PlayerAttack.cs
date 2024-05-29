using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Collider _enemyDetectionCollider;
    [SerializeField] private int _damages;
    public static Action _shootAction;
    public static Action _reloading;
    public static Action _aimAction;
    public static Action _stopAimAction;
    public bool _isAimingFinished;
    public bool _isRecoilFinished;
    public bool _isAiming;
    public bool _isRangeWeaponEqupiped;
    public bool _isMeleeWeaponEqupiped;
    private PC _playerController;


    [SerializeField] private float _cadence;
    private float _cadenceTimer;

    public static PlayerAttack Instance;

    private PC _playerController;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        _playerController = GetComponent<PC>();
        _isAimingFinished = true;
        _isRecoilFinished = true;
}
    }
    private void Update()
    {
        _cadenceTimer += Time.deltaTime;
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (_playerController.IsInBook)
            return;
        if(_isRangeWeaponEqupiped)
        {
            _shootAction?.Invoke();
        }
        else if (_cadenceTimer >= _cadence && _isMeleeWeaponEqupiped)
        {
            _cadenceTimer = 0;
            StartCoroutine(ActiveDesactiveCollider());
        }
        
    }
    public void SetCadence(float number)
    {
        _cadence = number;
    }
    public void SetAttack(int number)
    {
        _damages = number;
    }
    public void Aim(InputAction.CallbackContext context)
    {
        if (_playerController.IsInBook || !_isRangeWeaponEqupiped)
            return;
        if (context.started)
        {
            StartAiming();
        }
        else if (context.canceled && _isAiming)
        {
            StopAiming();
        }
    }

    public void StartAiming()
    {
        _isAiming = true;
        _aimAction?.Invoke();
    }

    public void StopAiming()
    {
        _isAiming = false;
        _stopAimAction?.Invoke();
    }

    public void Reload(InputAction.CallbackContext context)
    {
        if (_playerController.IsInBook)
            return;
        _reloading?.Invoke();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.GetComponent<LifeManager>().TakeDamage(_damages);
        }
    }

    public IEnumerator ActiveDesactiveCollider()
    {
        _enemyDetectionCollider.enabled = true;
        yield return new WaitForSeconds(1f);
        _enemyDetectionCollider.enabled = false;
    }
}
