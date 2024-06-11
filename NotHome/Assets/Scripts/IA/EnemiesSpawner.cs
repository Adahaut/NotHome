using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    public List<Transform> _spawnPoints;
    public GameObject _enemyPrefab;
    public int _numberOfEnemiesToSpawn;

    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < _numberOfEnemiesToSpawn; i++)
        {
            Vector3 _spawnPosition = GetRandomPointInPolygon();
            Instantiate(_enemyPrefab, _spawnPosition, Quaternion.identity);
        }
    }

    Vector3 GetRandomPointInPolygon()
    {
        Vector3 _randomPoint = Vector3.zero;
        bool _pointInPolygon = false;

        while (!_pointInPolygon)
        {
            //Limits of the polygon
            Bounds bounds = new Bounds(_spawnPoints[0].position, Vector3.zero);
            foreach (Transform _point in _spawnPoints)
            {
                bounds.Encapsulate(_point.position);
            }

            //random point in the area
            _randomPoint = new Vector3(Random.Range(bounds.min.x, bounds.max.x), 100, Random.Range(bounds.min.z, bounds.max.z));

            //check if is in the area
            if (IsPointInPolygon(_randomPoint))
            {
                _pointInPolygon = true;
            }
        }

        return _randomPoint;
    }

    bool IsPointInPolygon(Vector3 point)
    {
        // Utiliser la méthode de l'algorithme du rayon pour vérifier si le point est dans le polygone
        int crossingNumber = 0;

        for (int i = 0; i < _spawnPoints.Count; i++)
        {
            Transform p1 = _spawnPoints[i];
            Transform p2 = _spawnPoints[(i + 1) % _spawnPoints.Count];

            // Vérifier si un rayon horizontal vers la droite croise une arête du polygone
            if (((p1.position.z <= point.z && point.z < p2.position.z) || (p2.position.z <= point.z && point.z < p1.position.z)) &&
                (point.x < (p2.position.x - p1.position.x) * (point.z - p1.position.z) / (p2.position.z - p1.position.z) + p1.position.x))
            {
                crossingNumber++;
            }
        }

        // Le point est à l'intérieur du polygone si le nombre de croisements est impair
        return (crossingNumber % 2 == 1);
    }
}
