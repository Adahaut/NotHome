using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InventorySlot : MonoBehaviour
{
    [SerializeField] private ItemObject _itemContained;
    private GameObject _itemImage;
    private int _number;
    [SerializeField] private TextMeshProUGUI _numberText;

    private void Awake()
    {
        _itemImage = transform.GetChild(0).gameObject;
        ResetItem();
        _numberText.text = "";
    }

    public int Number() { return _number; }

    public void AddNumber() 
    {  
        _number++;
        _numberText.text = _number.ToString();
    }

    public void SetNumber(int _newNumber)
    {
        _number = _newNumber;
        _numberText.text = _number.ToString();
    }

    public ItemObject ItemContained() {  return _itemContained; }

    public void ChangeItem(string _ItemName, Sprite _itemSprite)
    {
        _itemContained.SetItem(_ItemName, _itemSprite);
        _numberText.text = _number.ToString();
        UpdateItemVisuel();
    }

    public void ResetItem()
    {
        _itemContained = new ItemObject();
        _itemContained.SetItem("None", null);
        _number = 1;
        _numberText.text = "";
        UpdateItemVisuel();
    }

    private void UpdateItemVisuel()
    {
        _itemImage.GetComponent<Image>().sprite = _itemContained.ItemSprite();
    }
}