using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private string _name;

    [SerializeField] private Sprite _sprite;

    public string ItemName() { return _name; }

    public Sprite ItemSprite() { return _sprite; }
}
