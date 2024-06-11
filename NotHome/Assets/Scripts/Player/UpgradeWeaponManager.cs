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
                        PlayerAttack.Instance.UpgradeMachetteVisual(0);
                        PlayerAttack.Instance.SetAttack(20);
                        PlayerAttack.Instance.SetCadence(1.5f);
                        break;
                    case 3:
                        PlayerAttack.Instance.UpgradeMachetteVisual(1);
                        PlayerAttack.Instance.SetAttack(25);
                        PlayerAttack.Instance.SetCadence(1f);
                        break;
                    case 4:
                        PlayerAttack.Instance.UpgradeMachetteVisual(2);
                        PlayerAttack.Instance.SetAttack(35);
                        PlayerAttack.Instance.SetCadence(0.5f);
                        break;
                    default:
                        break;
                }
                break;
            case "Distance":
                RangeWeapon.Instance.NextWeapon(); 
                RangeWeapon.Instance._weaponLevel++;
                switch (_levelWeapon)
                {
                    case 2:
                        RangeWeapon.Instance.UpgradeWeaponVisual(RangeWeapon.Instance._level2Weapon);
                        break;
                    case 3:
                        RangeWeapon.Instance.UpgradeWeaponVisual(RangeWeapon.Instance._level3Weapon);
                        RangeWeapon.Instance.AciveRedDot();
                        break;
                    case 4:
                        RangeWeapon.Instance.UpgradeWeaponVisual(RangeWeapon.Instance._level4Weapon);
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
