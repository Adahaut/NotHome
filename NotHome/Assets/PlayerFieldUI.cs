using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFieldUI : NetworkBehaviour
{
    [SerializeField] private GameObject _player;

    private void OnEnable()
    {

        foreach (var plant in NewFieldManager.instance._allPlants)
        {
            List<Transform> slots = _player.GetComponentInChildren<PlayerFieldSlot>()._listSlots;
            List<TMP_Text> textSlots = _player.GetComponentInChildren<PlayerFieldSlot>()._listTexts;
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].GetComponent<Image>().sprite = plant._img;
                textSlots[i].text = plant._name;
            }
            //plant.gameObject.transform.position = slots[plant._index].transform.position;
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
