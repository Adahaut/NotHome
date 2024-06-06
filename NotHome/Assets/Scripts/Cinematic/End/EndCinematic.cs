using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCinematic : MonoBehaviour
{
    [SerializeField] private GameObject _spaceSkybox;
    [SerializeField] private GameObject _asteroid;
    [SerializeField] private ParticleSystem[] _smoke;
    [SerializeField] private GameObject[] _explosion;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void ActiveSpaceSkybox()
    {
        _spaceSkybox.SetActive(true);
    }

    public void ActiveAsteroid()
    {
        _asteroid.SetActive(true);
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
