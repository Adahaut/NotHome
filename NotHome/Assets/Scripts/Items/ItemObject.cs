using UnityEngine;

public class ItemObject : ScriptableObject
{
    [SerializeField] private string _name;

    [SerializeField] private Sprite _sprite;

    [SerializeField] private bool _isAnEquipement;

    public string ItemName() { return _name; }

    public Sprite ItemSprite() { return _sprite; }


    public void SetItem(string _newName, Sprite _newSprite)
    {
        _name = _newName;
        //_sprite = _newSprite;
    }
}
