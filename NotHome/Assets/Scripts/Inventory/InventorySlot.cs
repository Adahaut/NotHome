using UnityEngine;
using UnityEngine.UI;


public class InventorySlot : MonoBehaviour
{
    [SerializeField] private ItemObject _itemContained;
    private GameObject _itemImage;
    private int _number;

    private void Awake()
    {
        _itemImage = transform.GetChild(0).gameObject;
        _itemContained = new ItemObject();
        _itemContained.SetItem("None", null);
    }

    public int Number() { return _number; }

    public ItemObject ItemContained() {  return _itemContained; }

    public void ChangeItem(string _ItemName, Sprite _itemSprite)
    {
        _itemContained.SetItem(_ItemName, _itemSprite);
        UpdateItemSprite();
    }

    private void UpdateItemSprite()
    {
        _itemImage.GetComponent<Image>().sprite = _itemContained.ItemSprite();
    }
}