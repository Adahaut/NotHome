using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Enderman : MonoBehaviour
{
    [SerializeField] private float _checkRadius;
    private NavMeshAgent _agent;
    private Transform _thisTransform;
    private GameObject[] _players;

    private void Start()
    {
        _players = GameObject.FindGameObjectsWithTag("Player");
        _agent = GetComponent<NavMeshAgent>();
        _thisTransform = transform;
    }

    private void Update()
    {
        GameObject closestPlayer = GetClosestPlayer();
        if (closestPlayer == null)
        {
            _agent.enabled = false;
            return;
        }

        bool _isVisibleByAnyPlayer = _players.Any(player => IsEnemyVisibleByPlayer(player));
        float _distanceToPlayer = Vector3.Distance(_thisTransform.position, closestPlayer.transform.position);

        if (_distanceToPlayer <= _checkRadius && _isVisibleByAnyPlayer)
        {
            _agent.enabled = false;
        }
        else
        {
            _agent.enabled = true;
            _agent.SetDestination(closestPlayer.transform.position);
        }
    }

    private GameObject GetClosestPlayer()
    {
        GameObject _closestPlayer = null;
        float _closestDistance = Mathf.Infinity;

        foreach (var _player in _players)
        {
            float _distance = Vector3.Distance(_thisTransform.position, _player.transform.position);
            if (_distance < _closestDistance)
            {
                _closestPlayer = _player;
                _closestDistance = _distance;
            }
        }

        return _closestPlayer;
    }

    private bool IsEnemyVisibleByPlayer(GameObject _player)
    {
        Camera _playerCam = _player.GetComponentInChildren<Camera>();

        // Check if the enemy is in the player's camera frustum
        Plane[] _planes = GeometryUtility.CalculateFrustumPlanes(_playerCam);
        bool _isInFrustum = _planes.All(plane => plane.GetDistanceToPoint(_thisTransform.position) >= 0);

        if (!_isInFrustum)
        {
            return false;
        }

        // Check if there is a direct line of sight
        Vector3 directionToEnemy = _thisTransform.position - _playerCam.transform.position;
        if (Physics.Raycast(_playerCam.transform.position, directionToEnemy, out RaycastHit hit))
        {
            if (hit.transform.gameObject == gameObject)
            {
                return true;
            }
        }

        return false;
    }
}
