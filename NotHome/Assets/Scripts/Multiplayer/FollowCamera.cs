using Mirror;
using UnityEngine;

public class FollowCamera : NetworkBehaviour
{
    public Camera linkedCam;

    public Camera cam;

    [SyncVar(hook = nameof(OnPositionChanged))]
    private Vector3 syncPosition;

    [SyncVar(hook = nameof(OnRotationChanged))]
    private Quaternion syncRotation;

    private void Update()
    {
        if (isOwned && cam.gameObject.activeSelf)
        {
            linkedCam.transform.position = cam.transform.position;
            linkedCam.transform.rotation = cam.transform.rotation;

            CmdModifyPositionRotation(cam.transform.position, cam.transform.rotation);
        }
    }

    [Command]
    void CmdModifyPositionRotation(Vector3 position, Quaternion rotation)
    {
        syncPosition = position;
        syncRotation = rotation;
    }

    private void OnPositionChanged(Vector3 oldPosition, Vector3 newPosition)
    {
        linkedCam.transform.position = newPosition;
    }

    private void OnRotationChanged(Quaternion oldRotation, Quaternion newRotation)
    {
        linkedCam.transform.rotation = newRotation;
    }
}