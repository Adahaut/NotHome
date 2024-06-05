using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InventorySlot : MonoBehaviour
{
    [SerializeField] public ItemObject _itemContained;
    public Image _itemImage;
    [SerializeField] private int _number;
    public TextMeshProUGUI _numberText;
    public bool _isSeleceted;

    private bool firstOpened;


    private void Awake()
    {
        firstOpened = true;
        _itemImage = transform.GetChild(0).gameObject.GetComponent<Image>();
        ResetItem();
        _numberText.text = "";
    }

    public int Number() { return _number; }

    public void SetNumberInventorySlot(int n)
    {
        _number = n;
        _numberText.text = _number.ToString();
    }

    public void AddNumber() 
    {  
        _number++;
        UpdateNumber();
    }

    public void SetNumber(int _newNumber)
    {
        _number = _newNumber;
        if (_number == 0)
        {
            ResetItem();
        }
        else
            UpdateNumber();
    }

    public void UpdateItem(int _newNumber, Sprite _newSprite, string _name)
    {
        if(_itemImage == null)
            _itemImage = transform.GetChild(0).gameObject.GetComponent<Image>();

        ResetItem();

        _itemImage.sprite = _newSprite;
        _number = _newNumber;
        UpdateNumber();

        
        _itemContained.SetItem(_name, _newSprite);
    }

    public void UpdateNumber()
    {
        _numberText.text = _number.ToString();
    }

    public ItemObject ItemContained() {  return _itemContained; }

    public void ChangeItem(string _ItemName, Sprite _itemSprite, bool _isAnEquipement)
    {
        if (!_isAnEquipement)
        {
            _itemContained.SetItem(_ItemName, _itemSprite);
            UpdateNumber();
            UpdateItemVisuel();
            GetComponent<Image>().sprite = null;
        }
    }

    public void ResetItem()
    {
        _itemContained = new ItemObject();
        _itemContained.SetItem("None", null);
        _number = 0;
        _numberText.text = "";
        UpdateItemVisuel();
    }

    public void UpdateItemVisuel()
    {
        if (_itemImage == null)
            _itemImage = transform.GetChild(0).gameObject.GetComponent<Image>();

        _itemImage.sprite = _itemContained.ItemSprite();
    }
}