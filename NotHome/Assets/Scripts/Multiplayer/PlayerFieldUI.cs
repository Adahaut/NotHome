using Mirror;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerFieldUI : NetworkBehaviour
{
    [SerializeField] private FieldSlotsLists _playerSlots;

    NewFieldManager fieldManager = NewFieldManager.instance;

    private void Update()
    {
        if (isOwned)
        {
            UpdateUI();
        }

        

    }

    public void UpdateUI()
    {
        List<Transform> slots = _playerSlots._listSlots;
        //List<TMP_Text> textSlots = _playerSlots._listTexts;

        for (int i = 0; i < slots.Count; i++)
        {
            Seed plant = NewFieldManager.instance._allPlants[i];
            PlayerFieldSlot slot = slots[i].GetComponent<PlayerFieldSlot>();
            

            if (NewFieldManager.instance._seedPlantedObjects[i] != null)
            {
                slot.seedImage.sprite = fieldManager._seedPlantedObjects[i].seedImage;
                slot.fruitImage.sprite = fieldManager._seedPlantedObjects[i].fruitImage;
                slot.seedNameTextUI.text = fieldManager._seedPlantedObjects[i].seedStruct._name;
                slot.fillBar.fillAmount = fieldManager._seedPlantedObjects[i]._currentTimer / fieldManager._seedPlantedObjects[i]._timeToGrow;
            }
            else
            {
                slot.seedImage.sprite = null;
                slot.fruitImage.sprite = null;
                slot.seedNameTextUI.text = "Empty";
                slot.fillBar.fillAmount = 0.05f;
            }

            //textSlots[i].text = plant._name;
        }
    }

    public void DisableFieldUI()
    {
        this.gameObject.SetActive(false);
    }

    public static void UpdateAllUIs()
    {
        foreach (var playerFieldUI in FindObjectsOfType<PlayerFieldUI>())
        {
            playerFieldUI.UpdateUI();
        }
    }

    public void OnDisable()
    {
        GetComponentInParent<PlayerController>().DisablePlayer(false);
    }
}
