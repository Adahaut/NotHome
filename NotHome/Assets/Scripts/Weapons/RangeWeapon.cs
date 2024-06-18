using Mirror;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class RangeWeapon : NetworkBehaviour
{
    [SerializeField] public WeaponData _weaponData;
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
    [SerializeField] private GameObject _smokeBulletImpact;
    public int _weaponLevel;

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

    [SerializeField] public List<GameObject> _level2Weapon = new List<GameObject>();
    [SerializeField] public List<GameObject> _level3Weapon = new List<GameObject>();
    [SerializeField] public List<GameObject> _level4Weapon = new List<GameObject>();

    private List<List<GameObject>> _levelWeaponList = new();
    [SerializeField] private List<WeaponData> _weaponLvl = new();
    [SerializeField] private List<float> _muzzlePositionByLevel = new();

    public static RangeWeapon Instance;
    [SerializeField] private GameObject _hitMarker;
    [SerializeField] private GameObject _bloodEffect;
    [SerializeField] private TextMeshProUGUI _textAmmo;
    public int _nbAmmo;

    private void Awake()
    {
        if (Instance == null) 
            Instance = this;
        _playerCamera = GetComponentInParent<Camera>();
    }

    private void Start()
    {
        _transform = transform;
        _levelWeaponList.Add(_level2Weapon);
        _levelWeaponList.Add(_level3Weapon);
        _levelWeaponList.Add(_level4Weapon);
        if (_weaponLevel == 0)
            _weaponLevel = 1;
        _weaponData = _weaponLvl[_weaponLevel - 1];
        PlayerAttack._shootAction += Shoot;
        PlayerAttack._reloading += StartReload;
        PlayerAttack._aimAction += StartAiming;
        PlayerAttack._stopAimAction += StopAiming;
        _riffleAudioSource = transform.parent.GetChild(0).GetComponent<AudioSource>();
        _nbAmmo = _weaponData._magSize;
        _currentAmmo = _nbAmmo;
        _startWeaponHolder = _weaponHolder.localPosition;
        _playerAttack = GetComponentInParent<PlayerAttack>();
        _originalPosition = _transform.localPosition;
        _recoil = GetComponent<ProceduralRecoil>();
        _playerController = GetComponentInParent<PlayerController>();
        UpdateWeaponVisualAtLaunch();
        _textAmmo.text = _currentAmmo + "/" + _nbAmmo;
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
    public void AddAmmo(int number)
    {
        _nbAmmo += number;
        _textAmmo.text = _currentAmmo + "/" + _nbAmmo;
    }

    public void UpgradeWeaponVisual(List<GameObject> _meshList)
    {
        for (int i = 0;  i < _meshList.Count; i++)
        {
            _meshList[i].SetActive(true);
        }
        UpdateMuzzulePosition();
    }

    private void UpdateMuzzulePosition()
    {
        _muzzle.localPosition = new Vector3(_muzzlePositionByLevel[_weaponLevel], _muzzle.localPosition.y, _muzzle.localPosition.z);
    }

    private void UpdateWeaponVisualAtLaunch()
    {
        for (int i = 0; i < _weaponLevel; ++i)
        {
            UpgradeWeaponVisual(_levelWeaponList[i]);
        }
    }

    private bool CanShoot()
    {
        return !_isReloading && _timeSinceLastShot > 1f / (_weaponData._fireRate / 60f);
    }

    public void StartAiming()
    {
        _playerController.SetAnimation("Aiming", true);
        _playerController._weapon.GetComponent<Animator>().enabled = false;
        StartCoroutine(Zooming());
    }

    public void StopAiming()
    {
        if (_playerController.GetMoveDir() != Vector2.zero)
            _playerController._weapon.GetComponent<Animator>().enabled = true;
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
        if(!_isReloading && _nbAmmo > 0 && _currentAmmo < _weaponData._magSize) 
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
        if (_nbAmmo > _weaponData._magSize)
        {
            _nbAmmo -= _weaponData._magSize - _currentAmmo;
            _currentAmmo += _weaponData._magSize - _currentAmmo;
            
        }
        else
        {
            if (_weaponData._magSize - _currentAmmo > _nbAmmo)
            {
                _currentAmmo += _nbAmmo;
                _nbAmmo = 0;
            }
            else
            {
                _nbAmmo -= _weaponData._magSize - _currentAmmo;
                _currentAmmo += _weaponData._magSize - _currentAmmo;
            }
        }
        _textAmmo.text = _currentAmmo.ToString() + "/" + _nbAmmo;
        print("finish reload");
        _playerController.SetAnimation("Reload", false);
    }

    public void Shoot()
    {
        if(_currentAmmo > 0)
        {
            if (CanShoot())
            {
                StartCoroutine(_playerController.AnimOneTime("Shoot"));
                StartRecoil();

                if(isOwned)
                {
                    CmdPlaySound(transform.position);
                    CmdPlayMuzzleFlash(_muzzle.position);
                }
                if (Physics.Raycast(_playerCamera.transform.position, _transform.right * -1, out RaycastHit _hitInfo, _weaponData._maxDistance))
                {
                    if (_hitInfo.collider.GetComponent<LifeManager>() != null && _hitInfo.collider.gameObject != transform.root.gameObject)
                    {
                        CreateBlood(_hitInfo.point);
                        StartCoroutine(HitMarker());
                        CmdAttackPlayer(_hitInfo.transform.gameObject, _weaponData._damages);
                    }
                    else
                    {
                        CreateSmoke(_hitInfo.point);
                    }
                }
                _currentAmmo--;
                _textAmmo.text = _currentAmmo.ToString() + "/" + _nbAmmo;
                _timeSinceLastShot = 0;
            }
        }
        else
        {
            StartReload();
        }
    }

    [Command]
    void CmdAttackPlayer(GameObject player, int damage)
    {
        var lifeManager = player.GetComponent<LifeManager>();
        if (lifeManager != null)
        {
            lifeManager.TakeDamage(damage);
        }
    }

    [Command]
    void CmdPlaySound(Vector3 position)
    {
        RpcPlayShootSound(position);
    }

    [ClientRpc]
    void RpcPlayShootSound(Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(_riffleAudioClip, pos);
    }

    public void KillEnemy(GameObject e)
    {
        DestroyEnemyOnServer(e);
    }

    [Command]
    void DestroyEnemyOnServer(GameObject toDestroy)
    {
        NetworkServer.Destroy(toDestroy);
    }

    private void CreateSmoke(Vector3 _position)
    {
        GameObject _smoke = Instantiate(_smokeBulletImpact, _position, Quaternion.identity);
        Destroy(_smoke, 0.2f);
    }

    private void CreateBlood(Vector3 _position)
    {
        GameObject _blood = Instantiate(_bloodEffect, _position, Quaternion.identity);
        Destroy(_blood, 1f);
    }

    [Command]
    private void CmdPlayMuzzleFlash(Vector3 pos)
    {
        RpcPlayMuzzleFlash(pos);
    }

    [ClientRpc]
    private void RpcPlayMuzzleFlash(Vector3 position)
    {
        GameObject _muzzleFlash = Instantiate(_muzzuleFlashEffect, position, _muzzle.rotation);
        _muzzleFlash.transform.position = _muzzle.position + (_muzzleFlash.transform.right * 0.1f);
        _muzzleFlash.GetComponent<ParticleSystem>().Play();
        Destroy(_muzzleFlash, 0.2f);
    }

    private void StartRecoil()
    {
        _recoil.Recoil();
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
        _elapsedTime = 0f;
        while (_elapsedTime < _duration * 10)
        {
            _playerController.Rotation = new Vector2(_playerController.Rotation.x, _playerController.Rotation.y + (_strengh / 10f));

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

    private IEnumerator HitMarker()
    {
        _hitMarker.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        _hitMarker.SetActive(false);
    }
}
