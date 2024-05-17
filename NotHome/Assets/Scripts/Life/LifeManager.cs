using UnityEngine;

public class LifeManager : MonoBehaviour
{
    [SerializeField] private int _maxLife;
    public int _currentLife;

    void Start()
    {
        _currentLife = _maxLife;
    }

    public void TakeDamage(int damage)
    {
        _currentLife -= damage;

        if(_currentLife <= 0)
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
        //gestion du respawn
    }
}