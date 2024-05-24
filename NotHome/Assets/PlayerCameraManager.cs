using Mirror;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCameraManager : NetworkBehaviour
{
    public RenderTexture[] renderTextures;

    [SerializeField] private Camera playerRenderCamera;

    private static int nextIndex = 0;
    private int index;

    public TMP_Text test;

    private NetworkLobbyManager room;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if(GameObject.FindGameObjectsWithTag("Player").Length == 1)
            index = connectionToClient.connectionId;

        if (isOwned)
        {
            index = nextIndex++;

            test.gameObject.SetActive(true);
            test.text = index.ToString();

            if (playerRenderCamera != null)
            {
                playerRenderCamera.targetTexture = renderTextures[index];
                CmdSetupCameraDisplay(index, renderTextures[index].name);
            }
        }
    }

    [Command]
    private void CmdSetupCameraDisplay(int playerIndex, string renderTextureName)
    {
        RpcSetupCameraDisplay(playerIndex, renderTextureName);
    }

    [ClientRpc]
    private void RpcSetupCameraDisplay(int playerIndex, string renderTextureName)
    {

        string planeName = "CameraPlane" + (playerIndex);
        GameObject cameraPlane = GameObject.Find(planeName);
        if (cameraPlane != null)
        {
            cameraPlane.GetComponent<Renderer>().material.mainTexture = renderTextures[playerIndex];
        }
    }

}
