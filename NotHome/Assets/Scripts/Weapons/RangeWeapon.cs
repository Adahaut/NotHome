using System;
using System.Collections;
using UnityEngine;

public class RangeWeapon : MonoBehaviour
{
    [SerializeField] private WeaponData _weaponData;
    [SerializeField] private Transform _muzzle;

    private float _timeSinceLastShot;

    private void Start()
    {
        PlayerAttack._shootAction += Shoot;
        PlayerAttack._reloading += StartReload;
    }

    private bool CanShoot()
    {
        return !_weaponData._isReloading && _timeSinceLastShot > 1f / (_weaponData._fireRate / 60f);
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
