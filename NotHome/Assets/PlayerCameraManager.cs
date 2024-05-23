using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerCameraManager : NetworkBehaviour
{
    [SerializeField] private List<RenderTexture> _cameraRenderTextures;
    public static PlayerCameraManager instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if(isServer)
        {
            StartCoroutine(InitializePlayers());
        }
    }

    private IEnumerator<WaitForSeconds> InitializePlayers()
    {
        yield return new WaitForSeconds(1f);

        PlayerNetwork[] players = FindObjectsOfType<PlayerNetwork>();
        for (int i = 0; i < players.Length; i++)
        {
            Camera playerCamera = players[i]._renderCamera;
            if (playerCamera != null)
            {
                playerCamera.targetTexture = _cameraRenderTextures[i];
            }
            else
            {
                playerCamera.gameObject.GetComponent<ProximityVoiceChat>().test.text = "Player cam = null";
            }
        }
    }

    }
