using UnityEngine;
using Mirror;
using Steamworks;

public class ProximityVoiceChat : NetworkBehaviour
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
            if (ret == EVoiceResult.k_EVoiceResultOK && compressed > 512)
            {
                Debug.Log(compressed);
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
           float distance = Vector3.Distance(transform.position, players[i].gameObject.transform.position);
           float volume = Mathf.Clamp(1 - (distance / maxDistance), 0, 1);


            Target_PlaySound(players[i].GetComponent<NetworkIdentity>().connectionToClient, data, size, volume);
        }
    }



    [TargetRpc(channel = 2)]
    void Target_PlaySound(NetworkConnection conn, byte[] destBuffer, uint bytesWritten, float volume)
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

            if (!isOwned)
            {
                audioSource.volume = volume;
            }

            audioSource.clip.SetData(test, 0);
            audioSource.Play();
        }
    }
}
