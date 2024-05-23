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
    [SerializeField] private int _aimingZoomSteps;
    [SerializeField] private Transform _weaponHolder;
                     private Vector3 _startWeaponHolder;
    [SerializeField] private Transform _endWeaponHolder;
    public bool _isAiming;

    private float _timeSinceLastShot;

    private void Start()
    {
        PlayerAttack._shootAction += Shoot;
        PlayerAttack._reloading += StartReload;
        PlayerAttack._aimAction += StartAiming;
        PlayerAttack._stopAimAction += StopAiming;
        _startWeaponHolder = _weaponHolder.localPosition;
    }

    private bool CanShoot()
    {
        return !_weaponData._isReloading && _timeSinceLastShot > 1f / (_weaponData._fireRate / 60f);
    }

    private void StarRecoil()
    {
        _savedCameraTransform = _playerCamera.transform;
        StartCoroutine(Recoil());
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
        print(_weaponDestination);
        float _deltaT = 1f / _aimingZoomSteps;
        for (int i = 0; i < _aimingZoomSteps; i++)
        {
            _playerCamera.fieldOfView = _playerCamera.fieldOfView - (dir * _zoomForce);
            _weaponHolder.localPosition = Vector3.Lerp(_weaponHolder.localPosition, _weaponDestination, _deltaT * i);
            yield return new WaitForSeconds(0.02f);
        }
        _weaponHolder.localPosition = Vector3.Lerp(_weaponHolder.localPosition, _weaponDestination, 1f);
    }



    private IEnumerator Recoil()
    {
        for(int i = 0; i < _recoilStepsNumber; i++)
        {
            _playerCamera.transform.rotation = Quaternion.Euler(_playerCamera.transform.rotation.x - _recoilForce, _playerCamera.transform.rotation.y, _playerCamera.transform.rotation.z);
            yield return new WaitForSeconds(1 / (_recoilStepsNumber / 2));
        }
        for (int i = 0; i < _recoilStepsNumber; i++)
        {
            _playerCamera.transform.rotation = Quaternion.Euler(_playerCamera.transform.rotation.x + _recoilForce, _playerCamera.transform.rotation.y, _playerCamera.transform.rotation.z);
            yield return new WaitForSeconds(1 / (_recoilStepsNumber / 2));
        }
        _playerCamera.transform.rotation = Quaternion.Euler(_savedCameraTransform.rotation.x, _playerCamera.transform.rotation.y, _playerCamera.transform.rotation.z);
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
