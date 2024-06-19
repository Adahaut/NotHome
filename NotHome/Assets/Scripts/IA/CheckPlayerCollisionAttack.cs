using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerCollisionAttack : NetworkBehaviour
{
    [SerializeField] private int _damages;

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
                }
            }
        }

        if (other.CompareTag("Enemy"))
        {
            var enemyLifeManager = other.GetComponent<LifeManager>();
            if (enemyLifeManager != null)
            {
                DealDamage(enemyLifeManager);
            }
        }
    }

    [Server]
    private void DealDamage(LifeManager playerLifeManager)
    {
        playerLifeManager.TakeDamage(_damages);
    }
}
