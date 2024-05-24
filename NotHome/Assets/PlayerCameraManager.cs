using Mirror;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class PlayerCameraManager : NetworkBehaviour
{
    public RenderTexture[] renderTextures;

    [SerializeField] private Camera playerRenderCamera;

    private static int nextIndex = 0;
    private int index;

    public TMP_Text test;

    public override void OnStartClient()
    {
        base.OnStartClient();
        //index = connectionToClient.connectionId;

        if (isOwned)
        {
            test.gameObject.SetActive(true);
            test.text = "caca";

            index = nextIndex++;
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
