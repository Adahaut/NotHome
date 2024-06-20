using Mirror;
using UnityEngine;

public class FollowCamera : NetworkBehaviour
{
    public Camera linkedCam;

    public Camera cam;


    private void Update()
    {
        if (cam.gameObject.activeSelf && isOwned)
        {
            linkedCam.transform.position = cam.transform.position;
            linkedCam.transform.rotation = cam.transform.rotation;

            CmdModifyPositionRotation();
        }

    }

    [Command]
    void CmdModifyPositionRotation()
    {
        linkedCam.transform.position = cam.transform.position;
        linkedCam.transform.rotation = cam.transform.rotation;
        RpcModify(cam, cam.transform.position, cam.transform.rotation);
    }

    [ClientRpc]
    void RpcModify(Camera cam, Vector3 position, Quaternion rotation)
    {
        cam.transform.position = position;
        cam.transform.rotation = rotation;
    }
}
