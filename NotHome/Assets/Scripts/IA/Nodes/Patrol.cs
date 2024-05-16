using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : Node
{
    public float _walkingRadius;
    private NavMeshAgent _agent;
    private Vector3 _initialPosition;

    public Patrol(float walkingRadius, NavMeshAgent agent, Vector3 initialPos)
    {
        _walkingRadius = walkingRadius;
        _agent = agent;
        _initialPosition = initialPos;
    }

    Vector3 GetRandomPointInRadius(Vector3 center, float radius)
    {
        Vector3 _randomPos = Random.insideUnitSphere * radius;
        _randomPos += center;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(_randomPos, out hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return center;
    }

    public override NodeState Evaluate()
    {
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            Vector3 randomPoint = GetRandomPointInRadius(_initialPosition, _walkingRadius);
            _agent.SetDestination(randomPoint);
        }

        _nodeState = NodeState.SUCCESS;
        return _nodeState;
    }    
}