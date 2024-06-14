using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerCollisionAttack : MonoBehaviour
{
    [SerializeField] private int _damages;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("collisionPlayer");
            other.GetComponent<LifeManager>().TakeDamage(_damages);
        }
    }
}
