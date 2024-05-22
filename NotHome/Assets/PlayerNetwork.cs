using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    public TMP_Text nameHoverHeadText;

    private string _displayName;
    private NetworkConnection conn;

    private List<GameObject> _allNames;

    public void SetDisplayName(string name)
    {
        _displayName = name;
    }

    public void SetConnection(NetworkConnection conn)
    {
        this.conn = conn;
    }

    public override void OnStartAuthority()
    {
        nameHoverHeadText.text = _displayName;
        StartCoroutine(GetAllNames());
        
    }

    IEnumerator GetAllNames()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            _allNames.Add(player.GetComponent<PlayerNetwork>().nameHoverHeadText.gameObject);
        }
    }

    private void Update()
    {
        foreach(var name in _allNames)
        {
            name.transform.LookAt(this.transform.position);
        }
    }
}
