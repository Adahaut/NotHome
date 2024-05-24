using Mirror;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCameraManager : NetworkBehaviour
{
    public RenderTexture[] renderTextures;

    [SerializeField] private Camera playerRenderCamera;


    //private int index;

    private static int nextPlayerId = 0;

    [SyncVar(hook = nameof(OnPlayerIdChanged))]
    private int playerId = -1; // Initialize to -1 to detect uninitialized state

    public TMP_Text test;

    private NetworkLobbyManager room;

    public override void OnStartClient()
    {
        //base.OnStartClient();

        //if (isOwned)
        //{
        //    test.gameObject.SetActive(true);
        //    test.text = netId.ToString();

        //    if (playerRenderCamera != null)
        //    {
        //        playerRenderCamera.targetTexture = renderTextures[index];
        //        CmdSetupCameraDisplay(index, renderTextures[index].name);
        //    }
        //}

        base.OnStartClient();

        if (isOwned)
        {
            CmdRequestPlayerId();
        }
    }

    [Command]
    private void CmdRequestPlayerId(NetworkConnectionToClient sender = null)
    {
        playerId = nextPlayerId++;
    }

    private void OnPlayerIdChanged(int oldPlayerId, int newPlayerId)
    {
        if (isOwned)
        {
            test.gameObject.SetActive(true);
            test.text = newPlayerId.ToString();
            if (playerRenderCamera != null)
            {
                playerRenderCamera.targetTexture = renderTextures[newPlayerId];
                CmdSetupCameraDisplay(newPlayerId, renderTextures[newPlayerId].name);
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
