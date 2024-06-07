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
    private Vector3 _chargeDirection;

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
            //_agent.ResetPath();
            return;
        }

        _distanceToQg = Vector3.Distance(_transform.position, _qgTransform.position);
        if (!_isCharging && _distanceToQg <= _chargingDistance)
        {
            Debug.Log("supposed to charge");
            StartCoroutine(ChargeAttack());
        }
    }

    IEnumerator ChargeAttack()
    {
        Debug.Log("WaitBeforeCharging");
        _isCharging = true;
        _agent.ResetPath();
        yield return new WaitForSeconds(_chargeTime);

        Debug.Log("Charge");
        _chargeDirection = (_qgTransform.position - _transform.position).normalized;

        float chargeStartTime = Time.time;
        while (!_isStunned && Vector3.Distance(_transform.position, _qgTransform.position) > _agent.stoppingDistance)
        {
            float elapsedTime = Time.time - chargeStartTime;
            Vector3 nextPosition = _transform.position + _chargeDirection * _chargeSpeed * Time.deltaTime;
            _agent.Warp(nextPosition);
            yield return null;
        }

        _isCharging = false;
        _agent.SetDestination(_qgTransform.position);
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

        Vector3 knockbackDirection = (_transform.position - _qgTransform.position).normalized;
        Vector3 knockbackPosition = _transform.position + knockbackDirection * _knockbackDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(knockbackPosition, out hit, _knockbackDistance, NavMesh.AllAreas))
        {
            knockbackPosition = hit.position;
        }
        else
        {
            Vector3 randomDirection = Random.insideUnitSphere * 60f;
            randomDirection += _transform.position;
            NavMesh.SamplePosition(randomDirection, out hit, 60f, NavMesh.AllAreas);
            knockbackPosition = hit.position;
        }

        yield return new WaitForSeconds(_stunDuration);
        _agent.Warp(knockbackPosition);

        _isStunned = false;
        _agent.SetDestination(_qgTransform.position);
    }
}