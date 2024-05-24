using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    [SerializeField] private GameObject _qg;
    [SerializeField] private float _chargeTime;
    [SerializeField] private float _chargeSpeed;
    [SerializeField] private float _chargingDistance;
    [SerializeField] private float _stunDuration;
    [SerializeField] private float _knockbackDistance;

    private float _distanceToQg;
    private NavMeshAgent _agent;
    private bool _isCharging;
    private bool _isStunned;
    private Transform _transform;
    private Transform _qgTransform;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _transform = transform;
        _qgTransform = _qg.transform;

        _agent.SetDestination(_qgTransform.position);
    }

    private void Update()
    {
        if (_isStunned)
        {
            return;
        }

        _distanceToQg = Vector3.Distance(_transform.position, _qgTransform.position);
        if (!_isCharging && _distanceToQg <= _chargingDistance)
        {
            StartCoroutine(ChargeAttack());
        }
    }

    IEnumerator ChargeAttack()
    {
        _isCharging = true;
        _agent.ResetPath();
        yield return new WaitForSeconds(_chargeTime);

        _agent.speed = _chargeSpeed;
        _agent.SetDestination(_qg.transform.position);

        while (!_isStunned && Vector3.Distance(_transform.position, _qg.transform.position) > _agent.stoppingDistance)
        {
            yield return null;
        }

        _isCharging = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Rampart"))
        {
            Debug.Log("Collision rampart");
            StartCoroutine(StunBoss());
        }
    }

    IEnumerator StunBoss()
    {
        Debug.Log("Stun");
        _isStunned = true;
        _isCharging = false;
        _agent.ResetPath();

        Vector3 _knockbackDirection = (_transform.position - _qgTransform.position).normalized;
        Vector3 _knockbackPosition = _transform.position + _knockbackDirection * _knockbackDistance;
        _agent.Warp(_knockbackPosition); //teleport

        yield return new WaitForSeconds(_stunDuration);
        
        _isStunned = false;
        _agent.SetDestination(_qgTransform.position);
    }
}