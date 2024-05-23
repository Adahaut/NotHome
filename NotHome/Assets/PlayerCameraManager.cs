using Mirror;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerCameraManager : NetworkBehaviour
{
    public RenderTexture[] renderTextures;

    [SerializeField] private Camera playerCamera;

    int index;
    public override void OnStartClient()
    {
        base.OnStartClient();

        index = connectionToClient.connectionId;

        if (playerCamera != null)
        {
            playerCamera.targetTexture = renderTextures[index];

            CmdSetupCameraDisplay(netId, renderTextures[index].name);
        }
    }

    [Command]
    private void CmdSetupCameraDisplay(uint playerId, string renderTextureName)
    {
        RpcSetupCameraDisplay(playerId, renderTextureName);
    }

    [ClientRpc]
    private void RpcSetupCameraDisplay(uint playerId, string renderTextureName)
    {

        string planeName = "CameraPlane" + (playerId);
        GameObject cameraPlane = GameObject.Find(planeName);
        if (cameraPlane != null)
        {
            cameraPlane.GetComponent<Renderer>().material.mainTexture = Resources.Load<RenderTexture>(renderTextureName);
        }
    }

}
