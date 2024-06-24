using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveDesactiveCacCollider : MonoBehaviour
{
    public Collider _enemyDetectionCollider;
    public AudioSource _audioSource;
    public List<AudioClip> _swordSfx = new List<AudioClip>();
    public CheckPlayerCollisionAttack _playerEnemieDetection;

    private void Start()
    {
        _playerEnemieDetection = GetComponentInChildren<CheckPlayerCollisionAttack>();
    }

    public void ActiveCollider()
    {
        _enemyDetectionCollider.enabled = true;
        StartCoroutine(WaitAndPlaySFX());
    }

    public void DisableCollider()
    {
        _enemyDetectionCollider.enabled = false;
    }

    private IEnumerator WaitAndPlaySFX()
    {
        yield return new WaitForSeconds(0.1f);
        int clipindex = _playerEnemieDetection._hit == true ? 1 : 0;
        _audioSource.PlayOneShot(_swordSfx[clipindex]);
        _playerEnemieDetection._hit = false;
    }

}
