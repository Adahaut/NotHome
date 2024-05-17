using Mirror;
using UnityEngine;

public class ProximityChat : NetworkBehaviour
{
    private AudioSource _audioSource;
    private string _microphone;

    public override void OnStartAuthority()
    {
        _audioSource = GetComponent<AudioSource>();

        if (Microphone.devices.Length > 0)
        {
            _microphone = Microphone.devices[0];
            CmdPlaySound();
        }
        else
        {
            Debug.LogError("No microphone found!");
        }
    }

    [Command]
    public void CmdPlaySound()
    {
        RpcPlaySound();
    }

    [ClientRpc]
    void RpcPlaySound()
    {
        _audioSource.clip = Microphone.Start(_microphone, true, 10, 44100);
        _audioSource.loop = true;
        _audioSource.mute = true;

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
