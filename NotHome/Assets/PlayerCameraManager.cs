using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerCameraManager : NetworkBehaviour
{
    [SerializeField] private RenderTexture[] _cameraRenderTextures;
    public GameObject[] cameraPlanes; // Assign planes in the inspector
    private Camera[] playerCameras;

    void Start()
    {
        playerCameras = new Camera[4];

        if (isServer)
        {
            StartCoroutine(FindPlayerCameras());
        }
    }

    IEnumerator FindPlayerCameras()
    {
        while (true)
        {
            var players = FindObjectsOfType<NetworkIdentity>();
            int cameraIndex = 0;

            foreach (var player in players)
            {
                var camera = player.GetComponentInChildren<Camera>();
                if (camera != null)
                {
                    playerCameras[cameraIndex] = camera;
                    camera.targetTexture = _cameraRenderTextures[cameraIndex];
                    cameraIndex++;
                }

                if (cameraIndex >= 4) break;
            }

            if (cameraIndex >= 4) yield break;
            yield return new WaitForSeconds(1.0f);
        }
    }

    [ClientRpc]
    public void RpcUpdateCameraDisplays()
    {
        Debug.Log("kvberqzjadn");
        for (int i = 0; i < playerCameras.Length; i++)
        {
            if (playerCameras[i] != null)
            {
                cameraPlanes[i].GetComponent<Renderer>().material.mainTexture = _cameraRenderTextures[i];
            }
        }
    }

}
