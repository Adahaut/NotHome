using Mirror;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCameraManager : NetworkBehaviour
{
    public RenderTexture[] renderTextures;

    public Camera playerRenderCamera;

    public TMP_Text test;

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
        }
    }

}
