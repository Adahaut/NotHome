using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class ProximityChat : NetworkBehaviour
{
    // Distance maximale pour la communication vocale
    public float maxVoiceDistance = 10f;

    // R�f�rence � la source audio du joueur
    AudioSource audioSource;

    // Initialisation
    void Start()
    {
        // R�cup�re la source audio attach�e � ce GameObject
        audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {
        CmdSendVoiceChat();
    }

    // Commande pour envoyer le chat vocal aux joueurs � proximit�
    [Command]
    void CmdSendVoiceChat()
    {
        // R�cup�re tous les joueurs sur le serveur
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // Parcourt tous les joueurs
        foreach (GameObject player in players)
        {
            // V�rifie si le joueur est � port�e de voix
            if (Vector3.Distance(player.transform.position, transform.position) <= maxVoiceDistance)
            {
                // Obtient le composant NetworkIdentity du joueur
                NetworkIdentity playerIdentity = player.GetComponent<NetworkIdentity>();

                // V�rifie si le joueur poss�de un identifiant r�seau
                if (playerIdentity != null)
                {
                    // Envoie le chat vocal au joueur � proximit� via le client
                    RpcReceiveVoiceChat(playerIdentity.netId);
                }
            }
        }
    }

    // RPC pour recevoir le chat vocal sur le client
    [ClientRpc]
    void RpcReceiveVoiceChat(uint playerId)
    {
        // V�rifie si le client local est l'�metteur du chat vocal
        if (isLocalPlayer)
        {
            // Joue le chat vocal sur la source audio locale
            // Remplacez cette ligne avec le code pour jouer le chat vocal
            // depuis votre syst�me de chat vocal en temps r�el (par exemple, Photon Voice ou Unity's built-in Voice Chat)
            audioSource.Play();
        }
    }
}
