using Mirror;
using System.Collections;
using UnityEngine;

public class RangeWeapon : NetworkBehaviour
{
    [SerializeField] private WeaponData _weaponData;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private Transform _savedCameraTransform;
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private int _aimingZoomSteps;
    [SerializeField] private Transform _weaponHolder;
                     private Vector3 _startWeaponHolder;
    [SerializeField] private Transform _endWeaponHolder;
    public bool _isAiming;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private float _speedFactor;
    private ProceduralRecoil _recoil;
    [SerializeField] private GameObject _muzzuleFlashEffect;

    private AudioSource _riffleAudioSource;
    [SerializeField] private AudioClip _riffleAudioClip;
    private Transform _transform;
    private PlayerAttack _playerAttack;

    [SerializeField] private Vector3 _upRecoilValue;
    private Vector3 _originalPosition;

    private int _currentAmmo;

    private float _timeSinceLastShot;

    private bool _isReloading;

    [SerializeField] private GameObject _redDot;
    [SerializeField] private GameObject _laser;

    [SerializeField] private NetworkIdentity identity;

    public static RangeWeapon Instance;

    private void Awake()
    {
        if (Instance == null) 
            Instance = this;
        _playerCamera = GetComponentInParent<Camera>();
    }

    private void Start()
    {
        _transform = transform;
        PlayerAttack._shootAction += Shoot;
        PlayerAttack._reloading += StartReload;
        PlayerAttack._aimAction += StartAiming;
        PlayerAttack._stopAimAction += StopAiming;
        _riffleAudioSource = transform.parent.GetChild(0).GetComponent<AudioSource>();
        _currentAmmo = _weaponData._magSize;
        _startWeaponHolder = _weaponHolder.localPosition;
        _playerAttack = GetComponentInParent<PlayerAttack>();
        _originalPosition = _transform.localPosition;
        _recoil = GetComponent<ProceduralRecoil>();
        _playerController = GetComponentInParent<PlayerController>();
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
        _playerController.SetAnimation("Aiming", true);
        StartCoroutine(Zooming());
    }

    public void StopAiming()
    {
        _playerController.SetAnimation("Aiming", false);
        StartCoroutine(Zooming(-1));
    }

    private IEnumerator Zooming(int dir = 1) // 1 = zoom, -1 = dezoom
    {
        _playerAttack._isAimingFinished = false;
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
        _playerAttack._isAimingFinished = true;
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
        _playerController.SetAnimation("Reload", true);
        _isReloading = true;

        yield return new WaitForSeconds(_weaponData._reloadSpeed);

        _isReloading = false;
        _currentAmmo = _weaponData._magSize;
        print("finish reload");
        _playerController.SetAnimation("Reload", false);
    }

    public void Shoot()
    {
        if(_currentAmmo > 0)
        {
            //print(_currentAmmo.ToString());
            if (CanShoot())
            {
                StartCoroutine(_playerController.AnimOneTime("Shoot"));
                StartRecoil();

                //if (identity.isOwned || identity.isServer)
                //    CmdPlayShootSound();
                if(isOwned)
                    CmdPlayShootSound();

                if(isOwned)
                    PlayMuzzuleFlash();
                
                if (Physics.Raycast(_muzzle.position, _transform.right * -1, out RaycastHit _hitInfo, _weaponData._maxDistance))
                {
                    if (_hitInfo.collider.GetComponent<LifeManager>() != null)
                    {
                        print(_hitInfo.collider.name);
                        _hitInfo.collider.GetComponent<LifeManager>()._currentLife -= _weaponData._damages;
                    }
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

    [Command]
    void CmdPlayShootSound()
    {
        RpcPlayShootSound();
    }

    [ClientRpc]
    void RpcPlayShootSound()
    {
        AudioSource.PlayClipAtPoint(_riffleAudioClip, this.transform.position);
    }

    [Command]
    private void PlayMuzzuleFlash()
    {
        GameObject _muzzuleFlash = Instantiate(_muzzuleFlashEffect, _muzzle);
        NetworkServer.Spawn(_muzzuleFlash);
        _muzzuleFlash.transform.position = _muzzle.position + (_muzzuleFlash.transform.right * 0.1f);
        _muzzuleFlash.GetComponent<ParticleSystem>().Play();
        Destroy(_muzzuleFlash, 0.2f);
    }

    private void StartRecoil()
    {
        //_recoil.Recoil();
        StartCoroutine(CameraShake(0.1f, 1));
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
        _transform.localPosition = _originalPosition;
    }

    private void Update()
    {
        _timeSinceLastShot += Time.deltaTime;
    }


}
