using UnityEngine;
using UnityEngine.AI;

public class Enemies : MonoBehaviour
{
    [SerializeField] private float _walkingRadius;
    [SerializeField] private float _timeBetweenAttacks;
    [SerializeField] private float _sightRange;
    [SerializeField] private float _attackRange;

    private NavMeshAgent _agent;
    private Vector3 _initialPosition;
    private Transform _transform;
    public GameObject[] _players;
    private GameObject _closestPlayer;
    private bool _hasSeenPlayer;
    private bool _alreadyAttacked;
    private float _distanceToPlayer;
    private Collider _playerDetectionCollider;
    private Animator _animator;
    private void Start()
    {
        _transform = transform;
        _initialPosition = _transform.position;
        _agent = GetComponent<NavMeshAgent>();
        _players = GameObject.FindGameObjectsWithTag("Player");
        //_playerDetectionCollider = GetComponentInChildren<BoxCollider>();
        //_playerDetectionCollider.enabled = false;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _closestPlayer = GetClosestPlayer();

        if (_hasSeenPlayer || PlayerInSightRange())
        {
            _hasSeenPlayer = true;
            ChasePlayer(_closestPlayer);
        }
        else
        {
            Patrol();
        }

        if (PlayerInAttackRange())
        {
            _agent.ResetPath();
            Attack();
        }
    }

    private void Patrol()
    {
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            Vector3 _randomPoint = GetRandomPointInRadius(_initialPosition, _walkingRadius);
            _agent.SetDestination(_randomPoint);
        }
    }

    private GameObject GetClosestPlayer()
    {
        GameObject _closestPlayer = null;
        float _closestDistance = Mathf.Infinity;

        foreach (var player in _players)
        {
            float _distance = Vector3.Distance(_transform.position, player.transform.position);
            if (_distance < _closestDistance)
            {
                _closestPlayer = player;
                _closestDistance = _distance;
            }
        }

        return _closestPlayer;
    }

    private bool PlayerInSightRange()
    {
        if (_closestPlayer == null)
            return false;

        _distanceToPlayer = Vector3.Distance(_transform.position, _closestPlayer.transform.position);
        if (_distanceToPlayer <= _sightRange)
        {
            // Check if there is a direct line of sight
            Vector3 directionToTarget = _closestPlayer.transform.position - _transform.position;
            if (Physics.Raycast(_transform.position, directionToTarget, out RaycastHit hit, _sightRange))
            {
                if (hit.transform.gameObject == _closestPlayer)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool PlayerInAttackRange()
    {
        if (_closestPlayer == null)
            return false;

        _distanceToPlayer = Vector3.Distance(_transform.position, _closestPlayer.transform.position);
        return _distanceToPlayer <= _attackRange;
    }

    private void ChasePlayer(GameObject player)
    {
        if (player != null)
        {
            _agent.SetDestination(player.transform.position);
        }
    }

    private Vector3 GetRandomPointInRadius(Vector3 _center, float _radius)
    {
        Vector3 _randomPos = Random.insideUnitSphere * _radius;
        _randomPos += _center;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(_randomPos, out hit, _radius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return _center;
    }

    public void Attack()
    {
        if (!_alreadyAttacked)
        {
            _animator.SetBool("IsAttacking", true);
            _alreadyAttacked = true;
            _playerDetectionCollider.enabled = true;
            _animator.SetBool("IsAttacking", false);
            Invoke(nameof(ResetAttack), _timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        _playerDetectionCollider.enabled = false;
        _alreadyAttacked = false;

        // Reactivate attack if player is still in attack range
        if (PlayerInAttackRange())
        {
            Attack();
        }
    }

    public void ActiveCollider()
    {
        _playerDetectionCollider.enabled = true;
    }

    public void DesactiveCollider()
    {
        _playerDetectionCollider.enabled = false;
    }
}
