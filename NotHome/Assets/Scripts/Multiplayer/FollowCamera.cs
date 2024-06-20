using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : NetworkBehaviour
{
    public Camera linkedCam;

    public Camera cam;


    private void Update()
    {
        if(cam.gameObject.activeSelf && isOwned)
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
    }
}
