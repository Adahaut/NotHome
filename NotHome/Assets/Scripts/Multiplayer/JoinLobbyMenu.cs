using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField] private NetworkLobbyManager networkManager = null;

    [Header("UI")]
    [SerializeField] private GameObject landingPagePanel = null;
    [SerializeField] private TMP_InputField ipAdressInputField = null;
    [SerializeField] private Button joinButton = null;

    private void OnEnable()
    {
        NetworkLobbyManager.OnClientConnected += HandleClientConnected;
        NetworkLobbyManager.OnClientDisconnected += HandleClientDisconnected;
    }

    private void OnDisable()
    {
        NetworkLobbyManager.OnClientConnected -= HandleClientConnected;
        NetworkLobbyManager.OnClientDisconnected -= HandleClientDisconnected;
    }

    public void JoinLobby()
    {
        

        string ipAdress = ipAdressInputField.text;

        networkManager.networkAddress = ipAdress;
        networkManager.StartClient();

        joinButton.interactable = false;

    }

    private void HandleClientConnected()
    {
        joinButton.interactable = true;

        gameObject.SetActive(false);
        landingPagePanel.SetActive(false);
    }

    public void HandleClientDisconnected()
    {
        joinButton.interactable = true;
        
    }
}
