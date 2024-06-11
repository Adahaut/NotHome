using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnZone
{
    public List<Transform> _points;
    public List<GameObject> _spawnablePrefabs;
    public int _numberOfEnemiesToSpawn;
}

public class EnemiesSpawner : MonoBehaviour
{
    public List<SpawnZone> _spawnZones;

    void Start()
    {
        SpawnEnemies(1);
    }

    public void SpawnEnemies(int _zoneIndex)
    {
        if (_zoneIndex < 0 || _zoneIndex >= _spawnZones.Count)
        {
            Debug.LogError("Invalid spawn zone index.");
            return;
        }

        SpawnZone _selectedZone = _spawnZones[_zoneIndex];

        for (int i = 0; i < _selectedZone._numberOfEnemiesToSpawn; i++)
        {
            GameObject _prefabToSpawn = _selectedZone._spawnablePrefabs[Random.Range(0, _selectedZone._spawnablePrefabs.Count)];
            Vector3 _spawnPosition = GetRandomPointInPolygon(_selectedZone._points);
            Instantiate(_prefabToSpawn, _spawnPosition, Quaternion.identity);
        }
    }

    Vector3 GetRandomPointInPolygon(List<Transform> _polygon)
    {
        Vector3 _randomPoint = Vector3.zero;
        bool _pointInPolygon = false;

        while (!_pointInPolygon)
        {
            //limit of the selected spawn area
            Bounds bounds = new Bounds(_polygon[0].position, Vector3.zero);
            foreach (Transform point in _polygon)
            {
                bounds.Encapsulate(point.position);
            }

            //random point in the area
            _randomPoint = new Vector3(Random.Range(bounds.min.x, bounds.max.x), 100, Random.Range(bounds.min.z, bounds.max.z));

            //check if is in the area
            if (IsPointInPolygon(_randomPoint, _polygon))
            {
                _pointInPolygon = true;
            }
        }

        return _randomPoint;
    }

    bool IsPointInPolygon(Vector3 _point, List<Transform> _polygon)
    {
        int _crossingNumber = 0;

        for (int i = 0; i < _polygon.Count; i++)
        {
            Transform p1 = _polygon[i];
            Transform p2 = _polygon[(i + 1) % _polygon.Count];

            //Vérifier si un rayon horizontal vers la droite croise une arête du polygone
            if (((p1.position.z <= _point.z && _point.z < p2.position.z) || (p2.position.z <= _point.z && _point.z < p1.position.z)) &&
                (_point.x < (p2.position.x - p1.position.x) * (_point.z - p1.position.z) / (p2.position.z - p1.position.z) + p1.position.x))
            {
                _crossingNumber++;
            }
        }

        //Le point est à l'intérieur du polygone si le nombre de croisements est impair
        return (_crossingNumber % 2 == 1);
    }
}
