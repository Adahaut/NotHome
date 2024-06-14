using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RepaireBridge : MonoBehaviour
{
    [SerializeField] private GameObject _bridge;

    [SerializeField] private List<string> _itemsNeeded = new List<string>();
    [SerializeField] private List<int> _itemsNumberNeeded = new List<int>();

    [SerializeField] private TextMeshProUGUI _ressourcesNeeded;
    [SerializeField] private TextMeshProUGUI _message;

    private InventoryManager _playerInventory;

    private void OnEnable()
    {
        if (_playerInventory == null)
            _playerInventory = transform.parent.GetComponentInChildren<InventoryManager>();

        for (int i = 0; i < _itemsNeeded.Count; i++)
        {
            _ressourcesNeeded.text = _ressourcesNeeded.text + _itemsNeeded[i] + "\n";
        }
    }

    public void CreateBridge()
    {
        _message.text = "";
        for (int i = 0; i < _itemsNeeded.Count; i++)
        {
            if(_playerInventory.ContainItem(_itemsNeeded[i]))
            {
                if (!(_playerInventory._slotList[_playerInventory.GetIndexOfSlotByName(_itemsNeeded[i])].GetComponent<InventorySlot>().Number() == _itemsNumberNeeded[i]))
                {
                    if (_message.text == "")
                        _message.text += "missing ressources : ";

                    int _missingQuantity = _itemsNumberNeeded[i] - _playerInventory._slotList[_playerInventory.GetIndexOfSlotByName(_itemsNeeded[i])].GetComponent<InventorySlot>().Number();
                    _message.text += _itemsNeeded[i] + " X " + _missingQuantity + "\n";
                }
            }
            else
            {
                if (_message.text == "")
                    _message.text += "missing ressources : ";
                _message.text += _itemsNeeded[i] + "\n";
            }
        }
        if(_message.text == "")
        {
            _bridge.SetActive(true);
            _message.text = "Bridge Reparation Done!";
        }
    }

}
