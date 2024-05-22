using Mirror;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class TalkieWalkieManager : NetworkBehaviour
{
    public AudioSource audioSource;
    public bool ownTalkieWalkie = false;

    private CircularBuffer circularBuffer;
    private float[] playbackBuffer;
    private int playbackOffset = 0;
    private const int sampleRate = 44100;
    private const int bufferSize = sampleRate * 2;

    private bool buttonPressed = false; 

    private void Start()
    {
        //Temp
        ownTalkieWalkie = true;

        if (isOwned) audioSource.volume = 0f;
        else audioSource.volume = 1f;

        circularBuffer = new CircularBuffer(bufferSize);

        //audioSource.clip = AudioClip.Create("VoiceChatBuffer", bufferSize, 1, sampleRate, true, OnAudioRead);
        audioSource.loop = true;
        audioSource.Play();
    }

    public void OnTalkieWalkieActive(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SteamUser.StartVoiceRecording();
            buttonPressed = true;
        }

        if(context.performed)
        {
            SteamUser.StopVoiceRecording();
            buttonPressed = false;  
        }
    }

    private void Update()
    {
        if(buttonPressed && isOwned)
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
        TalkieWalkieManager[] players = FindObjectsOfType<TalkieWalkieManager>();

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].ownTalkieWalkie)
                Target_PlaySound(players[i].GetComponent<NetworkIdentity>().connectionToClient, data, size);
        }
    }

    [TargetRpc(channel = 2)]
    void Target_PlaySound(NetworkConnection conn, byte[] destBuffer, uint bytesWritten)
    {
        byte[] destBuffer2 = new byte[sampleRate * 2];
        uint bytesWritten2;
        EVoiceResult ret = SteamUser.DecompressVoice(destBuffer, bytesWritten, destBuffer2, (uint)destBuffer2.Length, out bytesWritten2, sampleRate);
        if (ret == EVoiceResult.k_EVoiceResultOK && bytesWritten2 > 0)
        {

            float[] audioData = new float[bytesWritten2 / 2];
            for (int i = 0; i < audioData.Length; i++)
            {
                audioData[i] = (short)(destBuffer2[i * 2] | destBuffer2[i * 2 + 1] << 8) / 32768.0f;
            }

            lock (circularBuffer)
            {
                circularBuffer.Write(audioData);
            }
        }
    }

    private void OnAudioRead(float[] data)
    {
        lock (circularBuffer)
        {
            int samplesRead = circularBuffer.Read(data, data.Length);
            if (samplesRead < data.Length)
            {
                System.Array.Clear(data, samplesRead, data.Length - samplesRead);
            }
        }
    }


}
