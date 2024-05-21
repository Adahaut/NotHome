using Mirror;
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

    public void Hostlobby()
    {
        networkManager.StartHost();
        landingPagePanel.SetActive(false);
    }


}
