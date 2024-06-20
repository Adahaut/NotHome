using Mirror;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class Enemies : NetworkBehaviour
{
    [SerializeField] private float _walkingRadius;
    [SerializeField] private float _timeBetweenAttacks;
    [SerializeField] private float _sightRange;
    [SerializeField] private float _attackRange;
    [SerializeField] private AudioSource[] _audioSources;
    [SerializeField] private Collider _playerDetectionCollider;

    private NavMeshAgent _agent;
    private Vector3 _initialPosition;
    private Transform _transform;
    public GameObject[] _players;
    private GameObject _closestPlayer;
    private bool _hasSeenPlayer;
    private bool _alreadyAttacked;
    private float _distanceToPlayer;
    private Animator _animator;
    public float _timer;
    public float _timerRate;

    private void Start()
    {
        _transform = transform;
        _initialPosition = _transform.position;
        _agent = GetComponent<NavMeshAgent>();
        _players = GameObject.FindGameObjectsWithTag("Player");
        _animator = GetComponent<Animator>();
        _timerRate = 2f;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if ( _timer > _timerRate )
        {
            _closestPlayer = GetClosestPlayer();
            _timer = 0f;
        }
        if (!isServer) return; // Ensure that only the server controls enemy logic


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

        // Sync position with clients
        RpcSyncPositionAndRotation(transform.position, transform.rotation);

        // Update animator with walking status
        _animator.SetFloat("IsWalking", _agent.velocity.magnitude);
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
            Debug.Log("player in sight range");
            return true;

            // Check if there is a direct line of sight
            //Vector3 directionToTarget = _transform.position - _closestPlayer.transform.position;
            //if (Physics.Raycast(_transform.position, directionToTarget, out RaycastHit hit, _sightRange))
            //{
            //    if (hit.transform.gameObject == _closestPlayer)
            //    {
            //        Debug.Log("seePlayer");
            //        return true;
            //    }
            //}
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

    private void Attack()
    {
        if (!_alreadyAttacked)
        {
            _alreadyAttacked = true;
            _animator.SetBool("IsAttacking", true);
            StartCoroutine(ResetAttackAfterAnimation());
        }
    }

    private IEnumerator ResetAttackAfterAnimation()
    {
        // Wait for the attack animation to finish
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
        _animator.SetBool("IsAttacking", false);
        yield return new WaitForSeconds(_timeBetweenAttacks);
        _alreadyAttacked = false;

        // Reactivate attack if player is still in attack range
        if (PlayerInAttackRange())
        {
            Attack();
        }
    }

    [ClientRpc]
    private void RpcSyncPositionAndRotation(Vector3 position, Quaternion rotation)
    {
        if (isServer) return; // Server doesn't need to sync position to itself
        transform.position = position;
        transform.rotation = rotation;
    }

    //active collider in animation to attack
    public void ActiveCollider()
    {
        print("activateCollider");
        _playerDetectionCollider.enabled = true;
        _audioSources[2].Play();
    }

    public void DesactiveCollider()
    {
        _playerDetectionCollider.enabled = false;
    }
}
