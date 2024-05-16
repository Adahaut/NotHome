using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CanAttack : Node
{
    private GameObject _playerRef;
    private float _attackRange;
    private NavMeshAgent _agent;

    public CanAttack(GameObject playerRef, float attackRange, NavMeshAgent agent)
    {
        _playerRef = playerRef;
        _attackRange = attackRange;
        _agent = agent;
    }

    public override NodeState Evaluate()
    {
        float distanceToPlayer = Vector3.Distance(_agent.transform.position, _playerRef.transform.position);

        //check if player is in attack range
        if (distanceToPlayer <= _attackRange)
        {
            _nodeState = NodeState.SUCCESS;
        }
        else
        {
            _nodeState = NodeState.FAILURE;
        }

        return _nodeState;
    }
}
