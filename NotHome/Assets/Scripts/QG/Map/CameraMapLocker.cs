using UnityEngine;

public class CameraMapLocker : MonoBehaviour
{
    [HideInInspector] public Vector3 _lastPosition;

    private void OnTriggerExit(Collider other)
    {
        transform.position = _lastPosition;
    }

    private void OnTriggerStay(Collider other)
    {
        _lastPosition = transform.position;
        print("dedans");
    }
}
