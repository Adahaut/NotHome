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
    [HideInInspector] [SyncVar] public int screenIndex;

    private void Start()
    {
        screenIndex = -1;
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
            players[i].GetComponent<PlayerCameraManager>().screenIndex = i;
            //screens[i].transform.GetChild(0).gameObject.SetActive(false);
        }
    }

}
