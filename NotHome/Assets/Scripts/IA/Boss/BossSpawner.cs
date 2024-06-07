using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _bossPrefab;
    [SerializeField] private float _minDistance = 2000f;

    [SerializeField] private float _minSpawnDelay = 5f;
    [SerializeField] private float _maxSpawnDelay = 10f;
    private Transform _transform;
    public float _navMeshSampleDistance = 5f;
    private Vector3 _spawnPosition;
    private float _spawnDelay;


    void Start()
    {
        _transform = transform;

        if (_bossPrefab != null)
        {
            StartCoroutine(SpawnBossCoroutine());
        }
    }

    IEnumerator SpawnBossCoroutine()
    {
        while (true)
        {
            _spawnDelay = Random.Range(_minSpawnDelay, _maxSpawnDelay);
            yield return new WaitForSeconds(_spawnDelay);

            _spawnPosition = GetNavMeshSpawnPosition();
            if (_spawnPosition != Vector3.zero)
            {
                Instantiate(_bossPrefab, _spawnPosition, Quaternion.identity);
            }
        }
    }

    Vector3 GetNavMeshSpawnPosition()
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 _randomDirection = Random.insideUnitSphere.normalized;
            _randomDirection.y = 0;
            Vector3 _randomPosition = _transform.position + _randomDirection * _minDistance;

            if (NavMesh.SamplePosition(_randomPosition, out NavMeshHit hit, _navMeshSampleDistance, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        return Vector3.zero;
    }
}
