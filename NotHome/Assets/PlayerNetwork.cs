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

    private void Start()
    {
        if(!isOwned)
        {
            nameHoverHeadText.text = _displayName;
        }
    }
}
