using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Collider _enemyDetectionCollider;
    [SerializeField] private int _damages;
    public static Action _shootAction;

    public void Attack(InputAction.CallbackContext context)
    {
        //StartCoroutine(ActiveDesactiveCollider());
        _shootAction?.Invoke();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.GetComponent<LifeManager>().TakeDamage(_damages);
        }
    }

    public IEnumerator ActiveDesactiveCollider()
    {
        _enemyDetectionCollider.enabled = true;
        yield return new WaitForSeconds(1f);
        _enemyDetectionCollider.enabled = false;
    }
}
