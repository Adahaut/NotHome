using UnityEngine;

public class PlayCinematic : MonoBehaviour
{
    private GameObject[] _playersRef;
    [SerializeField] private Animator _animator;
    [SerializeField] private Animator _shieldAnim;
    [SerializeField] private ParticleSystem[] _smoke;
    [SerializeField] private GameObject[] _explosion;
    private AudioSource[] _audioSources;


    private void Start()
    {
        _playersRef = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < _playersRef.Length; i++)
        {
            _playersRef[i].SetActive(false);
        }

        _animator.speed = 3.0f;
        
        _audioSources = GetComponents<AudioSource>();
        _audioSources[0].Play();
        _audioSources[1].Play();
        _audioSources[2].Play();
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

    public void PlayExplosionSound()
    {
        _audioSources[2].Play();
    }

    public void PlayBigExplosion()
    {
        _audioSources[3].Play();
    }

    public void StopAlarm()
    {
        _audioSources[1].Stop();
    }

    public void PlayCrashSound()
    {
        _audioSources[4].Play();
    }

    public void stopSounds()
    {
        for(int i = 0;  i < 3; i++)
        {
            _audioSources[i].Stop();
        }
    }
}