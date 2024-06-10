using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCameraManager : NetworkBehaviour
{
    public RenderTexture[] renderTextures;
    public List<GameObject> screens = new List<GameObject> ();

    public Camera playerRenderCamera;
    [HideInInspector] public GameObject screen;

    private void Start()
    {
        for (int i = 0; i < renderTextures.Length; i++)
        {
            screens.Add(GameObject.Find("CameraPlane" + i.ToString()));
        }
    }
    public override void OnStartClient()
    {
        if(isOwned)
            StartCoroutine(LoadCameras());
    }

    IEnumerator LoadCameras()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<PlayerCameraManager>().playerRenderCamera.targetTexture = renderTextures[i];
            players[i].GetComponent<PlayerCameraManager>().screen = screens[i];
        }
    }

}
