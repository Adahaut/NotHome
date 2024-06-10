using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{    
    private NavMeshAgent _agent;
    [SerializeField] private GameObject _playerRef;
    [SerializeField] private float _walkingRadius;
    private Vector3 _initialPos;
    [SerializeField] private float _sightRange;
    [SerializeField] private float _attackRange;

    [SerializeField] private float _timeBetweenAttacks;
    private bool _alreadyAttacked;
    private bool _canAttack;
    [SerializeField] private int _damages;
    [SerializeField] private Collider _playerDetectionCollider;

    Node start;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _playerRef = GameObject.FindWithTag("Player");
        _initialPos = transform.position;
        _playerDetectionCollider = GetComponent<BoxCollider>();

        //BT
        Patrol patrol = new Patrol(_walkingRadius, _agent, _initialPos);
        SeePlayer seePlayer = new SeePlayer(_playerRef, _sightRange, _agent);
        ChasePlayer chasePlayer = new ChasePlayer(_agent, _playerRef);
        CanAttack canAttack = new CanAttack(_playerRef, _attackRange, _agent);
        Attack attack = new Attack(this);

        Sequence sequence1 = new Sequence(new List<Node> { canAttack, attack });
        Selector selector1 = new Selector(new List<Node> { sequence1, chasePlayer });
        Sequence sequence2 = new Sequence(new List<Node> { seePlayer, selector1 });
        Selector selector2 = new Selector(new List<Node> { sequence2, patrol });

        start = selector2;

    }

    void Update()
    {
        start.Evaluate();
    }

    public void Attack()
    {
        if (!_alreadyAttacked)
        {
            _alreadyAttacked = true;
            _playerDetectionCollider.enabled = true;
            Debug.Log("Attack");
            Invoke(nameof(ResetAttack), _timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        _playerDetectionCollider.enabled = false;
        _alreadyAttacked = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            _playerRef.GetComponent<LifeManager>().TakeDamage(_damages);
        }
    }
}
