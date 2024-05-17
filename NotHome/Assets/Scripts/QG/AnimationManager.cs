using System.Collections;
using UnityEditor.Animations;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] private Animator _doorAnimation;

    [SerializeField] private AnimatorController _closeDoor;
    [SerializeField] private AnimatorController _openDoor;
    private float _timerAnim = 1.0f;
    private bool _canStartAnim = true;
    [SerializeField] private float _distRayCast;
    [SerializeField] private GameObject _camera;
    [HideInInspector] public bool _doorIsOpen;

    public static AnimationManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    public void OpenDoor()
    {
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, _distRayCast) && hit.collider.CompareTag("Door"))
        {
            if (_canStartAnim && _doorAnimation.runtimeAnimatorController != _openDoor)
            {
                _doorIsOpen = true;
                _doorAnimation.runtimeAnimatorController = _openDoor;
                StartCoroutine(WaitAnim());
            }
        }
    }
    public void CloseDoor()
    {
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, _distRayCast) && hit.collider.CompareTag("Door"))
        {
            if (_canStartAnim && _doorAnimation.runtimeAnimatorController != _closeDoor)
            {
                _doorIsOpen = false;
                _doorAnimation.runtimeAnimatorController = _closeDoor;
                StartCoroutine(WaitAnim());
            }
        }
    }
    IEnumerator WaitAnim()
    {
        _canStartAnim = false;
        yield return new WaitForSeconds(_timerAnim);
        _canStartAnim = true;
    }
}
