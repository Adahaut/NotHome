using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveDesactiveCacCollider : MonoBehaviour
{
    public Collider _enemyDetectionCollider;
    public AudioSource _audioSource;

    public void ActiveCollider()
    {
        _audioSource.Play();
        _enemyDetectionCollider.enabled = true;
    }

    public void DisableCollider()
    {
        _enemyDetectionCollider.enabled = false;
    }
}
