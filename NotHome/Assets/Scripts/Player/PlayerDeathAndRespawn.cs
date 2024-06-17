using Mirror;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
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


    private Transform _playerRespawnPoint;
    private float _timeToRespawn;
    private bool _canRespawn;

    private void Start()
    {
        _playerTransform = transform;
        _playerRespawnPoint = transform;
        _playerController = GetComponent<PlayerController>();
        _playerLifeManager = GetComponent<LifeManager>();
    }

    public void PlayerDeath()
    {
        if (_playerController.IsDead) return;
        _canRespawn = false;
        _playerTransform.rotation = Quaternion.Euler(0, 0, 90);
        _playerController.IsDead = true;
        _playerInputs.SetActive(false);
        //cameraAnimator.SetBool("Death", true);
        StartCoroutine(DisableCamera(1.5f));
    }

    public void Respawn()
    {
        _noSignal.SetActive(false);
        _playerLifeManager.SetMaxHealth();
        _playerController.IsDead = false;
        _playerTransform = _playerRespawnPoint;
        //cameraAnimator.SetBool("Death", false);    
        StartCoroutine(RespawnAnimation());

    }

    private IEnumerator DisableCamera(float totalTime)
    {
        if(isOwned)
        {
            if (_hasStartedRespawn)
                yield break;

            _hasStartedRespawn = true;

            float time = 0f;
            while(time / totalTime < 1)
            {
                time += Time.deltaTime;
                cameraTransform.position = Vector3.Lerp(cameraTransform.position, cameraTransform.position + new Vector3(0, -1.5f, 0), time / totalTime * 1.1f);
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
            Respawn();
        }
    }

    private void Update()
    {
        if(isOwned)
        {
            if (_timeToRespawn < 0 && _playerController.IsDead && _canRespawn)
            {
                _canRespawn = false;
            }
            else if (_playerController.IsDead)
            {
                _timeToRespawn -= Time.deltaTime;
                _timer.text = "wait " + Mathf.RoundToInt(_timeToRespawn).ToString();
            }
        }
        
    }

}
