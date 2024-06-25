using Mirror;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.UIElements;

public class SpawnItem : NetworkBehaviour
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

    private List<GameObject> _spawnedItems = new List<GameObject>();

    private void Start()
    {
        _position = transform.position;
        //ItemSpawn();
    }

    [Server]
    private void ItemSpawn()
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
                        GameObject _newItem = Instantiate(_items[Random.Range(0, _items.Count)], hit.point, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
                        NetworkServer.Spawn(_newItem);
                        RpcSetupItem(_newItem, _newItem.transform.position, _newItem.transform.rotation, transform);
                        //_newItem.transform.SetParent(transform);
                        _spawnedItems.Add(_newItem);
                    }
                }
            }
        }
    }

    [ClientRpc]
    private void RpcSetupItem(GameObject item, Vector3 position, Quaternion rotation, Transform parent)
    {
        Debug.Log("RpcSetupItem called");

        if (parent == null)
            parent = transform;


        item.transform.SetParent(parent);
        item.transform.position = position;
        item.transform.rotation = rotation;

    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (isClient && !isServer)
        {
            foreach (GameObject item in _spawnedItems)
            {
                item.transform.SetParent(transform);
            }
        }
    }

    [Command(requiresAuthority = false)]
    void CmdSpawnItem(GameObject obj)
    {
        NetworkServer.Spawn(obj);
    }

    public void DeleteResources()
    {
        if (transform.childCount == 0)
            return;

        foreach (Transform child in transform)
        {
            NetworkServer.Destroy(child.gameObject);
        }
    }

    public void Spawn()
    {
        ItemSpawn();
    }

    public void DestroyAndSpawn()
    {
        DeleteResources();
        print("spawn " + gameObject.name);
        ItemSpawn();
    }
}
