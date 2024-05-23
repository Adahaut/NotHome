using Mirror;
using Steamworks;
using System.Linq;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using Unity.VisualScripting;
using UnityEditor.DeviceSimulation;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private GameObject nameTagPrefab;
    [SerializeField] private Vector3 nameTagOffset = new Vector3(0, 1.5f, 0);

    [HideInInspector] public GameObject nameTagInstance;
    private TMP_Text nameTagText;

    [SyncVar(hook = nameof(OnNameChanged))]
    private string _displayName;

    private static List<Camera> _playerCameras = new List<Camera>();

    RenderTexture _renderTexture;

    private void Start()
    {
        if(isOwned)
        {
            CmdSetPlayerName(SteamFriends.GetPersonaName());
        }

        Camera playerCamera = GetComponentInChildren<Camera>();
        Camera displayCamera = this.AddComponent<Camera>();
        displayCamera = playerCamera;
        displayCamera.targetTexture = _renderTexture;
        if(playerCamera != null && !_playerCameras.Contains(playerCamera)) _playerCameras.Add(playerCamera);

        nameTagInstance = Instantiate(nameTagPrefab, transform.position + nameTagOffset, Quaternion.identity, transform);
        nameTagText = nameTagInstance.GetComponentInChildren<TMP_Text>();

        if(isOwned) nameTagInstance.SetActive(false);
        else nameTagInstance.SetActive(true);
    }

    private void OnDestroy()
    {
        Camera playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera != null && _playerCameras.Contains(playerCamera)) _playerCameras.Remove(playerCamera);
    }

    public void SetRenderTexture(RenderTexture text)
    {
        _renderTexture = text;
    }

    [Command]
    public void CmdSetPlayerName(string name)
    {
        _displayName = name;
    }

    private void OnNameChanged(string oldName, string newName)
    {
        if(nameTagText != null)
        {
            nameTagText.text = newName;
        }
    }

    private void Update()
    {
        if (nameTagInstance != null)
        {
            
            foreach (var playerCamera in _playerCameras)
            {
                if (playerCamera != null && playerCamera.gameObject != this.gameObject)
                {
                    nameTagInstance.transform.LookAt(playerCamera.transform);
                    nameTagInstance.transform.Rotate(0, 180, 0);
                    break;
                }
            }
        }
    }

}
