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

    //private int index;

    public TMP_Text test;

    public override void OnStartClient()
    {
        if(isOwned)
            StartCoroutine(LoadCameras());
    }

    IEnumerator LoadCameras()
    {
        yield return new WaitForSeconds(2f);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<PlayerCameraManager>().playerRenderCamera.transform.parent.gameObject.SetActive(true);
            players[i].GetComponent<PlayerCameraManager>().playerRenderCamera.targetTexture = renderTextures[i];
        }
    }

    //private NetworkLobbyManager room;

    //public override void OnStartClient()
    //{
    //    base.OnStartClient();

    //    if (isOwned)
    //    {
    //        index = GameObject.Find("SpawnSystem").GetComponent<PlayerSpawnSystem>().playerCount;
    //        //index = GameObject.FindGameObjectsWithTag("Player").Length - 1;
    //        test.gameObject.SetActive(true);
    //        test.text = index.ToString();

    //        if (playerRenderCamera != null)
    //        {
    //            playerRenderCamera.targetTexture = renderTextures[index];
    //            CmdSetupCameraDisplay(index, renderTextures[index].name);
    //        }
    //    }
    //}

    //[Command]
    //private void CmdSetupCameraDisplay(int playerIndex, string renderTextureName)
    //{
    //    RpcSetupCameraDisplay(playerIndex, renderTextureName);
    //}

    //[ClientRpc]
    //private void RpcSetupCameraDisplay(int playerIndex, string renderTextureName)
    //{

    //    string planeName = "CameraPlane" + (playerIndex);
    //    GameObject cameraPlane = GameObject.Find(planeName);
    //    if (cameraPlane != null)
    //    {
    //        cameraPlane.GetComponent<Renderer>().material.mainTexture = renderTextures[playerIndex];
    //    }
    //}

}
