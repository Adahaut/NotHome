using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SeePlayer : Node
{
    private GameObject _playerRef;
    private float _sightRange;
    private NavMeshAgent _agent;

    public SeePlayer(GameObject playerRef, float sightRange, NavMeshAgent agent)
    {
        _playerRef = playerRef;
        _sightRange = sightRange;
        _agent = agent;
    }

    public override NodeState Evaluate()
    {
        float distanceToPlayer = Vector3.Distance(_agent.transform.position, _playerRef.transform.position);

        //check if player is in sight range
        if (distanceToPlayer <= _sightRange)
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
