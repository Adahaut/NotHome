using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    [Header("Spawn Options")]
    [SerializeField, Range(1, 6)] private int _priority;
    [SerializeField] private int _spawnRange;
    [SerializeField] private List<GameObject> _items = new List<GameObject>();
    [SerializeField] private int _minimumItem;

    private void SpawnAnItem(GameObject _item)
    {
        GameObject _newItem = Instantiate(_item);
        StartCoroutine(SetPositionOfItem(_newItem, CreateRandomPosition()));
        _newItem.AddComponent<Rigidbody>();
    }

    private IEnumerator SetPositionOfItem(GameObject _item, Vector3 _position)
    {
        _item.transform.position = _position;
        _item.SetActive(true);
        if (_item.GetComponent<Item>()._isOnAnotherGameObject)
        {
            Destroy(_item);
            SpawnAnItem(_items[Random.Range(0, _items.Count - 1)]);
            yield return null;
        }
        else if (!Physics.Raycast(_item.transform.position, _item.transform.up * -1, out RaycastHit _hit))
        {
            print("en bas");
            StartCoroutine(SetPositionOfItem(_item, new Vector3(_position.x, _position.y + 1, _position.z)));
        }
        yield return null;
    }

    private Vector3 CreateRandomPosition()
    {
        return new Vector3(transform.position.x + Random.Range(-_spawnRange, _spawnRange), transform.position.y, transform.position.z + Random.Range(-_spawnRange, _spawnRange));
    }

    public void SpawnItems()
    {
        for(int i = 0; i < (_minimumItem * _priority); i++)
        {
            SpawnAnItem(_items[Random.Range(0, _items.Count - 1)]);
        }
    }

    private void Start()
    {
        SpawnItems();
    }
}
