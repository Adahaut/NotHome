using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class SpawnZone
{
    public List<Transform> _points;
    public List<GameObject> _spawnablePrefabs;
    public int _numberOfEnemiesToSpawn;
}

public class EnemiesSpawner : NetworkBehaviour
{
    public List<SpawnZone> _spawnZones;
    public LayerMask _groundLayer;
    public Transform _enemiesParent;

    private void Start()
    {
        //SpawnEnemies(0);
    }

    public void SpawnEnemies(int _zoneIndex)
    {
        print("spawn " +  _zoneIndex);
        if (_zoneIndex < 0 || _zoneIndex >= _spawnZones.Count)
        {
            Debug.LogError("Invalid spawn zone index.");
            return;
        }

        SpawnZone _selectedZone = _spawnZones[_zoneIndex];

        for (int i = 0; i < _selectedZone._numberOfEnemiesToSpawn; i++)
        {
            Vector3 _spawnPosition = GetRandomPointInPolygon(_selectedZone._points);

            // Adjust the spawn position to be on the ground
            _spawnPosition = GetGroundPosition(_spawnPosition);

            if (_spawnPosition != Vector3.zero)
            {
                GameObject go = Instantiate(_selectedZone._spawnablePrefabs[Random.Range(0, _selectedZone._spawnablePrefabs.Count)], _spawnPosition, Quaternion.identity, _enemiesParent);
                //CmdSpawnObject(go);
                NetworkServer.Spawn(go);
                //NetworkServer.Spawn(go);
            }
        }
    }

    [Command(requiresAuthority = false)]
    void CmdSpawnObject(GameObject obj)
    {
        
    }

    public void DestroyAllEnemies()
    {
        if (_enemiesParent.childCount == 0)
            return;

        foreach (Transform child in _enemiesParent)
        {
            NetworkServer.Destroy(child.gameObject);
        }
    }

    Vector3 GetRandomPointInPolygon(List<Transform> _polygon)
    {
        Vector3 _randomPoint = Vector3.zero;
        bool _pointInPolygon = false;

        while (!_pointInPolygon)
        {
            // Limit of the selected spawn area
            Bounds bounds = new Bounds(_polygon[0].position, Vector3.zero);
            foreach (Transform point in _polygon)
            {
                bounds.Encapsulate(point.position);
            }

            // Random point in the area
            _randomPoint = new Vector3(Random.Range(bounds.min.x, bounds.max.x), 100, Random.Range(bounds.min.z, bounds.max.z));

            // Check if is in the area
            if (IsPointInPolygon(_randomPoint, _polygon))
            {
                _pointInPolygon = true;
            }
        }

        return _randomPoint;
    }

    Vector3 GetGroundPosition(Vector3 _position)
    {
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(_position.x, 100, _position.z), Vector3.down, out hit, Mathf.Infinity, _groundLayer))
        {
            Vector3 groundPosition = hit.point;
            NavMeshHit navMeshHit;

            if (NavMesh.SamplePosition(groundPosition, out navMeshHit, 1.0f, NavMesh.AllAreas))
            {
                return navMeshHit.position;
            }
        }

        return Vector3.zero;
    }

    bool IsPointInPolygon(Vector3 _point, List<Transform> _polygon)
    {
        int _crossingNumber = 0;

        for (int i = 0; i < _polygon.Count; i++)
        {
            Transform p1 = _polygon[i];
            Transform p2 = _polygon[(i + 1) % _polygon.Count];

            // Vérifier si un rayon horizontal vers la droite croise une arête du polygone
            if (((p1.position.z <= _point.z && _point.z < p2.position.z) || (p2.position.z <= _point.z && _point.z < p1.position.z)) &&
                (_point.x < (p2.position.x - p1.position.x) * (_point.z - p1.position.z) / (p2.position.z - p1.position.z) + p1.position.x))
            {
                _crossingNumber++;
            }
        }

        // Le point est à l'intérieur du polygone si le nombre de croisements est impair
        return (_crossingNumber % 2 == 1);
    }
}
