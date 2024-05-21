using UnityEngine;
using Mirror;
using Steamworks;

public class VoiceChat : NetworkBehaviour
{
    private const int SAMPLE_RATE = 11025;
    private byte[] voiceDataBuffer;
    private uint voiceBufferSize = 22050; // Adjust size as needed

    private void Start()
    {
        if (isLocalPlayer)
        {
            voiceDataBuffer = new byte[voiceBufferSize];
        }
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            CaptureAndSendVoiceData();
        }
    }

    private void CaptureAndSendVoiceData()
    {
        uint bytesWritten = 0;
        EVoiceResult result = SteamUser.GetVoice(true, voiceDataBuffer, voiceBufferSize, out bytesWritten);

        if (result == EVoiceResult.k_EVoiceResultOK && bytesWritten > 0)
        {
            CmdSendVoiceData(voiceDataBuffer, bytesWritten);
        }
    }

    [Command]
    private void CmdSendVoiceData(byte[] data, uint size)
    {
        RpcReceiveVoiceData(data, size);
    }

    [ClientRpc]
    private void RpcReceiveVoiceData(byte[] data, uint size)
    {
        if (!isLocalPlayer)
        {
            PlayVoiceData(data, size);
        }
    }

    private void PlayVoiceData(byte[] data, uint size)
    {
        AudioClip clip = AudioClip.Create("Voice", (int)size / 2, 1, SAMPLE_RATE, false);
        float[] samples = new float[size / 2];
        for (int i = 0; i < samples.Length; i++)
        {
            short sample = (short)(data[i * 2] | data[i * 2 + 1] << 8);
            samples[i] = sample / 32768.0f;
        }
        clip.SetData(samples, 0);
        AudioSource.PlayClipAtPoint(clip, transform.position);
    }
}