using UnityEngine;
using UnityEngine.AI;

public class Boss2 : MonoBehaviour
{
    [SerializeField] private GameObject _hq;
    [SerializeField] private float _timeBetweenAttacks;
    [SerializeField] private float _attackRange;
    [SerializeField] private AudioSource[] _audioSources;

    private NavMeshAgent _agent;
    private Transform _transform;
    private Transform _hqTransform;
    private bool _alreadyAttacked;
    private float _distanceToHq;
    public Collider _attackCollider;
    private Animator _animator;

    private void Start()
    {
        _hq = GameObject.Find("MiniMaps (1)");
        _agent = GetComponent<NavMeshAgent>();
        _transform = transform;
        _hqTransform = _hq.transform;
        _animator = GetComponent<Animator>();

        _agent.SetDestination(_hqTransform.position);
    }

    private void Update()
    {
        _distanceToHq = Vector3.Distance(_transform.position, _hqTransform.position);

        if(HqInAttackRange())
        {
            _agent.ResetPath();
            Attack();
        }
        else
        {
            _agent.SetDestination(_hqTransform.position);
        }
    }

    private bool HqInAttackRange()
    {
        _distanceToHq = Vector3.Distance(_transform.position, _hqTransform.position);
        return _distanceToHq <= _attackRange;
    }

    public void Attack()
    {
        if (!_alreadyAttacked)
        {
            _animator.SetBool("IsAttacking", true);
            _alreadyAttacked = true;
            _attackCollider.enabled = true;
            _animator.SetBool("IsAttacking", false);
            Invoke(nameof(ResetAttack), _timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        _attackCollider.enabled = false;
        _alreadyAttacked = false;

        // Reactivate attack if player is still in attack range
        if (HqInAttackRange())
        {
            Attack();
        }
    }

    //active collider in animation to attack
    public void ActiveCollider()
    {
        _attackCollider.enabled = true;
        _audioSources[2].Play();
    }

    public void DesactiveCollider()
    {
        _attackCollider.enabled = false;
    }
}
