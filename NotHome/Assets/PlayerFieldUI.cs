using Mirror;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFieldUI : NetworkBehaviour
{
    [SerializeField] private PlayerFieldSlot _playerSlots;

    

    private void OnEnable()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        List<Transform> slots = _playerSlots._listSlots;
        List<TMP_Text> textSlots = _playerSlots._listTexts;

        for (int i = 0; i < slots.Count; i++)
        {
            Seed plant = NewFieldManager.instance._allPlants[i];
            slots[i].GetComponent<Image>().sprite = plant._img;
            textSlots[i].text = plant._name;
        }
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

    public void DisableFieldPanel()
    {
        gameObject.SetActive(false);
    }
}
