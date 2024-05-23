using Mirror;
using Steamworks;
using System.Linq;
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
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != this)
                {
                    players[i].GetComponent<PlayerNetwork>().nameTagInstance.transform.LookAt(this.transform);
                    players[i].GetComponent<PlayerNetwork>().nameTagInstance.transform.Rotate(0, 180, 0);
                }
            }
        }
    }

}
