using System;
using UnityEngine;

public class RangeWeapon : MonoBehaviour
{
    [SerializeField] private WeaponData _weaponData;
    [SerializeField] private Transform _muzzle;

    private float _timeSinceLastShot;

    private void Start()
    {
        PlayerAttack._shootAction += Shoot;
    }

    private bool CanShoot()
    {
        return !_weaponData._isReloading && _timeSinceLastShot > 1f / (_weaponData._fireRate / 60f);
    }

    public void Shoot()
    {
        if(_weaponData._currentAmmo > 0 && CanShoot())
        {
            print("tire");
            if (Physics.Raycast(_muzzle.position, transform.forward, out RaycastHit _hitInfo, _weaponData._maxDistance))
            {
                print("touche " + _hitInfo.collider.name);
            }

            _weaponData._currentAmmo--;
            _timeSinceLastShot = 0;
            OnGunShoot();
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
