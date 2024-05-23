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
    public static Action _reloading;
    public static Action _aimAction;
    public static Action _stopAimAction;
    public bool _isRangeWeaponEqupiped;
    

    public void Attack(InputAction.CallbackContext context)
    {
        if(_isRangeWeaponEqupiped)
        {
            _shootAction?.Invoke();
        }
        else
        {
            StartCoroutine(ActiveDesactiveCollider());
        }
        
    }

    public void Aim(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            print("aim");
            _aimAction?.Invoke();
        }
        else if (context.canceled)
        {
            print("stop aim");
            _stopAimAction?.Invoke();
        }
        

    }

    public void Reload(InputAction.CallbackContext context)
    {
        _reloading?.Invoke();
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
