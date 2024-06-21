using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerCollisionAttack : NetworkBehaviour
{
    [SerializeField] private int _damages;
    public GameObject _bloodParticle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.gameObject != transform.root.gameObject)
            {
                var playerLifeManager = other.GetComponent<LifeManager>();
                if (playerLifeManager != null)
                {
                    DealDamage(playerLifeManager);

                    if(_bloodParticle != null)
                    {
                        CreateBlood(transform.position);
                    }
                }
            }
        }
        
        if (other.CompareTag("Enemy"))
        {
            var enemyLifeManager = other.GetComponent<LifeManager>();
            if (enemyLifeManager != null)
            {
                DealDamage(enemyLifeManager);

                if (_bloodParticle != null)
                {
                    CreateBlood(transform.position);
                }
            }
        }
    }

    [Server]
    private void DealDamage(LifeManager playerLifeManager)
    {
        playerLifeManager.TakeDamage(_damages);
    }

    private void CreateBlood(Vector3 _position)
    {
        GameObject _blood = Instantiate(_bloodParticle, _position, Quaternion.identity);
        Destroy(_blood, 1f);
    }
}
