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
            print("player");
            _playerDeathAndRespawnManager = GetComponent<PlayerDeathAndRespawn>();
            SetMaxHealth();
        }
    }

    public void TakeDamage(int damage)
    {
        if(_currentLife < 0)
            return;


        print("take damage");
        _currentLife -= damage;
        if (gameObject.tag == "Player")
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
        Destroy(gameObject);
    }

    private void PlayerDeath()
    {
        Debug.Log("playerMort");
        _playerDeathAndRespawnManager.PlayerDeath();
    }
}