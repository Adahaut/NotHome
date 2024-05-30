using System.Collections;
using UnityEngine;

public class RangeWeapon : MonoBehaviour
{
    [SerializeField] private WeaponData _weaponData;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private Transform _savedCameraTransform;
    [SerializeField] private int _recoilStepsNumber;
    [SerializeField] private float _recoilForce;
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private int _aimingZoomSteps;
    [SerializeField] private Transform _weaponHolder;
                     private Vector3 _startWeaponHolder;
    [SerializeField] private Transform _endWeaponHolder;
    public bool _isAiming;
    [SerializeField] private PC _playerController;

    private Transform _transform;

    [SerializeField] private Vector3 _upRecoilValue;
    private Vector3 _originalRotation;

    private int _currentAmmo;

    private float _timeSinceLastShot;

    private bool _isReloading;

    [SerializeField] private GameObject _redDot;
    [SerializeField] private GameObject _laser;

    public static RangeWeapon Instance;

    private void Awake()
    {
        if (Instance == null) 
            Instance = this;
    }

    private void Start()
    {
        _transform = transform;
        _cameraTransform = _playerCamera.transform;
        PlayerAttack._shootAction += Shoot;
        PlayerAttack._reloading += StartReload;
        PlayerAttack._aimAction += StartAiming;
        PlayerAttack._stopAimAction += StopAiming;

        _currentAmmo = _weaponData._magSize;
        _startWeaponHolder = _weaponHolder.localPosition;

        _originalRotation = _transform.localEulerAngles;
    }
    public void NextWeapon()
    {
        if (_weaponData._nextWeapon != null)
            _weaponData = _weaponData._nextWeapon;
    }
    public void AciveRedDot()
    {
        _redDot.SetActive(true);
    }
    public void ActiveLaser()
    {
        _laser.SetActive(true);
    }
    private bool CanShoot()
    {
        return !_isReloading && _timeSinceLastShot > 1f / (_weaponData._fireRate / 60f);
    }

    public void StartAiming()
    {
        StartCoroutine(Zooming());
    }

    public void StopAiming()
    {
        StartCoroutine(Zooming(-1));
    }

    private IEnumerator Zooming(int dir = 1) // 1 = zoom, -1 = dezoom
    {
        _isAiming = !_isAiming;
        float _zoomForce = 30f / _aimingZoomSteps;
        Vector3 _weaponDestination = dir == 1 ? _endWeaponHolder.localPosition : _startWeaponHolder;
        float _deltaT = 1f / _aimingZoomSteps;
        for (int i = 0; i < _aimingZoomSteps; i++)
        {
            _playerCamera.fieldOfView = _playerCamera.fieldOfView - (dir * _zoomForce);
            _weaponHolder.localPosition = Vector3.Lerp(_weaponHolder.localPosition, _weaponDestination, _deltaT * i);
            yield return new WaitForSeconds(0.02f);
        }
        _weaponHolder.localPosition = Vector3.Lerp(_weaponHolder.localPosition, _weaponDestination, 1f);
    }


    public void StartReload()
    {
        if(!_isReloading ) 
        {
            StartCoroutine(Reloading());
        }
    }

    private IEnumerator Reloading()
    {
        print("reload");
        _isReloading = true;

        yield return new WaitForSeconds(_weaponData._reloadSpeed);

        _isReloading = false;
        _currentAmmo = _weaponData._magSize;
        print("finish reload");
    }

    public void Shoot()
    {
        if(_currentAmmo > 0)
        {
            //print(_currentAmmo.ToString());
            if (CanShoot())
            {
               // print("tire");
                StartRecoil();
                if (Physics.Raycast(_muzzle.position, _transform.forward, out RaycastHit _hitInfo, _weaponData._maxDistance))
                {
               //     print("touche " + _hitInfo.collider.name);
                    //damage enemies here
                }
                _currentAmmo--;
                _timeSinceLastShot = 0;
            }
        }
        else
        {
            StartReload();
        }
    }

    private void StartRecoil()
    {
        StartCoroutine(Recoil());
        StartCoroutine(CameraShake(0.1f, 1));
    }

    private IEnumerator Recoil()
    {
        for(int i = 0; i < 3; i++)
        {
            _transform.localEulerAngles += _upRecoilValue / 3f;
            yield return new WaitForSeconds((1f / (_weaponData._fireRate / 60f)) / 21f);
        }
        yield return new WaitForSeconds((1f / (_weaponData._fireRate / 60f)) / 6f);
        for (int i = 0; i < 3; i++)
        {
            _transform.localEulerAngles -= _upRecoilValue / 3f;
            yield return new WaitForSeconds((1f / (_weaponData._fireRate / 60f)) / 21f);
        }
        RecoverFromRecoil();
    }

    private IEnumerator CameraShake(float _duration, float _strengh)
    {
        float _elapsedTime = 0f;

        while (_elapsedTime < _duration)
        {
            _playerController.Rotation = new Vector2(_playerController.Rotation.x, _playerController.Rotation.y - _strengh);

            _elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private void RecoverFromRecoil()
    {
        _transform.localEulerAngles = _originalRotation;
    }

    private void Update()
    {
        _timeSinceLastShot += Time.deltaTime;
    }


}
