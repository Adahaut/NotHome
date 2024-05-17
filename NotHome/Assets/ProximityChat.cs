using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class ProximityChat : NetworkBehaviour
{
    public float maxVoiceDistance = 10f; // Distance maximale pour la communication vocale
    public List<AudioClip> audioClips; // Liste des clips audio disponibles

    private AudioSource audioSource;
    private AudioClip recordedClip;
    private int microphoneDeviceIndex = 0; // Index du p�riph�rique de microphone � utiliser

    void Start()
    {
        // Cr�ez une source audio pour jouer l'audio captur�
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;

        // Commencez � capturer l'audio du microphone au d�marrage
        StartMicrophoneCapture();
    }

    void Update()
    {
        // Si ce joueur est l'�metteur du son
        if (isLocalPlayer)
        {
            // Arr�tez l'enregistrement et r�cup�rez l'audio captur�
            StopMicrophoneCapture();

            // Choisissez un clip audio al�atoire � jouer
            AudioClip clipToPlay = audioClips[Random.Range(0, audioClips.Count)];

            // Envoyez l'identifiant du clip audio aux joueurs � proximit�
            int clipIndex = audioClips.IndexOf(clipToPlay);
            CmdSendVoiceChat(clipIndex);

            // Red�marrez l'enregistrement pour capturer de nouveaux �chantillons audio
            StartMicrophoneCapture();
        }
    }

    void StartMicrophoneCapture()
    {
        // Commencez � capturer l'audio du microphone
        string device = Microphone.devices[microphoneDeviceIndex];
        recordedClip = Microphone.Start(device, true, 10, AudioSettings.outputSampleRate);
    }

    void StopMicrophoneCapture()
    {
        // Arr�tez l'enregistrement et r�cup�rez l'audio captur�
        Microphone.End(null);
    }

    [Command]
    void CmdSendVoiceChat(int clipIndex)
    {
        // Envoyez l'identifiant du clip audio � tous les clients
        RpcReceiveVoiceChat(clipIndex);
    }

    [ClientRpc]
    void RpcReceiveVoiceChat(int clipIndex)
    {
        // Jouez le clip audio sur tous les clients
        if (clipIndex >= 0 && clipIndex < audioClips.Count)
        {
            audioSource.clip = audioClips[clipIndex];
            audioSource.Play();
        }
    }
}
