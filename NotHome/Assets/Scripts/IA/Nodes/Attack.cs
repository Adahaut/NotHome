using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Node
{
    EnemyAI _enemyAI;

    public Attack(EnemyAI enemyAI)
    {
        _enemyAI = enemyAI;
    }

    public override NodeState Evaluate()
    {
        _enemyAI.Attack();

        _nodeState = NodeState.SUCCESS;
        return _nodeState;
    }
}
