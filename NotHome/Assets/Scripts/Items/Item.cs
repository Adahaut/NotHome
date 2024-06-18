using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class Item : NetworkBehaviour
{
    [SerializeField] private string _name;

    [SerializeField] private Sprite _sprite;

    [SerializeField] private int _radius;

    public bool _isOnAnotherGameObject;

    [Header("For Metalic Parts")]
    [SerializeField] private bool _isMetalicPart;
    [SerializeField] private List<Mesh> _metalicMeshs = new();
    [SerializeField] private Material _metalicMaterials;
    
    public string ItemName() { return _name; }

    public Sprite ItemSprite() { return _sprite; }

    private void OnEnable()
    {
        print("here");
        if (_isMetalicPart)
        {
            print("metalic");
            int _index = Random.Range(0, _metalicMeshs.Count - 1);
            transform.GetChild(0).GetComponent<MeshFilter>().mesh = _metalicMeshs[_index];
            transform.GetChild(0).GetComponent<MeshRenderer>().material = _metalicMaterials;
        }

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
