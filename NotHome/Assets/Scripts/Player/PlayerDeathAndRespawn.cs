using Mirror;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerDeathAndRespawn : NetworkBehaviour
{
    private Transform _playerTransform;
    private PlayerController _playerController;
    [SerializeField] private GameObject _noSignal;
    [SerializeField] private TextMeshProUGUI _timer;
    [SerializeField] private GameObject _playerInputs;
    [SerializeField] private Image _respawnScreen;
    [SerializeField] private GameObject _playerUI;
    private LifeManager _playerLifeManager;
    private bool _hasStartedRespawn;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private GameObject[] playerMesh;

    private Vector3 cameraSpawnTransform;
    private Quaternion cameraSpawnRotation;
    private Vector3 _playerRespawnPoint;
    private float _timeToRespawn;
    private bool _canRespawn;

    private void Start()
    {
        _playerTransform = transform;
        _playerRespawnPoint = transform.position;
        cameraSpawnTransform = cameraTransform.position;
        cameraSpawnRotation = cameraTransform.rotation;
        _playerController = GetComponent<PlayerController>();
        _playerLifeManager = GetComponent<LifeManager>();
    }

    public void PlayerDeath()
    {
        _playerController = GetComponent < PlayerController > ();
        if (_playerInputs == null) print("player input null"); 
        if (_playerController.IsDead) return;
        _canRespawn = false;
        _playerController.IsDead = true;
        _playerInputs.SetActive(false);
        transform.position = Vector3.zero;
        
        StartCoroutine(DisableCamera(0.5f));
        
    }

    public void Respawn()
    {
        _noSignal.SetActive(false);
        _playerLifeManager.SetMaxHealth();
        _playerController.IsDead = false;
        transform.position = _playerRespawnPoint;
        transform.rotation = cameraSpawnRotation;
        cameraTransform.position = cameraSpawnTransform;
        cameraTransform.rotation = cameraSpawnRotation;

        StartCoroutine(RespawnAnimation());
    }

    private IEnumerator DisableCamera(float totalTime)
    {
        if(isOwned)
        {
            Vector3 _initCamPos = cameraTransform.position;
            if (_hasStartedRespawn)
                yield break;

            _hasStartedRespawn = true;

            float time = 0f;
            while(time / totalTime < 1)
            {
                time += Time.deltaTime;
                cameraTransform.position = Vector3.Lerp(_initCamPos, _initCamPos + new Vector3(2, -1.5f, 2), time / totalTime);
                cameraTransform.rotation = Quaternion.Euler(cameraTransform.rotation.x + (90 * time / totalTime), cameraTransform.rotation.y + (180*time/totalTime),cameraTransform.rotation.z) ;
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(0.5f);
            _noSignal.SetActive(true);
            _timeToRespawn = 10;
            _canRespawn = true;
        }
        
    }

    private IEnumerator RespawnAnimation()
    {
        if(isOwned)
        {
            _respawnScreen.gameObject.SetActive(true);
            float _factor = 1f / 10f;
            for (float i = 1f; i < 10f; i++)
            {
                _respawnScreen.color = new Color(255, 255, 255, 1f - _factor * i);
                yield return new WaitForSeconds(0.1f);
            }
            _respawnScreen.gameObject.SetActive(false);
            int _numberOfBlink = Random.Range(2, 5);
            for (int i = 0; i < _numberOfBlink; i++)
            {
                float _waitingTime = Random.Range(0.5f, 1.5f);
                float _interval = _waitingTime + Random.Range(0.1f, 0.6f);
                StartCoroutine(StartBlink(_waitingTime));
                yield return new WaitForSeconds(_interval);
            }
            _playerInputs.SetActive(true);
            _hasStartedRespawn = false;
        }
    }

    private IEnumerator StartBlink(float _waitingTime)
    {
        if(isOwned)
            _playerUI.SetActive(false);
        yield return new WaitForSeconds(_waitingTime);
        if(isOwned)
        {
            _playerUI.SetActive(true);
            transform.position = _playerRespawnPoint;
        }
    }

    private void Update()
    {
        if (isOwned)
        {
            if (_timeToRespawn <= 0 && _playerController.IsDead && _canRespawn)
            {
                _canRespawn = false;
                Respawn();
            }
            else if (_playerController.IsDead)
            {
                _timeToRespawn -= Time.deltaTime;
                _timer.text = "wait " + Mathf.RoundToInt(_timeToRespawn).ToString();
            }
        }
        
    }

}
