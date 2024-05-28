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
        UpdateFieldUI();
    }

    public void UpdateFieldUI()
    {
        if (NewFieldManager.instance == null) return;

        List<Transform> slots = _player.GetComponentInChildren<PlayerFieldSlot>()._listSlots;
        List<TMP_Text> textSlots = _player.GetComponentInChildren<PlayerFieldSlot>()._listTexts;

        for (int i = 0; i < NewFieldManager.instance._allPlants.Count; i++)
        {
            int plantId = NewFieldManager.instance._allPlants[i];
            if (plantId >= 0 && plantId < NewFieldManager.instance._seedPrefabs.Count && i < slots.Count)
            {
                Seed plant = NewFieldManager.instance._seedPrefabs[plantId];
                slots[i].GetComponent<Image>().sprite = plant._img;
                textSlots[i].text = plant._name;
                plant.transform.position = slots[i].position;
            }
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
