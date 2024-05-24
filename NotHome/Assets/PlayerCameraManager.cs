using Mirror;
using TMPro;
using UnityEngine;

public class PlayerCameraManager : NetworkBehaviour
{
    public RenderTexture[] renderTextures;

    [SerializeField] private Camera playerRenderCamera;
    public TMP_Text test;

    private static int playerCount = 0; // Compte des joueurs
    private int playerIndex;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (isOwned)
        {
            playerIndex = playerCount++;
            test.gameObject.SetActive(true);
            test.text = playerIndex.ToString();

            if (playerRenderCamera != null)
            {
                playerRenderCamera.targetTexture = renderTextures[playerIndex];
                CmdSetupCameraDisplay(playerIndex, renderTextures[playerIndex].name);
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
        string planeName = "CameraPlane" + playerIndex;
        GameObject cameraPlane = GameObject.Find(planeName);
        if (cameraPlane != null)
        {
            cameraPlane.GetComponent<Renderer>().material.mainTexture = renderTextures[playerIndex];
        }
    }
}