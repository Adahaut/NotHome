using UnityEngine;
using Mirror;
using Steamworks;

public class LobbyVoiceChat : NetworkBehaviour
{
    public AudioSource audioSource;

    [SerializeField] private float maxDistance = 15f;

    private void Start()
    {
        if (isOwned)
        {
            SteamUser.StartVoiceRecording();
            audioSource.volume = 0f;
            Debug.Log("Record Start");
        }
        else
        {
            audioSource.volume = 1f;
        }
    }
    private void Update()
    {
        if (isOwned)
        {
            uint compressed;
            EVoiceResult ret = SteamUser.GetAvailableVoice(out compressed);
            if (ret == EVoiceResult.k_EVoiceResultOK && compressed > 1024)
            {
                byte[] destBuffer = new byte[8192];
                uint bytesWritten;
                ret = SteamUser.GetVoice(true, destBuffer, 8192, out bytesWritten);
                if (ret == EVoiceResult.k_EVoiceResultOK && bytesWritten > 0)
                {
                    Cmd_SendData(destBuffer, bytesWritten);
                }
            }
        }
    }

    [Command(channel = 2)]
    void Cmd_SendData(byte[] data, uint size)
    {
        Debug.Log("Command");
        ProximityVoiceChat[] players = FindObjectsOfType<ProximityVoiceChat>();

        for (int i = 0; i < players.Length; i++)
        {
            Target_PlaySound(players[i].GetComponent<NetworkIdentity>().connectionToClient, data, size);
        }
    }



    [TargetRpc(channel = 2)]
    void Target_PlaySound(NetworkConnection conn, byte[] destBuffer, uint bytesWritten)
    {
        Debug.Log("Target");
        byte[] destBuffer2 = new byte[44100 * 2];
        uint bytesWritten2;
        EVoiceResult ret = SteamUser.DecompressVoice(destBuffer, bytesWritten, destBuffer2, (uint)destBuffer2.Length, out bytesWritten2, 44100);
        if (ret == EVoiceResult.k_EVoiceResultOK && bytesWritten2 > 0)
        {
            audioSource.clip = AudioClip.Create(UnityEngine.Random.Range(100, 1000000).ToString(), 44100, 1, 44100, false);

            float[] test = new float[44100];
            for (int i = 0; i < test.Length; i++)
            {
                test[i] = (short)(destBuffer2[i * 2] | destBuffer2[i * 2 + 1] << 8) / 32768.0f;
            }
            
            audioSource.clip.SetData(test, 0);
            audioSource.Play();
        }
    }
}
