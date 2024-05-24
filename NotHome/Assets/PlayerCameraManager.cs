using Mirror;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerCameraManager : NetworkBehaviour
{
    public RenderTexture[] renderTextures;

    [SerializeField] private Camera playerCamera;

    private static int nextIndex = 0;
    private int index;
    public override void OnStartClient()
    {
        base.OnStartClient();
        index = connectionToClient.connectionId;
        if(isOwned)
        {
            index = nextIndex++;
            if (playerCamera != null)
            {
                playerCamera.targetTexture = renderTextures[index];
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
