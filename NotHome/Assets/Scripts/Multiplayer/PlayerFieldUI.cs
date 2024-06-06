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

            uint id = NewFieldManager.instance._seedPlantedObjects[i];
            NetworkIdentity networkIdentity = null;
            if (id != 0)
                networkIdentity = NetworkClient.spawned[id];

            if (networkIdentity != null)
            {
                SeedObject obj = networkIdentity.gameObject.GetComponent<SeedObject>();
                slot.seedImage.sprite = obj.seedImage;
                slot.fruitImage.sprite = obj.fruitImage;
                slot.seedNameTextUI.text = obj.seedStruct._name;
                slot.fillBar.fillAmount = obj._currentTimer / obj._timeToGrow;
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
