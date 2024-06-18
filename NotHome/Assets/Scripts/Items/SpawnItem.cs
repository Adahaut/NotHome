using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnItem : MonoBehaviour
{
    [Header("Spawn Options")]
    public List<GameObject> _items = new List<GameObject>();
    public float _spawnChance;
    public float _maxChanceFactor;

    [Header("Raycast Options")]
    public float _distanceBetweenCheck;
    public float _heightOfCheck = 10;
    public float _rangeOfCheck;
    public LayerMask _layerMask;
    public Vector2 _positivePosition;
    public Vector2 _negativePosition;
    private Vector3 _position;

    private void Start()
    {
        _position = transform.position;
        ItemSpawn();
    }
    public void ItemSpawn()
    {
        for (float x = _negativePosition.x; x < _positivePosition.x; x += _distanceBetweenCheck)
        {
            for (float z = _negativePosition.y; z < _positivePosition.y; z += _distanceBetweenCheck)
            {
                RaycastHit hit;
                Vector3 _pos = new Vector3(x, _heightOfCheck, z) + _position;
                if (Physics.Raycast(_pos, Vector3.down, out hit, _rangeOfCheck, _layerMask))
                {
                    if (_spawnChance > Random.Range(0f, _maxChanceFactor))
                    {
                        Instantiate(_items[Random.Range(0, _items.Count)], hit.point, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)), transform);
                    }
                }
            }
        }
    }

    public void DeleteResources()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
