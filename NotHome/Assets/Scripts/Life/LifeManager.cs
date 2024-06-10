using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{
    [SerializeField] private int _maxLife;
    public int _currentLife;
    private PlayerDeathAndRespawn _playerDeathAndRespawnManager;

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
    }

    public void TakeDamage(int damage)
    {
        _currentLife -= damage;
        print(_currentLife);
        if (gameObject.tag == "Player" && _helthSlider != null)
            SetHealthBar();

        if (_currentLife <= 0)
        {
            if(gameObject.tag == "Enemy")
            {
                EnemyDeath();
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