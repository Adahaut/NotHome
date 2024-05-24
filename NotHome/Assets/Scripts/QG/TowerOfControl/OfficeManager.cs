using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class OfficeManager : MonoBehaviour
{
    [SerializeField] private Transform _chair;
    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _camera;
    [SerializeField] private float _speed;
    [SerializeField] private float _speedCamera;
    [SerializeField] private float _speedZoom;
    private bool _isOnChair;
    private bool _isMouv;
    private bool _isZoomed;
    [SerializeField] private float _distRayCast;
    [SerializeField] private float _mouvCam;

    public static OfficeManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private IEnumerator CharacterMove(float total_time)
    {
        if (!_isMouv)
        {
            _player.GetComponentInChildren<PlayerInput>().actions.actionMaps[0].actions[0].Disable();
            _player.GetComponentInChildren<PlayerInput>().actions.actionMaps[0].actions[1].Disable();
            _isMouv = true;
            float time = 0f;
            Vector3 end_pos = _chair.position;
            end_pos.y = _player.transform.position.y;
            Vector3 start_pos = _player.transform.position;
            while (time / total_time < 1)
            {
                print(Vector3.Distance(_player.transform.position, _chair.position));
                if (Vector3.Distance(_player.transform.position, _chair.position) < 0.3)
                {
                    time = total_time * 2;
                    yield return null;
                }
                else
                {
                    time += Time.deltaTime * _speed;
                    _player.transform.position = Vector3.Lerp(start_pos, end_pos, time / total_time);
                    yield return null;
                }
            }
            if (time / total_time >= 1)
                StartCoroutine(MouvCam());
        }
    }
    private IEnumerator MouvCam()
    {
        if (_isMouv)
        {
            while (_camera.position.y > _player.transform.position.y + 1 - _mouvCam)
            {
                _camera.position -= new Vector3(0, _speedCamera * Time.deltaTime, 0);
                yield return null;
            }
            _isOnChair = true;
            _isMouv = false;
        }
    }
    private IEnumerator BackChair()
    {
        if (!_isMouv)
        {
            _isMouv = true;
            while (_camera.position.y < _player.transform.position.y + 0.5f + _mouvCam)
            {
                _camera.position += new Vector3(0, _speedCamera * Time.deltaTime, 0);
                yield return null;
            }
            _isOnChair = false;
            _isMouv = false;
            _camera.position = new Vector3(_camera.position.x, _player.transform.position.y + 1, _camera.position.z);
            _player.GetComponentInChildren<PlayerInput>().actions.actionMaps[0].actions[0].Enable();
            _player.GetComponentInChildren<PlayerInput>().actions.actionMaps[0].actions[1].Enable();
        }
    }
    public void MouvToChair()
    {
        if (Physics.Raycast(_camera.position, _camera.forward, out RaycastHit hit, _distRayCast) && hit.collider.CompareTag("Chair"))
        {
            if (!_isMouv && !_isOnChair)
            {
                StartCoroutine(CharacterMove(1));
            }
        }
        if (!_isMouv && _isOnChair && _camera.GetComponent<Camera>().fieldOfView == 60 && !_isZoomed)
        {
            StartCoroutine(BackChair());
        }
    }
    private void Update()
    {
        if (_isOnChair && Input.GetMouseButtonDown(0) && !_isZoomed && !_isMouv)
        {
            ZoomScreen();
        }
    }
    private void ZoomScreen()
    {
        if (Physics.Raycast(_camera.position, _camera.forward, out RaycastHit hit, 10) && hit.collider.CompareTag("RawImage"))
        {
            StartCoroutine(AnimZoom(_camera.GetComponent<Camera>().fieldOfView));
        }
    }
    private IEnumerator AnimZoom(float fov)
    {
        _isZoomed = true;
        if (fov > 30)
        {
            _player.GetComponentInChildren<PlayerInput>().actions.actionMaps[0].actions[6].Disable();
            while (_camera.GetComponent<Camera>().fieldOfView > 30)
            {
                _camera.GetComponent<Camera>().fieldOfView -= _speedZoom * Time.deltaTime;
                yield return null;
            }
            _camera.GetComponent<Camera>().fieldOfView = 30;
        }
        else
        {
            _player.GetComponentInChildren<PlayerInput>().actions.actionMaps[0].actions[6].Enable();
            while (_camera.GetComponent<Camera>().fieldOfView < 60)
            {
                _camera.GetComponent<Camera>().fieldOfView += _speedZoom * Time.deltaTime;
                yield return null;
            }
            _camera.GetComponent<Camera>().fieldOfView = 60;
        }
        _isZoomed = false;
    }
}
