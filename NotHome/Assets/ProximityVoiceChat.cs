using UnityEngine;
using Mirror;
using Steamworks;
using TMPro;

public class ProximityVoiceChat : NetworkBehaviour
{
    public float voiceRange = 10f;
    private const int SAMPLE_RATE = 11025;
    private byte[] voiceDataBuffer;
    private uint voiceBufferSize = 22050; // Adjust size as needed


    public TMP_Text test;

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
            test.text = bytesWritten.ToString();
            foreach (var player in FindObjectsOfType<ProximityVoiceChat>())
            {
                if (player != this && Vector3.Distance(transform.position, player.transform.position) <= voiceRange)
                {
                    player.RpcReceiveVoiceData(voiceDataBuffer, bytesWritten);
                }
            }
        }
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