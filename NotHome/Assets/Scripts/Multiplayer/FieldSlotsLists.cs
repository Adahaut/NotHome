using Mirror;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FieldSlotsLists : MonoBehaviour
{
    public List<Transform> _listSlots = new();
    //public List<TMP_Text> _listTexts = new();

    public TMP_Text debug;

    private void Update()
    {
        debug.text = "";
        for (int i = 0; i < NewFieldManager.instance._seedPlantedObjects.Count; i++)
        {
            NetworkServer.spawned.TryGetValue(NewFieldManager.instance._seedPlantedObjects[i], out NetworkIdentity identity);
            if (identity != null)
                debug.text += i + "  " + identity.gameObject.name + "\n";
            else
                debug.text += i + "\n";

        }
    }
}
