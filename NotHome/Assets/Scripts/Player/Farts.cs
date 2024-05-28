using System.Collections.Generic;
using UnityEngine;

public class Farts : MonoBehaviour
{
    [SerializeField] private List<AudioClip> _fartsSound = new List<AudioClip>();
    private AudioSource _fartOrigine;
    [SerializeField] private GameObject _fartParticle;

    private void Awake()
    {
        _fartOrigine = transform.GetChild(6).GetComponent<AudioSource>();
    }

    public void PlayRandomFartSound()
    {
        int _randomFartIndex = Random.Range(0, _fartsSound.Count);
        _fartOrigine.clip = _fartsSound[_randomFartIndex];
        _fartOrigine.Play();
        GameObject _newFartParticles = Instantiate(_fartParticle);
        _newFartParticles.transform.position = transform.position;
        _newFartParticles.transform.rotation = Quaternion.Inverse(transform.rotation) ;
        _newFartParticles.transform.SetParent(null);
        _newFartParticles.GetComponent<ParticleSystem>().Play();
        Destroy(_newFartParticles, 2);
    }
}
