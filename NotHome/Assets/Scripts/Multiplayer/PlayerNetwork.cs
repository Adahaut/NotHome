using Mirror;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    private BuildingManager buildingManager;

    public TMP_Text debugText;

    private void Start()
    {
        StartCoroutine(FindBuildingManager());
        if(isOwned)
        {
            CmdSetPlayerName(SteamFriends.GetPersonaName());
            playerUI.SetActive(true);
            if (mainCamera != null && !_playerCameras.Contains(mainCamera)) _playerCameras.Add(mainCamera);
            //buildingManager = BuildingManager.instance;
            //CmdRequestAuthority();
        }

        nameTagInstance = Instantiate(nameTagPrefab, transform.position + nameTagOffset, Quaternion.identity, transform);
        nameTagText = nameTagInstance.GetComponentInChildren<TMP_Text>();


        if(isOwned) nameTagInstance.SetActive(false);
        else nameTagInstance.SetActive(true);
    }

    private IEnumerator FindBuildingManager()
    {
        while (BuildingManager.instance == null)
        {
            yield return null;
        }

        buildingManager = BuildingManager.instance;
        if(isOwned)
        {
            CmdRequestAuthority();
        }
    }

    [Command]
    public void CmdRequestAuthority()
    {
        buildingManager.AssignAuthority(connectionToClient);
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

    private void OnNameChanged(string oldName, string newName)
    {
        if(nameTagText != null)
        {
            nameTagText.text = newName;
        }
    }

    private void Update()
    {
        debugText.text = "";
        for (int i = 0; i < NewFieldManager.instance._allPlants.Count; i++)
        {
            debugText.text += i + " " + NewFieldManager.instance._allPlants[i]._name + "\n";
        }




        if (nameTagInstance != null && !isOwned)
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
