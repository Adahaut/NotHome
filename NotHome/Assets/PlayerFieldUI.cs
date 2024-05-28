using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFieldUI : NetworkBehaviour
{
    private GameObject canvas;
    private GameObject newCanvas;
    private void OnEnable()
    {
        if(isOwned && !NewFieldManager.instance._panelOpen)
        {
            canvas = NewFieldManager.instance.fieldPlayerCanvas;
            newCanvas = Instantiate(canvas, this.transform);
            newCanvas.SetActive(true);
            //canvas.SetActive(true);
            NewFieldManager.instance._panelOpen = true;
        }
    }

    public void DisablePanel()
    {
        GameObject oldCanvas = Instantiate(newCanvas, canvas.transform.parent);
        oldCanvas.SetActive(false);
        Destroy(canvas);
        NewFieldManager.instance.fieldPlayerCanvas = oldCanvas;
        NewFieldManager.instance._panelOpen = false;
        GetComponentInParent<PlayerController>().DisablePlayer(false);
        newCanvas.SetActive(false);
        Destroy(newCanvas);

    }
}
