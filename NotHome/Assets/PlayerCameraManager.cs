using Mirror;
using UnityEngine;

public class PlayerCameraManager : NetworkBehaviour
{
    public RenderTexture[] renderTextures;

    [SerializeField] private Camera playerCamera;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (playerCamera != null)
        {
            playerCamera.targetTexture = renderTextures[netId % 4];

            CmdSetupCameraDisplay(netId, renderTextures[netId % 4].name);
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
        
        string planeName = "CameraPlane" + (playerId % 4 - 1);
        GameObject cameraPlane = GameObject.Find(planeName);
        if (cameraPlane != null)
        {
            cameraPlane.GetComponent<Renderer>().material.mainTexture = Resources.Load<RenderTexture>(renderTextureName);
        }
    }

}
