using Mirror;
using Steamworks;
using System.Globalization;
using TMPro;
using UnityEditor;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private GameObject nameTagPrefab;
    [SerializeField] private Vector3 nameTagOffset = new Vector3(0, 1.5f, 0);

    private GameObject nameTagInstance;
    private TMP_Text nameTagText;

    [SyncVar(hook = nameof(OnNameChanged))]
    private string _displayName;

    

    private void Start()
    {
        if(isOwned)
        {
            CmdSetPlayerName(SteamFriends.GetPersonaName());
        }

        nameTagInstance = Instantiate(nameTagPrefab, transform.position + nameTagOffset, Quaternion.identity, transform);
        nameTagText = nameTagInstance.GetComponentInChildren<TMP_Text>();

        if(isOwned) nameTagInstance.SetActive(false);
        else nameTagInstance.SetActive(true);
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
            nameTagInstance.transform.LookAt(Camera.main.transform);
            nameTagInstance.transform.Rotate(0, 180, 0);
        }
    }

}
