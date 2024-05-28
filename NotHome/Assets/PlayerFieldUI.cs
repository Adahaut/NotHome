using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFieldUI : NetworkBehaviour
{
    private GameObject canvas;

    private void OnEnable()
    {
        if(isOwned && !NewFieldManager.instance._panelOpen)
        {
            canvas = NewFieldManager.instance.fieldPlayerCanvas;
            canvas.SetActive(true);
            NewFieldManager.instance._panelOpen = true;
        }
    }

    public void DisablePanel()
    {
        NewFieldManager.instance.fieldPlayerCanvas = canvas;
        NewFieldManager.instance._panelOpen = false;
        GetComponentInParent<PlayerController>().DisablePlayer(false);
        canvas.SetActive(false);    
    }
}
