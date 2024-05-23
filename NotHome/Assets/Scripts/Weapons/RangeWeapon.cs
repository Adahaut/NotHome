using System;
using System.Collections;
using UnityEngine;

public class RangeWeapon : MonoBehaviour
{
    [SerializeField] private WeaponData _weaponData;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Transform _savedCameraTransform;
    [SerializeField] private int _recoilStepsNumber;
    [SerializeField] private float _recoilForce;
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private int _aimingZoomSteps;
    public bool _isAiming;

    private float _timeSinceLastShot;

    private void Start()
    {
        PlayerAttack._shootAction += Shoot;
        PlayerAttack._reloading += StartReload;
        PlayerAttack._aimAction += StartAiming;
    }

    private bool CanShoot()
    {
        return !_weaponData._isReloading && _timeSinceLastShot > 1f / (_weaponData._fireRate / 60f);
    }

    private void StarRecoil()
    {
        _savedCameraTransform = _cameraTransform;
        StartCoroutine(Recoil());
    }

    public void StartAiming()
    {
        if (!_isAiming)
        {
            StartCoroutine(Aiming());
        }
        else
        {
            StartCoroutine(Aiming());
        }
    }

    private IEnumerator Aiming()
    {
        _isAiming = true;
        float _zoomForce = 30f / _aimingZoomSteps;
        for (int i = 0; i < _aimingZoomSteps; i++)
        {
            _playerCamera.fieldOfView = _playerCamera.fieldOfView - _zoomForce;
            yield return new WaitForSeconds(0.02f);
        }
    }

    private IEnumerator Recoil()
    {
        for(int i = 0; i < _recoilStepsNumber; i++)
        {
            _cameraTransform.rotation = Quaternion.Euler(_cameraTransform.rotation.x - _recoilForce, _cameraTransform.rotation.y, _cameraTransform.rotation.z);
            yield return new WaitForSeconds(1 / (_recoilStepsNumber / 2));
        }
        for (int i = 0; i < _recoilStepsNumber; i++)
        {
            _cameraTransform.rotation = Quaternion.Euler(_cameraTransform.rotation.x + _recoilForce, _cameraTransform.rotation.y, _cameraTransform.rotation.z);
            yield return new WaitForSeconds(1 / (_recoilStepsNumber / 2));
        }
        _cameraTransform.rotation = Quaternion.Euler(_savedCameraTransform.rotation.x, _cameraTransform.rotation.y, _cameraTransform.rotation.z);
    }

    public void StartReload()
    {
        if(!_weaponData._isReloading ) 
        {
            StartCoroutine(Reloading());
        }
    }

    private IEnumerator Reloading()
    {
        print("reload");
        _weaponData._isReloading = true;

        yield return new WaitForSeconds(_weaponData._reloadSpeed);

        _weaponData._isReloading = false;
        _weaponData._currentAmmo = _weaponData._magSize;
        print("finish reload");
    }

    public void Shoot()
    {
        if(_weaponData._currentAmmo > 0)
        {
            if (CanShoot())
            {
                print("tire");
                StarRecoil();
                if (Physics.Raycast(_muzzle.position, transform.forward, out RaycastHit _hitInfo, _weaponData._maxDistance))
                {
                    print("touche " + _hitInfo.collider.name);
                    //damage enemies here
                }
            }
            _weaponData._currentAmmo--;
            _timeSinceLastShot = 0;
            OnGunShoot();
        }
        else
        {
            StartReload();
        }
    }

    private void Update()
    {
        _timeSinceLastShot += Time.deltaTime;
    }

    private void OnGunShoot()
    {

    }
}
