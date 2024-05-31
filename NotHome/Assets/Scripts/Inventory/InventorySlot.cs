using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InventorySlot : MonoBehaviour
{
    [SerializeField] public ItemObject _itemContained;
    public GameObject _itemImage;
    [SerializeField] private int _number;
    //[SerializeField] private TextMeshProUGUI _numberText;
    public bool _isSeleceted;

    private void Awake()
    {
        _itemImage = transform.GetChild(0).gameObject;
        ResetItem();
        //_numberText.text = "";
    }

    public int Number() { return _number; }

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

    public void UpdateNumber()
    {
        //_numberText.text = _number.ToString();
    }

    public ItemObject ItemContained() {  return _itemContained; }

    public void ChangeItem(string _ItemName, Sprite _itemSprite, bool _isAnEquipement)
    {
        if (!_isAnEquipement)
        {
            _itemContained.SetItem(_ItemName, _itemSprite);
            UpdateNumber();
            UpdateItemVisuel();
        }
    }

    public void ResetItem()
    {
        _itemContained = new ItemObject();
        _itemContained.SetItem("None", null);
        _number = 0;
        //_numberText.text = "";
        UpdateItemVisuel();
    }

    private void UpdateItemVisuel()
    {
        _itemImage.GetComponent<Image>().sprite = _itemContained.ItemSprite();
    }
}