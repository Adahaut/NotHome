using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : NetworkBehaviour
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
    private PlayerController _playerController;
    public List<GameObject> _machetteUpgrades = new List<GameObject>();
    private RangeWeapon _rangeWeapon;
    private Animator _animator;


    [SerializeField] private float _cadence;
    private float _cadenceTimer;

    public static PlayerAttack Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        _playerController = GetComponent<PlayerController>();
        _isAimingFinished = true;
        _isRecoilFinished = true;
    }

    private void Start()
    {
        _rangeWeapon = GetComponentInChildren<RangeWeapon>();
    }

    private void Update()
    {
        _cadenceTimer += Time.deltaTime;
    }

    public void UpgradeMachetteVisual(int _index)
    {
        for(int i = 0; i  < _machetteUpgrades.Count; i++)
        {
            _machetteUpgrades[i].SetActive(false);
        }
        _machetteUpgrades[_index].SetActive(true);
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (isOwned && _isMeleeWeaponEqupiped && _playerController != null && context.performed)
        {
            StartCoroutine(GetComponentInChildren<SetAnimationMachet>().StartAnimMachet());
            StartCoroutine(_playerController.AnimOneTime("HitMachet"));
        }
        if (isOwned && _playerController != null && _playerController._canAttack)
        {
            if (_playerController._isInBook)
                return;
            if (_isRangeWeaponEqupiped)
            {
                _shootAction?.Invoke();
            }
            else if (_cadenceTimer >= _cadence && _isMeleeWeaponEqupiped)
            {
                GetComponentInChildren<SetAnimationMachet>().StartAnimMachet();
                _cadenceTimer = 0;
                StartCoroutine(ActiveDesactiveCollider());
            }
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
        if(isOwned)
        {
            if (_playerController._isInBook || !_isRangeWeaponEqupiped || _rangeWeapon._weaponLevel < 3 || !_playerController._canAttack)
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
        if(isOwned)
        {
            if (_playerController._isInBook)
                return;
            _reloading?.Invoke();
        }
        
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
