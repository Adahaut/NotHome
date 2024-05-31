using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCinematic : MonoBehaviour
{
    private GameObject[] _playersRef;
    [SerializeField] private Animator _animator;
    [SerializeField] private Animator _shieldAnim;
    [SerializeField] private ParticleSystem[] _smoke;
    [SerializeField] private GameObject[] _explosion;


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

    public void PlaySmoke(int _index)
    {
        if (_smoke[_index].isPlaying)
        {
            _smoke[_index].Pause();
        }
        else
        {
            _smoke[_index].Play();
        }        
    }    

    public void PlayExplosion(int _index)
    {
        ParticleSystem[] particleSystems = _explosion[_index].GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in particleSystems)
        {
            ps.Play();
        }
    }
}

