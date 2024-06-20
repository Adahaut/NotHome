using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveDesactiveCacCollider : MonoBehaviour
{
    public Collider _enemyDetectionCollider;

    public void ActiveCollider()
    {
        _enemyDetectionCollider.enabled = true;
    }

    public void DisableCollider()
    {
        _enemyDetectionCollider.enabled = false;
    }
}
