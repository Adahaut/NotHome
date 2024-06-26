using Mirror;
using Steamworks;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private GameObject nameTagPrefab;
    [SerializeField] private Vector3 nameTagOffset = new Vector3(0, 1.5f, 0);

    [HideInInspector] public GameObject nameTagInstance;
    private TMP_Text nameTagText;

    [SyncVar(hook = nameof(OnNameChanged))]
    private string _displayName;

    private static List<Camera> _playerCameras = new List<Camera>();

    public Camera mainCamera;

    [SerializeField] private GameObject playerUI;
    [SerializeField] private GameObject blackScreen;

    [SerializeField] private GameObject[] objectToDisable;

    private void Start()
    {
        if (isOwned)
        {
            CmdSetPlayerName(SteamFriends.GetPersonaName());
            SetActiveUI(false);
            foreach (GameObject obj in objectToDisable)
            {
                obj.SetActive(false);
            }
            if (mainCamera != null && !_playerCameras.Contains(mainCamera)) _playerCameras.Add(mainCamera);
        }

        nameTagInstance = Instantiate(nameTagPrefab, transform.position + nameTagOffset, Quaternion.identity, transform);
        nameTagText = nameTagInstance.GetComponentInChildren<TMP_Text>();
        GetComponent<PlayerDeathAndRespawn>().playerMesh.Add(nameTagInstance);

        if (isOwned) nameTagInstance.SetActive(false);
    }

    private void OnDestroy()
    {
        if (mainCamera != null && _playerCameras.Contains(mainCamera)) _playerCameras.Remove(mainCamera);
    }

    [Command]
    public void CmdSetPlayerName(string name)
    {
        _displayName = name;
    }

    public void SetActiveUI(bool active)
    {
        if (isOwned)
        {
            playerUI.SetActive(active);
            blackScreen.SetActive(!active);
            Invoke("DisableBlackScreen", 1f);
        }

    }

    void DisableBlackScreen()
    {
        blackScreen.SetActive(false);
    }


    private void OnNameChanged(string oldName, string newName)
    {
        if (nameTagText != null)
        {
            nameTagText.text = newName;
        }
    }

    private void Update()
    {
        if (!isOwned && nameTagInstance != null)
        {
            foreach (var playerCamera in _playerCameras)
            {
                if (playerCamera.transform.root.GetComponent<PlayerNetwork>().nameTagText.text == "-")
                {
                    playerCamera.transform.root.GetComponent<PlayerNetwork>().nameTagText.text = playerCamera.transform.root.GetComponent<PlayerNetwork>()._displayName;
                }

                nameTagInstance.transform.LookAt(playerCamera.transform);
                nameTagInstance.transform.Rotate(0, 180, 0);
                break;
            }
        }
    }

}
