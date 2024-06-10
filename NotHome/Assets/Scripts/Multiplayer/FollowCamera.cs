using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Camera linkedCam;

    [SerializeField] private Camera cam;
    private void Update()
    {
        linkedCam.transform.position = cam.transform.position;
        linkedCam.transform.rotation = cam.transform.rotation;
    }
}
