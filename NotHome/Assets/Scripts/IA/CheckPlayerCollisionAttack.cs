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
            var playerLifeManager = other.GetComponent<LifeManager>();
            if (playerLifeManager != null)
            {
                DealDamage(playerLifeManager);
            }
        }
    }

    [Server]
    private void DealDamage(LifeManager playerLifeManager)
    {
        playerLifeManager.TakeDamage(_damages);
    }
}
