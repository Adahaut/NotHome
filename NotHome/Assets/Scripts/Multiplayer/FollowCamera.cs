using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Camera linkedCam;

    public Camera cam;
    Vector3 newRotation;
    private void Update()
    {
        linkedCam.transform.position = cam.transform.position;

        newRotation = new Vector3(cam.transform.rotation.x, transform.root.rotation.y, cam.transform.rotation.z);

        linkedCam.transform.rotation = Quaternion.Euler(newRotation);
    }
}
