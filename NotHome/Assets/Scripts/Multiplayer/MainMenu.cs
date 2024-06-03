using Mirror;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private NetworkLobbyManager networkManager = null;

    [Header("UI")]
    public GameObject landingPagePanel = null;

    private void Start()
    {
        Screen.fullScreen = true;
    }

    public void Hostlobby()
    {
        networkManager.StartHost();
        landingPagePanel.SetActive(false);
    }

    public void OpenSteamOverlay()
    {
        if (SteamManager.Initialized)
        {
            SteamFriends.ActivateGameOverlay("Friends");
        }
    }

}
