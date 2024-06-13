using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundZone : MonoBehaviour
{
    private string _zone;
    [SerializeField] private int _chanceSound;
    private AudioSource _audioSource;
    public static SoundZone Instance;

    [Header("Audio Clip")]
    [SerializeField] private List<AudioClip> _soundMountain;
    [SerializeField] private List<AudioClip> _soundForest;
    [SerializeField] private List<AudioClip> _soundDesert;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    public void SetZone(string zone)
    {
        if (_zone != zone)
        {
            _zone = zone;
            PlaySound();
        }
    }

    public void PlaySound()
    {
        StopAllCoroutines();
        switch (_zone)
        {
            case "Mountain":
                StartCoroutine(StartSound(_soundMountain));
                break;
            case "Forest":
                StartCoroutine(StartSound(_soundForest));
                break;
            case "Desert":
                StartCoroutine(StartSound(_soundDesert));
                break;
            default:
                Debug.Log("No zone");
                break;
        }
    }
    IEnumerator StartSound(List<AudioClip> audioclips)
    {
        if (Random.Range(0,_chanceSound) == 0)
        {
            _audioSource.clip = audioclips[Random.Range(0, audioclips.Count)];
            _audioSource.Play();


        }
        yield return new WaitForSeconds(1);
        StartCoroutine(StartSound(audioclips));
    }
}
