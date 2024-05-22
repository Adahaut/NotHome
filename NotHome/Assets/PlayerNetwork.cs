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

    public void SetDisplayName(string name)
    {
        _displayName = name;
    }
    

    public override void OnStartAuthority()
    {
        nameHoverHeadText.text = _displayName;

        foreach(var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            player.GetComponent<PlayerNetwork>().nameHoverHeadText.text = player.GetComponent<PlayerNetwork>()._displayName;
        }
    }
}
