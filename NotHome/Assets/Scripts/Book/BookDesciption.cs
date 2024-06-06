using UnityEngine;

[CreateAssetMenu(fileName = "bookDesc", menuName = "Book")]
public class BookDesciption : ScriptableObject
{
    public string _description;
    public string _other;
    public Sprite _sprite;
    public bool _isDiscovered;
}
