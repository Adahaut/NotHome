using Mirror;
using UnityEngine;

[CreateAssetMenu]
public class ItemObject : ScriptableObject
{
    [SyncVar] public string _name;

    [SyncVar] public Sprite _sprite;

    [SyncVar] public bool _isAnEquipement;

    public string ItemName() { return _name; }

    public Sprite ItemSprite() { return _sprite; }


    public void SetItem(string _newName, Sprite _newSprite)
    {
        _name = _newName;
        _sprite = _newSprite;
    }
}
