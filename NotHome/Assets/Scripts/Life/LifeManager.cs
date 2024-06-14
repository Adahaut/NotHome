using Mirror;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LifeManager : NetworkBehaviour
{
    [SerializeField] private int _maxLife;
    [SyncVar] public int _currentLife;
    private PlayerDeathAndRespawn _playerDeathAndRespawnManager;
    private Animator _animator;

    [SerializeField] private Slider _helthSlider;

    [SerializeField] private AudioClip[] _hitAudioClip;
    private AudioSource[] _audioSource;
    [Header("Only for the player")]
    [SerializeField] private Gradient _damageGradient;
    [SerializeField] private Image _damageIndicator;
    private bool _isBlinking;
    private bool _isTackingDamage;
    private Coroutine _blinking;

    public void SetHealthBar()
    {
        _helthSlider.value = _currentLife;
    }

    public void SetMaxHealth()
    {
        _currentLife = _maxLife;
        _helthSlider.maxValue = _currentLife;
        _helthSlider.value = _currentLife;
    }

    void Start()
    {
        _audioSource = GetComponents<AudioSource>();
        if(_damageIndicator != null)
        {
            _damageIndicator.color = new Color(0, 0, 0, 0);
        }

        _currentLife = _maxLife;

        if (gameObject.tag == "Player")
        {
            _playerDeathAndRespawnManager = GetComponent<PlayerDeathAndRespawn>();
            SetMaxHealth();
        }
        else
        {
            _animator = GetComponent<Animator>();
        }
    }

    public void TakeDamage(int damage, GameObject player = null)
    {
        _currentLife -= damage;

        AudioClip randomClip = _hitAudioClip[Random.Range(0, _hitAudioClip.Length)];
        _audioSource[1].clip = randomClip;
        _audioSource[1].Play();

        if (gameObject.tag == "Player" && _helthSlider != null)
        {
            SetHealthBar();
        }
        else
        {
        //    _animator.SetBool("Hit", true);
        }

        if (_currentLife <= 0)
        {
            if (gameObject.tag == "Enemy")
            {
                EnemyDeath();
                player.GetComponentInChildren<RangeWeapon>().KillEnemy(gameObject);
            }
            else if (gameObject.tag == "Player")
            {
                PlayerDeath();
            }
        }
    }

    private void EnemyDeath()
    {
        if (gameObject.name == "Spider")
        {
            QuestManager.Instance.SetQuestSpider();
        }
        else if (gameObject.name == "X")
        {
            QuestManager.Instance.SetQuestX();
        }

        Destroy(gameObject);
    }

    private void PlayerDeath()
    {
        Debug.Log("playerMort");
        _playerDeathAndRespawnManager.PlayerDeath();
    }

    private IEnumerator UIBlinking(int _steps, float _force, bool _takingDamage)
    {
        _isBlinking = true;
        if(_takingDamage)
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
        _isBlinking = false;
    }

    private void Update()
    {
        if(gameObject.tag == "Player")
        {
            if(!_isBlinking && _currentLife < _maxLife && !_isTackingDamage)
            {
                StartBlinking();
            }
        }
    }

    private void StartBlinking(bool _takingDamage = false)
    {
        print(_takingDamage);
        float _force = _takingDamage == true ? 1f : (float)(_maxLife - _currentLife) / (float)_maxLife * 0.2f;
        print(_force);
        _blinking = StartCoroutine(UIBlinking(50, _force, _takingDamage));
    }

}
