using Steamworks;
using System.Collections.Generic;
using UnityEngine;

public class MapUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> _mapsList = new List<GameObject>();
    [SerializeField] private List<ItemObject> _usbKeysList = new List<ItemObject>();
    [SerializeField] private List<GameObject> _Buttons = new List<GameObject>();

    private InventoryManager _playerInventory;

    private void Awake()
    {
        _playerInventory = GetComponentInParent<PC>()._inventory.GetComponent<InventoryManager>();
    }

    private void OnEnable()
    {
        DesactiveAllMaps();
        SetActiveButtons();
    }

    private void SetActiveButtons()
    {
        for(int i = 0; i < _Buttons.Count; i++)
        {
            print(CheckForValidKey(_usbKeysList[i].ItemName()));
            _Buttons[i].SetActive(CheckForValidKey(_usbKeysList[i].ItemName()));
        }
    }

    private void DesactiveAllMaps()
    {
        for (int i = 0; i < _mapsList.Count; i++)
        {
            _mapsList[i].SetActive(false);
        }
    }

    private bool CheckForValidKey(string _keyName)
    {
        for(int i = 0; i < _playerInventory._slotList.Count; i++)
        {
            if(_playerInventory._slotList[i].GetComponent<InventorySlot>().ItemContained().ItemName() == _keyName)
            {
                return true;
            }
        }
        return false;
    }

    public void OpenMapByIndex(int _index)
    {
        DesactiveAllMaps();
        _mapsList[_index].SetActive(true);
    }

}
