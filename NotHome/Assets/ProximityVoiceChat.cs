using UnityEngine;
using Mirror;
using Steamworks;
using TMPro;

public class ProximityVoiceChat : NetworkBehaviour
{
    public float voiceRange = 10f;
    private const int SAMPLE_RATE = 11025;
    private byte[] voiceDataBuffer;
    private int voiceBufferSize = 44100; // Adjust size as needed
    private AudioSource audioSource;
    int t = 0;
    public TMP_Text test;

    private void Start()
    {
        if (isOwned)
        {
            SteamUser.StartVoiceRecording();
            voiceDataBuffer = new byte[voiceBufferSize];
            audioSource.playOnAwake = false;
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (isOwned)
        {
            CaptureAndSendVoiceData();
        }
    }

    private void CaptureAndSendVoiceData()
    {
        EVoiceResult voiceResult = SteamUser.GetAvailableVoice(out uint compressed);
        
        if (voiceResult == EVoiceResult.k_EVoiceResultOK && compressed > 1024)
        {
            byte[] byteBuffer = new byte[1024];
            voiceResult = SteamUser.GetVoice(true, byteBuffer, 1024, out uint bufferSize);

            if (voiceResult == EVoiceResult.k_EVoiceResultOK && bufferSize > 0)
            {
                foreach (var player in FindObjectsOfType<ProximityVoiceChat>())
                {
                    if (player != this && Vector3.Distance(transform.position, player.transform.position) <= voiceRange)
                    {
                        player.RpcReceiveVoiceData(byteBuffer, bufferSize);
                    }
                }
            }
        }
    }

    [ClientRpc]
    private void RpcReceiveVoiceData(byte[] data, uint size)
    {
        if (!isOwned)
        {
            PlayVoiceData(data, size);
        }
    }

    private void PlayVoiceData(byte[] byteBuffer, uint byteCount)
    {
        byte[] destBuffer = new byte[44100 * 2];
        EVoiceResult voiceResult = SteamUser.DecompressVoice(byteBuffer, byteCount, destBuffer, (uint)destBuffer.Length, out uint bytesWritten, 44100);

        if (voiceResult == EVoiceResult.k_EVoiceResultOK && bytesWritten > 0)
        {
            if (audioSource == null)
            {
                test.text = "audio source null" + t.ToString();
            }
            t += 1;

            audioSource.clip = AudioClip.Create(UnityEngine.Random.Range(100, 1000000).ToString(), 44100, 1, 44100, false);

            if (audioSource.clip == null)
            {
                test.text = "audio clip null !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!";
            }
            float[] testa = new float[44100];
            for (int i = 0; i < testa.Length; ++i)
            {
                testa[i] = (short)(destBuffer[i * 2] | destBuffer[i * 2 + 1] << 8) / 32768.0f;
            }
            audioSource.clip.SetData(testa, 0);
            audioSource.Play();
        }
    }
}