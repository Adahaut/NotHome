using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{
    [SerializeField] private int _maxLife;
    public int _currentLife;
    private PlayerDeathAndRespawn _playerDeathAndRespawnManager;
    private Animator _animator;

    [SerializeField] private Slider _helthSlider;

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
        _currentLife = _maxLife;

        if(gameObject.tag == "Player")
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

        if (gameObject.tag == "Player" && _helthSlider != null)
        {
            SetHealthBar();
        }            
        else
        {
            _animator.SetBool("Hit", true);
        }            

        if (_currentLife <= 0)
        {
            if(gameObject.tag == "Enemy")
            {
                EnemyDeath();
                player.GetComponentInChildren<RangeWeapon>().KillEnemy(gameObject);
            }
            else if(gameObject.tag == "Player")
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
}