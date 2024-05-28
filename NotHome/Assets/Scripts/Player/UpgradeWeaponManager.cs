using System.Collections.Generic;
using UnityEngine;

public class UpgradeWeaponManager : MonoBehaviour
{
    private int _levelWeapon = 1;
    public List<DictionnaryElement<string, List<DictionnaryElement<string, int>>>> _upgarde;
    private InventoryManager _inventoryManager;
    private PlayerManager _playerManager;
    [SerializeField] private string _nameWeapon;

    private void Start()
    {
        _inventoryManager = PC.Instance.GetInventory();
        _playerManager = GetComponent<PlayerManager>();
    }
    public void SetEffectLevelWeapon()
    {
        switch (_nameWeapon)
        {
            case "Mele":
                switch (_levelWeapon)
                {
                    case 2:
                        Debug.Log("Degat = 20");
                        Debug.Log("Cadence = 1.5s");
                        break;
                    case 3:
                        Debug.Log("Degat = 25");
                        Debug.Log("Cadence = 2s");
                        break;
                    case 4:
                        Debug.Log("Degat = 35");
                        break;
                    default:
                        break;
                }
                break;
            case "Distance":
                RangeWeapon.Instance.NextWeapon();
                switch (_levelWeapon)
                {
                    case 3:
                        RangeWeapon.Instance.AciveRedDot();
                        break;
                    case 4:
                        RangeWeapon.Instance.ActiveLaser();
                        break;
                    default:
                        break;
                }
                break;
            default:
                Debug.Log("No nameWeapon");
                break;
        }
        
    }
    public void UpdateBuilding()
    {
        if (_upgarde.Count >= _levelWeapon)
        {
            int number = 0;
            List<int> listIndex = new();
            List<int> listMat = new();
            for (int i = 0; i < _inventoryManager._slotList.Count; i++)
            {
                if (_inventoryManager._slotList[i].GetComponent<InventorySlot>().ItemContained() != null)
                {
                    for (int j = 0; j < _upgarde[_levelWeapon - 1].Value.Count; j++)
                    {
                        if (_inventoryManager._slotList[i].GetComponent<InventorySlot>().ItemContained().ItemName() == _upgarde[_levelWeapon - 1].Value[j].Key)
                        {
                            number++;
                            if (_inventoryManager._slotList[i].GetComponent<InventorySlot>().Number() < _upgarde[_levelWeapon - 1].Value[j].Value)
                            {
                                Debug.Log("Pas assez de materiaux");
                                return;
                            }
                            else
                            {
                                listIndex.Add(i);
                                listMat.Add(_upgarde[_levelWeapon - 1].Value[j].Value);
                            }
                        }
                    }
                }
            }
            if (number == _upgarde[_levelWeapon - 1].Value.Count)
            {
                _levelWeapon++;
                SetEffectLevelWeapon();
                for (int i = 0; i < listIndex.Count; i++)
                {
                    _inventoryManager._slotList[listIndex[i]].GetComponent<InventorySlot>().
                        SetNumber(_inventoryManager._slotList[listIndex[i]].GetComponent<InventorySlot>().Number() - listMat[i]);
                }
            }
            else
            {
                Debug.Log("Pas assez de materiaux");
            }
        }
        else
        {
            Debug.Log("Level max");
        }
    }
}
