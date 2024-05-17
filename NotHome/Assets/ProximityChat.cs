using Mirror;
using UnityEngine;

public class ProximityChat : NetworkBehaviour
{
    private AudioSource _audioSource;
    private string _microphone;

    public override void OnStartAuthority()
    {
        if (Microphone.devices.Length > 0)
        {
            _microphone = Microphone.devices[0];
            Debug.Log("Using microphone: " + _microphone);
        }
        else
        {
            Debug.LogError("No microphone found!");
            return;
        }

        _audioSource = GetComponent<AudioSource>();
        
        _audioSource.clip = Microphone.Start(_microphone, true, 10, 44100);
        while (!(Microphone.GetPosition(_microphone) > 0)) { }
        _audioSource.Play();
    }

    void OnApplicationQuit()
    {
        if (Microphone.IsRecording(_microphone))
        {
            Microphone.End(_microphone);
        }
    }
}
