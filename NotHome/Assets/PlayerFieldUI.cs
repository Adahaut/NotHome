using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFieldUI : NetworkBehaviour
{
    [SerializeField] private GameObject _player;

    private void OnEnable()
    {

        foreach (var plant in NewFieldManager.instance._allPlants)
        {
            List<Transform> slots = _player.GetComponentInChildren<PlayerFieldSlot>()._listSlots;
            plant.gameObject.transform.position = slots[plant._index].transform.position;
        }
    }

    //private void OnEnable()
    //{
    //    if(isOwned && !NewFieldManager.instance._panelOpen)
    //    {
    //        canvas = NewFieldManager.instance.fieldPlayerCanvas;
    //        newCanvas = Instantiate(canvas, this.transform);
    //        newCanvas.SetActive(true);
    //        //canvas.SetActive(true);
    //        NewFieldManager.instance._panelOpen = true;
    //    }
    //}

    public void OnDisable()
    {
        GetComponentInParent<PlayerController>().DisablePlayer(false);

    }

    public void DisableFieldPanel()
    {
        gameObject.SetActive(false);
    }
}
