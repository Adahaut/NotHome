using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDoorsQG : NetworkBehaviour
{
    private Animator _doorAnimator;
    [SerializeField] private AudioClip _doorClip;

    [SyncVar] private int _currentPlayerCount;

    private void Start()
    {
        _doorAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _currentPlayerCount++;
            if(!_doorAnimator.GetBool("Open"))
            {
                _doorAnimator.SetBool("Open", true);
                AudioSource.PlayClipAtPoint(_doorClip, transform.position, 0.25f);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _currentPlayerCount--;
            if(_currentPlayerCount <= 0)
            {
                _currentPlayerCount = 0;
                _doorAnimator.SetBool("Open", false);
                AudioSource.PlayClipAtPoint(_doorClip, transform.position, 0.25f);
            }
        }
    }
}
