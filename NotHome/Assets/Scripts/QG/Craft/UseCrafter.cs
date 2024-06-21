using Steamworks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UseCrafter : MonoBehaviour
{
    [SerializeField] private List<Button> _listButton = new();
    [SerializeField] private List<CraftScriptableObject> _listCraft = new();
    private CraftScriptableObject _currentCraft;
    [SerializeField] private Image _spriteCraft;
    public TextMeshProUGUI _craftName;
    [SerializeField] private List<GameObject> _materialsText = new List<GameObject>();
    [SerializeField] private InventoryManager _playerInventory;
    [SerializeField] private PlayerStockageUI _inventoryBase;
    public TextMeshProUGUI _textFeedBack;
    private Coroutine _feedback;

    private List<string> _materialsNameForCraft = new List<string>();
    private List<int> _materialsNumberForCraft = new List<int>();
    private int _indexMove;

    private void OnEnable()
    {
        ClearTexts();
        _craftName.text = "";
        _spriteCraft.color = new Color(255, 255, 255, 0);
    }

    private void Start()
    {
        _playerInventory = GetComponentInParent<PlayerController>()._inventory.GetComponent<InventoryManager>();
    }

    public void OnClick(Button button)
    {
        int index;
        if (button.name.Length > _listButton[0].name.Length)
            index = int.Parse(button.name[6].ToString() + button.name[7].ToString()) - 1;
        else
            index = int.Parse(button.name[6].ToString()) - 1;

        _spriteCraft.gameObject.SetActive(true);
        _spriteCraft.sprite = _listButton[index].GetComponent<Image>().sprite;
        _spriteCraft.color = Color.white;
        SetMaterialsCraft(index);
        _craftName.text = _currentCraft._resultName;
    }
    public void MoveButton(int direction)
    {
        for (int i = 0;  i < _listButton.Count; i++)
        {
            _listButton[i].transform.position += new Vector3(100 * Screen.width / 1920 * direction, 0, 0);
        }
        if (direction < 0)
        {
            _listButton[_indexMove].transform.position += new Vector3(100 * Screen.width / 1920 * _listButton.Count, 0, 0);
            _indexMove += 1;
            if (_indexMove == _listButton.Count)
                _indexMove = 0;
        }
        else
        {
            if (_indexMove != 0)
            {
                _listButton[_indexMove - 1].transform.position -= new Vector3(100 * Screen.width / 1920 * _listButton.Count, 0, 0);
                _indexMove -= 1;
                if (_indexMove < 0)
                    _indexMove = _listButton.Count - 1;
            }
            else
            {
                _listButton[_listButton.Count - 1].transform.position -= new Vector3(100 * Screen.width / 1920 * _listButton.Count, 0, 0);
                _indexMove = _listButton.Count - 1;
            }
        }
    }

    public void SetPlayerInventory(InventoryManager _newInventory)
    {
        _playerInventory = _newInventory;
    }

    public void CraftObject()
    {
        if (!_playerInventory.HasRemainingPlace(_currentCraft._resultName))
        {
            _feedback = StartCoroutine(CraftFeedBack(1));
            return;
        }

        bool _canCraft = CheckInplayerInventoryAndBase();


        if (_canCraft)
        {
            CraftItem();
            _feedback = StartCoroutine(CraftFeedBack(0));
        }
        else
        {
            _feedback = StartCoroutine(CraftFeedBack(2));
            return;
        }
    }

    private void ChangeText(string _text, Color _color)
    {
        _textFeedBack.text = _text;
        _textFeedBack.color = _color;
    }

    private IEnumerator CraftFeedBack(int _case)
    {
        switch (_case)
        {
            case 0:
                ChangeText("Item crafted !", Color.green);
                break;
            case 1:
                ChangeText("Not enough resources !", Color.red);
                break;
            case 2:
                ChangeText("No more inventory space !", Color.red);
                break;
        }
        _textFeedBack.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        _textFeedBack.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        StopCoroutine(_feedback);
    }

    private void CraftItem()
    {
        for (int i = 0; i < _materialsNameForCraft.Count; i++)
            RemoveItemsForCraft(_materialsNameForCraft[i]);
        _playerInventory.AddItem(_currentCraft._resultName, _currentCraft._resultSprite, _currentCraft._isAnEquipement);
        if (_currentCraft._resultName == "Ammo")
            GetComponentInParent<PlayerController>().GetComponentInChildren<RangeWeapon>().AddAmmo(1);
        QuestManager.Instance.QuestComplete(5);
    }

    private void RemoveItemsForCraft(string _materialName)
    {
        _playerInventory.RemoveItems(_materialName, _materialsNumberForCraft[_materialsNameForCraft.IndexOf(_materialName)]);
        if (InventoryBaseManager.instance.CheckForMaterial(_materialName))
            RemoveItemFromBase(_materialName, _materialsNumberForCraft[_materialsNameForCraft.IndexOf(_materialName)]);
    }

    private void RemoveItemFromBase(string _name, int _numberRemoved)
    {
        int _index = _inventoryBase.GetIndexOf(_name);

        if (InventoryBaseManager.instance._inventoryItems[_index]._number == _numberRemoved)
        {
            _inventoryBase.RemoveItemFromBase(_name, _numberRemoved, _index, _inventoryBase._slotList[_index].GetComponent<InventorySlot>());
        }
        else
        {
            _inventoryBase.UpdateItemInList(_name, _index, _numberRemoved);
        }

        _inventoryBase.UpdateUI();
    }

    private bool CheckInplayerInventoryAndBase()
    {
        //create a bool list to check if player has enough ressources
        List<bool> result = new List<bool>();
        for(int i = 0; i < _materialsNameForCraft.Count; i++)
        {
            result.Add(false);
        }

        //check in player inventory if there are needed ressources
        for (int i = 0; i < _materialsNameForCraft.Count; i++)
        {
            for (int y = 0; y < _playerInventory.InventorySlotNumber(); y++)
            {
                if (CheckInBothInventory(i, y))
                {
                    result[i] = true;
                }
            }
        }

        return !result.Contains(false);
    }

    private bool CheckInBothInventory(int i, int y)
    {
        if (InventoryBaseManager.instance.CheckForMaterial(_materialsNameForCraft[i]) || (_playerInventory.GetInventorySlot(y).ItemContained().ItemName() == _materialsNameForCraft[i]))
        {
            if (InventoryBaseManager.instance.CheckForMaterial(_materialsNameForCraft[i]) && InventoryBaseManager.instance.NumberOfMaterial(_materialsNameForCraft[i]) + _playerInventory.GetInventorySlot(y).Number() >= _materialsNumberForCraft[i])
            {
                return true;
            }
            else if (_playerInventory.GetInventorySlot(y).Number() >= _materialsNumberForCraft[i])
            {
                return true;
            }
        }
        return false;
    }

    private void SetMaterialsCraft(int _index)
    {
        ClearTexts();
        for (int i = 0; i < _listCraft[_index]._materialName.Count; i++)
        {
            AddText(_listCraft[_index]._materialName[i], _listCraft[_index]._materialNumber[i], _listCraft[_index]._materialSprite[i]);
            _currentCraft = _listCraft[_index];
        }
    }

    private void ClearTexts()
    {
        _materialsNameForCraft.Clear();
        _materialsNumberForCraft.Clear();
        for (int i = 0; i < _materialsText.Count; i++)
        {
            _materialsText[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            _materialsText[i].SetActive(false);
        }
    }
    private void AddText(string _materialName, int _materialNumber, Sprite _materialSprite)
    {
        for(int i = 0; i < _materialsText.Count; i++)
        {
            if (!_materialsText[i].activeInHierarchy)
            {
                _materialsText[i].SetActive(true);
                _materialsText[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _materialNumber.ToString() + " " + _materialName;
                _materialsText[i].transform.GetChild(1).GetComponent<Image>().sprite = _materialSprite;
                _materialsNameForCraft.Add(_materialName);
                _materialsNumberForCraft.Add(_materialNumber);
                break;
            }
        }
    }
}
