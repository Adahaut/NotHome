using Mirror;
using Steamworks;
using TMPro;
using UnityEngine;

public class ProximityChat : NetworkBehaviour
{
    #region Test
    //private AudioSource _audioSource;
    //[HideInInspector] public AudioClip _microphoneClip;
    //private string _microphoneName;

    //private GameObject[] _player;


    //public TMP_Text text;
    //int count = 0;

    //private void Awake()
    //{
    //    _audioSource = GetComponent<AudioSource>();

    //    //Get default microphone
    //    if (Microphone.devices.Length > 0)
    //    {
    //        _microphoneName = Microphone.devices[0];
    //        Debug.Log("Using microphone: " + _microphoneName);
    //    }
    //    else
    //    {
    //        Debug.LogError("No microphone found!");
    //        return;
    //    }

    //    //Set the clip of the AudioSource of the other players
    //    _microphoneClip = Microphone.Start(_microphoneName, true, 10, 44100);
    //    _audioSource.mute = true;
    //    _audioSource.loop = true;
    //    _audioSource.clip = _microphoneClip;

    //    while (!(Microphone.GetPosition(_microphoneName) > 0)) { }

    //    _audioSource.Play();
    //}

    //public override void OnStartAuthority()
    //{
    //    StartCoroutine(GetPlayersAndAssignMics());
    //}

    //IEnumerator GetPlayersAndAssignMics()
    //{
    //    yield return new WaitForSeconds(1.0f);

    //    _player = GameObject.FindGameObjectsWithTag("Player");

    //    foreach (var player in _player)
    //    {
    //        if (player != this.gameObject)
    //        {
    //            GameObject playerAudio = new GameObject(player.name + " Audio");
    //            playerAudio.transform.parent = this.transform;
    //            AudioSource audioSource = playerAudio.AddComponent<AudioSource>();
    //            audioSource.playOnAwake = false;
    //            audioSource.clip = player.GetComponent<ProximityChat>()._microphoneClip;
    //            audioSource.loop = true;
    //            audioSource.Play();

    //            count++;
    //        }

    //        text.text = count.ToString();
    //    }

    //}

    //void OnApplicationQuit()
    //{
    //    if (Microphone.IsRecording(_microphoneName))
    //    {
    //        Microphone.End(_microphoneName);
    //    }
    //}

    #endregion

    #region TestWithSTeamMic
    //public float maxDistance = 10f;
    //private AudioSource audioSource;
    //private List<AudioSource> remoteAudioSources = new List<AudioSource>();
    //private Dictionary<CSteamID, AudioSource> playerAudioSources = new Dictionary<CSteamID, AudioSource>();
    //private byte[] audioDataBuffer = new byte[4096];

    //void Start()
    //{

    //    audioSource = GetComponent<AudioSource>();
    //    audioSource.clip = Microphone.Start(null, true, 1, 44100);
    //    audioSource.loop = true;
    //    audioSource.mute = true;
    //    while (!(Microphone.GetPosition(null) > 0)) { }
    //    audioSource.Play();
    //}

    //void Update()
    //{
    //    CaptureAndSendAudio();
    //    ReceiveAndPlayAudio();
    //    UpdateAudioVolumes();
    //}

    //void CaptureAndSendAudio()
    //{
    //    float[] samples = new float[audioSource.clip.samples * audioSource.clip.channels];
    //    audioSource.clip.GetData(samples, 0);
    //    System.Buffer.BlockCopy(samples, 0, audioDataBuffer, 0, audioDataBuffer.Length);
    //    foreach (var player in playerAudioSources.Keys)
    //    {
    //        SteamNetworking.SendP2PPacket(player, audioDataBuffer, (uint)audioDataBuffer.Length, EP2PSend.k_EP2PSendUnreliable);
    //    }
    //}

    //void ReceiveAndPlayAudio()
    //{
    //    uint msgSize;
    //    while (SteamNetworking.IsP2PPacketAvailable(out msgSize))
    //    {
    //        byte[] buffer = new byte[msgSize];
    //        uint bytesRead;
    //        CSteamID remoteId;
    //        if (SteamNetworking.ReadP2PPacket(buffer, msgSize, out bytesRead, out remoteId))
    //        {
    //            if (!playerAudioSources.ContainsKey(remoteId))
    //            {
    //                AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
    //                newAudioSource.spatialBlend = 1.0f; // 3D sound
    //                newAudioSource.loop = true;
    //                playerAudioSources[remoteId] = newAudioSource;
    //                remoteAudioSources.Add(newAudioSource);
    //            }
    //            AudioSource remoteAudioSource = playerAudioSources[remoteId];
    //            float[] samples = new float[buffer.Length / sizeof(float)];
    //            System.Buffer.BlockCopy(buffer, 0, samples, 0, buffer.Length);
    //            remoteAudioSource.clip = AudioClip.Create("remoteClip", samples.Length, 1, 44100, false);
    //            remoteAudioSource.clip.SetData(samples, 0);
    //            if (!remoteAudioSource.isPlaying)
    //            {
    //                remoteAudioSource.Play();
    //            }
    //        }
    //    }
    //}

    //void UpdateAudioVolumes()
    //{
    //    foreach (var player in playerAudioSources)
    //    {
    //        CSteamID playerId = player.Key;
    //        AudioSource source = player.Value;
    //        Vector3 playerPosition = GetPlayerPosition(playerId);
    //        float distance = Vector3.Distance(transform.position, playerPosition);
    //        //source.volume = Mathf.Clamp(1 - (distance / maxDistance), 0, 1);
    //        source.volume = 1f;
    //    }
    //}

    //Vector3 GetPlayerPosition(CSteamID playerId)
    //{
    //    // Implémentez cette fonction pour retourner la position actuelle du joueur avec l'ID playerId
    //    // Cela pourrait impliquer de trouver le GameObject associé à ce joueur et de retourner sa position.
    //    // Par exemple:
    //    // return playerGameObjects[playerId].transform.position;
    //    return Vector3.zero;
    //}
    #endregion
}
