using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class ProximityChat : NetworkBehaviour
{
    public float maxVoiceDistance = 10f; // Distance maximale pour la communication vocale
    public List<AudioClip> audioClips; // Liste des clips audio disponibles

    private AudioSource audioSource;
    private AudioClip recordedClip;
    private int microphoneDeviceIndex = 0; // Index du périphérique de microphone à utiliser

    void Start()
    {
        // Créez une source audio pour jouer l'audio capturé
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;

        // Commencez à capturer l'audio du microphone au démarrage
        StartMicrophoneCapture();
    }

    void Update()
    {
        // Si ce joueur est l'émetteur du son
        if (isLocalPlayer)
        {
            // Arrêtez l'enregistrement et récupérez l'audio capturé
            StopMicrophoneCapture();

            // Choisissez un clip audio aléatoire à jouer
            AudioClip clipToPlay = audioClips[Random.Range(0, audioClips.Count)];

            // Envoyez l'identifiant du clip audio aux joueurs à proximité
            int clipIndex = audioClips.IndexOf(clipToPlay);
            CmdSendVoiceChat(clipIndex);

            // Redémarrez l'enregistrement pour capturer de nouveaux échantillons audio
            StartMicrophoneCapture();
        }
    }

    void StartMicrophoneCapture()
    {
        // Commencez à capturer l'audio du microphone
        string device = Microphone.devices[microphoneDeviceIndex];
        recordedClip = Microphone.Start(device, true, 10, AudioSettings.outputSampleRate);
    }

    void StopMicrophoneCapture()
    {
        // Arrêtez l'enregistrement et récupérez l'audio capturé
        Microphone.End(null);
    }

    [Command]
    void CmdSendVoiceChat(int clipIndex)
    {
        // Envoyez l'identifiant du clip audio à tous les clients
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
