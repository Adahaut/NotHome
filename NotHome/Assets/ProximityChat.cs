using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class ProximityChat : NetworkBehaviour
{
    // Distance maximale pour la communication vocale
    public float maxVoiceDistance = 10f;

    // Référence à la source audio du joueur
    AudioSource audioSource;

    // Initialisation
    void Start()
    {
        // Récupère la source audio attachée à ce GameObject
        audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {
        CmdSendVoiceChat();
    }

    // Commande pour envoyer le chat vocal aux joueurs à proximité
    [Command]
    void CmdSendVoiceChat()
    {
        // Récupère tous les joueurs sur le serveur
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // Parcourt tous les joueurs
        foreach (GameObject player in players)
        {
            // Vérifie si le joueur est à portée de voix
            if (Vector3.Distance(player.transform.position, transform.position) <= maxVoiceDistance)
            {
                // Obtient le composant NetworkIdentity du joueur
                NetworkIdentity playerIdentity = player.GetComponent<NetworkIdentity>();

                // Vérifie si le joueur possède un identifiant réseau
                if (playerIdentity != null)
                {
                    // Envoie le chat vocal au joueur à proximité via le client
                    RpcReceiveVoiceChat(playerIdentity.netId);
                }
            }
        }
    }

    // RPC pour recevoir le chat vocal sur le client
    [ClientRpc]
    void RpcReceiveVoiceChat(uint playerId)
    {
        // Vérifie si le client local est l'émetteur du chat vocal
        if (isLocalPlayer)
        {
            // Joue le chat vocal sur la source audio locale
            // Remplacez cette ligne avec le code pour jouer le chat vocal
            // depuis votre système de chat vocal en temps réel (par exemple, Photon Voice ou Unity's built-in Voice Chat)
            audioSource.Play();
        }
    }
}
