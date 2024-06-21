using Mirror;
using Steamworks;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RepaireBridge : NetworkBehaviour
{
    [SerializeField] private GameObject _bridge;

    [SerializeField] private List<string> _itemsNeeded = new List<string>();
    [SerializeField] private List<int> _itemsNumberNeeded = new List<int>();

    [SerializeField] private TextMeshProUGUI _ressourcesNeeded;
    [SerializeField] private TextMeshProUGUI _message;

    [SerializeField] private UpgradeHomeManager _hq;
    [SerializeField] private UpgradeHomeManager _ct;

    [SerializeField] private InventoryManager _playerInventory;

    private void OnEnable()
    {
        if(_bridge == null)
        {
            _bridge = GameObject.Find("Bridge");
        }


        if(_ressourcesNeeded.text == null)
        {
            for (int i = 0; i < _itemsNeeded.Count; i++)
            {
                _ressourcesNeeded.text = _ressourcesNeeded.text + _itemsNeeded[i] + "\n";
            }
        }
    }

    private bool CanUpgrade()
    {
        print("HQ " + _hq.GetLevel() + " CT " + _ct.GetLevel());
        return _hq.GetLevel() == 4 && _ct.GetLevel() == 3;
    }

    public void CreateBridge()
    {
        _message.text = "";
        //if (!CanUpgrade())
        //{
        //    _message.text = "You must have upgrade all your base to repair the bridge";
        //    return;
        //}
        //for (int i = 0; i < _itemsNeeded.Count; i++)
        //{
        //    if(_playerInventory.ContainItem(_itemsNeeded[i]))
        //    {
        //        if (!(_playerInventory._slotList[_playerInventory.GetIndexOfSlotByName(_itemsNeeded[i])].GetComponent<InventorySlot>().Number() == _itemsNumberNeeded[i]))
        //        {
        //            if (_message.text == "")
        //                _message.text += "missing ressources : ";

        //            int _missingQuantity = _itemsNumberNeeded[i] - _playerInventory._slotList[_playerInventory.GetIndexOfSlotByName(_itemsNeeded[i])].GetComponent<InventorySlot>().Number();
        //            _message.text += _itemsNeeded[i] + " X " + _missingQuantity + "\n";
        //        }
        //    }
        //    else
        //    {
        //        if (_message.text == "")
        //            _message.text += "missing ressources : ";
        //        _message.text += _itemsNeeded[i] + "\n";
        //    }
        //}
        if(_message.text == "" && isOwned)
        {
            CmdRepairBridgeVisual();
            _message.text = "Bridge Reparation Done!";
        }
    }

    [Command]
    private void CmdRepairBridgeVisual()
    {
        RpcRepairBridgeVisual();
    }

    [ClientRpc]
    private void RpcRepairBridgeVisual()
    {
        if(_bridge == null)
            _bridge = GameObject.Find("Bridge");
        _bridge.GetComponent<BoxCollider>().enabled = true;
        _bridge.GetComponent<MeshRenderer>().enabled = true;
    }

}
