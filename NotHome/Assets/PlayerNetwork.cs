using Mirror;
using System.Globalization;
using TMPro;
using UnityEditor;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    public TMP_Text _nameHoverHeadText;

    private string _displayName;

    public void SetDisplayName(string name)
    {
        _displayName = name;
        SetUIText();
    }

    public void SetUIText()
    {
        _nameHoverHeadText.text = _displayName;
    }

    private void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerNetwork>().SetUIText();
        }
    }

}
