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
    private bool _isOnChair;
    private bool _isMouv;
    [SerializeField] private float _distRayCast;

    public static OfficeManager Instance;

    private void Awake()
    {
        Instance = this;
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
            Vector3 start_pos = _player.transform.position;
            while (time / total_time < 1)
            {
                time += Time.deltaTime * _speed;
                _player.transform.position = Vector3.Lerp(start_pos, end_pos, time / total_time);
                yield return null;
            }
            if (time / total_time >= 1)
                StartCoroutine(MouvCam());
        }
    }
    private IEnumerator MouvCam()
    {
        if (_isMouv)
        {
            float posY = _camera.position.y;
            while (_camera.position.y > posY / 1.5f)
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
            float posY = _camera.position.y;
            while (_camera.position.y < posY * 1.5f)
            {
                _camera.position += new Vector3(0, _speedCamera * Time.deltaTime, 0);
                yield return null;
            }
            _isOnChair = false;
            _isMouv = false;
            _camera.position = new Vector3(_camera.position.x, 2.50f, _camera.position.z);
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
        if (!_isMouv && _isOnChair)
        {
            StartCoroutine(BackChair());
        }
    }
}
