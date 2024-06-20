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

        print(cam.transform.rotation);
        linkedCam.transform.rotation = cam.transform.rotation;
    }
}
