using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCinematic : MonoBehaviour
{
    private GameObject[] _playersRef;
    [SerializeField] private Animator _animator;
    [SerializeField] private Animator _shieldAnim;
    [SerializeField] private ParticleSystem _explosion;
    [SerializeField] private ParticleSystem _smoke;
    [SerializeField] private ParticleSystem _fire;
    [SerializeField] private Transform _explosionTransform;
    [SerializeField] private Transform _smokeTransform;
    [SerializeField] private Transform _fireTransform;


    private void Start()
    {
        _playersRef = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < _playersRef.Length; i++)
        {
            _playersRef[i].SetActive(false);
        }

        _animator.speed = 3.0f;
        
    }

    public void DeployShieldAnim()
    {
        _shieldAnim.enabled = true;        
    }

    public void PlayExplosion()
    {
        _explosion.Play();
    }

    public void PlaySmoke()
    {
        _smoke.Play();
    }

    public void PlayFire() 
    {
        _fire.Play();
    }
}
