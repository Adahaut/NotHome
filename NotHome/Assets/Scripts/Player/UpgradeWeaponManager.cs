using Mirror;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeWeaponManager : NetworkBehaviour
{
    private int _levelWeapon = 1;
    public List<DictionnaryElement<string, List<DictionnaryElement<string, int>>>> _upgarde;
    [SerializeField] private InventoryManager _inventoryManager;
    private PlayerManager _playerManager;
    [SerializeField] private RangeWeapon _rangeWeapon;
    [SerializeField] private PlayerAttack _playerAttack;
    [SerializeField] private string _nameWeapon;
    [SerializeField] private TextMeshProUGUI _ressourcesNeeded;
    [SerializeField] private TextMeshProUGUI _textLevel;
    [SerializeField] private GameObject _gunLevel3;
    [SerializeField] private SwordVisual _swordVisual;
    private void OnEnable()
    {
        UpdateRessourcesText();
    }

    private void Start()
    {
        _playerManager = GetComponent<PlayerManager>();
    }
    private void UpdateRessourcesText()
    {
        _ressourcesNeeded.text = "";

        for (int i = 0; i < _upgarde[_levelWeapon - 1].Value.Count; i++)
        {
            _ressourcesNeeded.text += _upgarde[_levelWeapon - 1].Value[i].Value + " X " + _upgarde[_levelWeapon - 1].Value[i].Key + "\n\n";
        }
    }
    public void SetEffectLevelWeapon()
    {
        switch (_nameWeapon)
        {
            case "Mele":
                _swordVisual.NextSword(_levelWeapon - 1);
                UpdateRessourcesText();
                switch (_levelWeapon)
                {
                    case 2:
                        _playerAttack.UpgradeMachetteVisual(0);
                        _playerAttack.SetAttack(20);
                        _playerAttack.SetCadence(1.5f);
                        break;
                    case 3:
                        _playerAttack.UpgradeMachetteVisual(1);
                        _playerAttack.SetAttack(25);
                        _playerAttack.SetCadence(1f);
                        break;
                    case 4:
                        _playerAttack.UpgradeMachetteVisual(2);
                        _playerAttack.SetAttack(35);
                        _playerAttack.SetCadence(0.5f);
                        break;
                    default:
                        break;
                }
                break;
            case "Distance":
                _rangeWeapon._weaponLevel++;
                _rangeWeapon.NextWeapon();
                switch (_levelWeapon)
                {
                    case 2:
                        _rangeWeapon.UpgradeWeaponVisual(_rangeWeapon._level2Weapon);
                        break;
                    case 3:
                        if (isOwned)
                            ActiveGun();
                        _rangeWeapon.UpgradeWeaponVisual(_rangeWeapon._level3Weapon);
                        break;
                    case 4:
                        _rangeWeapon.UpgradeWeaponVisual(_rangeWeapon._level4Weapon);
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

    [Command]
    void ActiveGun()
    {
        _gunLevel3.SetActive(true);
        if (isOwned)
            _gunLevel3.SetActive(false);
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
                _textLevel.text = "Level " + _levelWeapon;
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
