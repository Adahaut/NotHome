using UnityEngine;
using UnityEngine.AI;

public class ChasePlayer : Node
{
    private NavMeshAgent _agent;
    private GameObject _playerRef;

    public ChasePlayer(NavMeshAgent agent, GameObject playerRef)
    {
        _agent = agent;
        _playerRef = playerRef;
    }

    public override NodeState Evaluate()
    {
        _agent.SetDestination(_playerRef.transform.position);

        _nodeState = NodeState.SUCCESS;
        return _nodeState;
    }
}
