using System.Collections;
using UnityEngine;

public class SetAnimationMachet : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public IEnumerator StartAnimMachet()
    {
        print("enter");
        int number = Random.Range(1, 3);
        _animator.SetInteger("Attack", number);
        yield return new WaitForEndOfFrame();
        _animator.SetInteger("Attack", 0);
    }
}
