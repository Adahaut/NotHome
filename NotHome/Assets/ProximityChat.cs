using Mirror;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProximityChat : NetworkBehaviour
{
    private AudioSource _audioSource;
    [HideInInspector] public AudioClip _microphoneClip;
    private string _microphoneName;

    private GameObject[] _player;

    public TMP_Text text;
    int count = 0;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        //Get default microphone
        if (Microphone.devices.Length > 0)
        {
            _microphoneName = Microphone.devices[0];
            Debug.Log("Using microphone: " + _microphoneName);
        }
        else
        {
            Debug.LogError("No microphone found!");
            return;
        }

        //Set the clip of the AudioSource of the other players
        _microphoneClip = Microphone.Start(_microphoneName, true, 10, 44100);
        _audioSource.mute = true;
        _audioSource.loop = true;
        _audioSource.clip = _microphoneClip;

        while (!(Microphone.GetPosition(_microphoneName) > 0)) { }
    }

    public override void OnStartAuthority()
    {
        _player = GameObject.FindGameObjectsWithTag("Player");
        _audioSource.Play();


        foreach (var player in _player)
        {
            if(player != this.gameObject)
            {
                GameObject playerAudio = new GameObject(player.name + " Audio");
                playerAudio.transform.parent = this.transform;
                AudioSource audioSource = playerAudio.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
                audioSource.clip = player.GetComponent<ProximityChat>()._microphoneClip;
                audioSource.loop = true;
                audioSource.Play();

                count++;
            }

            text.text = count.ToString();
        }
    }



    void OnApplicationQuit()
    {
        if (Microphone.IsRecording(_microphoneName))
        {
            Microphone.End(_microphoneName);
        }
    }
}
