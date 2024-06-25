using Mirror;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : NetworkBehaviour
{
    [SerializeField] private int _maxLife;

    [SyncVar(hook = nameof(OnLifeChanged))] public int _currentLife;

    private PlayerDeathAndRespawn _playerDeathAndRespawnManager;
    private Animator _animator;

    [SerializeField] private Slider _healthSlider;

    [SerializeField] private AudioClip _hitAudioClip;
    private AudioSource _audioSource;

    [Header("Only for the player")]
    [SerializeField] private Gradient _damageGradient;
    [SerializeField] private Image _damageIndicator;
    private bool _isBlinking;
    private bool _isTackingDamage;
    private Coroutine _blinking;

    bool isDead;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if(_damageIndicator != null)
        {
            _damageIndicator.color = new Color(0, 0, 0, 0);
        }

        if (isServer)
        {
            _currentLife = _maxLife;
        }

        if (gameObject.tag == "Player")
        {
            _playerDeathAndRespawnManager = GetComponent<PlayerDeathAndRespawn>();
        }
        else
        {
            _animator = GetComponent<Animator>();
        }
    }


    public int MaxLife() { return _maxLife; }

    public void SetMaxHealth()
    {
        _currentLife = _maxLife;
        StartBlinking(false);
        isDead = false;
    }

    void OnLifeChanged(int oldLife, int newLife)
    {
        if (newLife <= 0)
        {
            if (gameObject.tag == "Enemy")
            {
                RpcEnemyDeath();
            }
            else if (gameObject.tag == "Player" &&!isDead)
            {
                RpcPlayerDeath();
                isDead = true;
            }
        }
    }

    [Server]
    public void TakeDamage(int damage)
    {
        if(gameObject.tag == "Player")
        {
            _currentLife -= damage;
            StartBlinking(true);
        }
        else if(gameObject.tag == "Enemy")
        {
            _currentLife -= damage;
        }

        //if (_currentLife <= 0)
        //{
        //    if (gameObject.tag == "Enemy")
        //    {
        //        RpcEnemyDeath();
        //    }
        //    else if (gameObject.tag == "Player")
        //    {
        //        RpcPlayerDeath();
        //    }
        //}

        RpcPlayHitSound(transform.position);

    }


    [ClientRpc]
    private void RpcPlayHitSound(Vector3 position)
    {
        //_audioSource.clip = _hitAudioClip;
        //_audioSource.Play();
        AudioSource.PlayClipAtPoint(_hitAudioClip, position);
    }

    [ClientRpc]
    private void RpcEnemyDeath()
    {
        if (isServer)
        {
            print(gameObject);
            if (gameObject.name == "Spider(Clone)")
            {
                QuestManager.Instance.SetQuestSpider();
            }
            else if (gameObject.name == "SlenderManUpdate(Clone)")
            {
                QuestManager.Instance.QuestComplete(6);
            }

            NetworkServer.Destroy(gameObject);
        }
    }

    //[ClientRpc]
    private void RpcPlayerDeath()
    {
        if (_playerDeathAndRespawnManager != null)
        {
            _playerDeathAndRespawnManager.PlayerDeath();
        }
    }

    private IEnumerator UIBlinking(int _steps, float _force, bool _takingDamage)
    {
        _isBlinking = true;
        if(_currentLife != _maxLife)
        {
            if (_takingDamage)
            {
                _damageIndicator.color = _damageGradient.Evaluate(100f);
                yield return new WaitForEndOfFrame();
                for (float i = _steps + 1; i > 1; i--)
                {
                    _damageIndicator.color = _damageGradient.Evaluate(i / ((float)_steps + 1f) * _force);
                    yield return new WaitForEndOfFrame();
                }
            }
            else
            {
                for (float i = 1; i < _steps + 1; i++)
                {
                    _damageIndicator.color = _damageGradient.Evaluate(i / ((float)_steps + 1f) * _force);
                    yield return new WaitForEndOfFrame();
                }
                for (float i = _steps + 1; i > 1; i--)
                {
                    _damageIndicator.color = _damageGradient.Evaluate(i / ((float)_steps + 1f) * _force);
                    yield return new WaitForEndOfFrame();
                }
            }
        }
        
        _isBlinking = false;
    }

    private void Update()
    {
        if (gameObject.tag == "Player")
        {
            if(!_isBlinking && _currentLife < 20 && !_isTackingDamage && !GetComponent<PlayerController>().IsDead)
            {
                StartBlinking();
            }
        }
    }

    public void StartBlinking(bool _takingDamage = false)
    {
        float _force = _takingDamage == true ? 1f : (float)(_maxLife - _currentLife) / (float)_maxLife * 0.2f;
        _blinking = StartCoroutine(UIBlinking(50, _force, _takingDamage));
    }

}
