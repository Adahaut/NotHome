using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFieldUI : NetworkBehaviour
{
    private GameObject canvas;

    private void OnEnable()
    {
        if(isOwned)
        {
            canvas = Instantiate(NewFieldManager.instance.fieldPlayerCanvas, this.transform);
            canvas.SetActive(true);
        }
    }

    public void DisablePanel()
    {
        if(isOwned)
        {
            NewFieldManager.instance.fieldPlayerCanvas = canvas;
            Destroy(canvas);
            GetComponentInParent<PlayerController>().DisablePlayer(false);
        }
    }
}
