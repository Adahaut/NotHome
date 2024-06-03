using Mirror;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private string _name;

    [SerializeField] private Sprite _sprite;

    [SerializeField] private int _radius;

    public bool _isOnAnotherGameObject;
    


    public string ItemName() { return _name; }

    public Sprite ItemSprite() { return _sprite; }

    private void OnEnable()
    {
        _isOnAnotherGameObject = false;
        RaycastHit[] _hits = Physics.SphereCastAll(transform.position, _radius, transform.forward);
        for (int i = 0; i < _hits.Length; ++i)
        {
            if (!_hits[i].collider.CompareTag("Terrain") && _hits[i].collider.gameObject != gameObject)
            {
                _isOnAnotherGameObject = true;
            }
        }
    }

}
