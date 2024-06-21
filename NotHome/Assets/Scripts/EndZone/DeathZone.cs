using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            var _lifeManager = other.GetComponent<LifeManager>();
            if(_lifeManager != null )
            {
                DealDamage(_lifeManager);
            }
        }
    }

    [Server]
    private void DealDamage(LifeManager playerLifeManager)
    {
        playerLifeManager.TakeDamage(999);
    }
}
