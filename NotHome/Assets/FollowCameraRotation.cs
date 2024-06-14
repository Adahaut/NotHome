using Mirror;
using UnityEngine;

public class FollowCameraRotation : NetworkBehaviour
{
    [SerializeField] private Camera _mainCamera;

    void Update()
    {
        transform.rotation = _mainCamera.transform.rotation;
    }
}
