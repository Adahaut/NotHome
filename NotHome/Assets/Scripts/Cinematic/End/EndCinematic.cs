using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCinematic : MonoBehaviour
{
    [SerializeField] private GameObject _spaceSkybox;
    [SerializeField] private GameObject _asteroid;
    [SerializeField] private ParticleSystem[] _smoke;
    [SerializeField] private GameObject[] _explosion;
    private AudioSource[] _audioSources;

    private void Start()
    {
        _audioSources = GetComponents<AudioSource>();

        _audioSources[0].Play();

        RenderSettings.fog = false;
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
        _audioSources[1].Play();
    }
}
