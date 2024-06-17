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
            AttackPlayer(other.gameObject);
        }
    }

    [Server]
    private void AttackPlayer(GameObject player)
    {
        var lifeManager = player.GetComponent<LifeManager>();
        if (lifeManager != null)
        {
            lifeManager.ServTakeDamage(_damages);
        }
    }
}
