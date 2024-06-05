using Mirror;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerFieldUI : NetworkBehaviour
{
    [SerializeField] private FieldSlotsLists _playerSlots;

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
        List<TMP_Text> textSlots = _playerSlots._listTexts;

        for (int i = 0; i < slots.Count; i++)
        {
            Seed plant = NewFieldManager.instance._allPlants[i];

            if (NewFieldManager.instance._seedPlantedObjects[i] != null)
                slots[i].GetComponent<PlayerFieldSlot>().fillBar.fillAmount =
                    NewFieldManager.instance._seedPlantedObjects[i]._currentTimer / NewFieldManager.instance._seedPlantedObjects[i]._timeToGrow;

            //slots[i].GetComponent<Image>().sprite = plant._img;
            textSlots[i].text = plant._name;
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
