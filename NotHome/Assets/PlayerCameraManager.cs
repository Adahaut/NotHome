using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerCameraManager : NetworkBehaviour
{
    [SerializeField] private List<RenderTexture> _cameraRenderTextures;
    private List<GameObject> _players = new(4);
    public static PlayerCameraManager instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }


    public void AddPlayer(GameObject player, int index)
    {
        _players.Add(player);
        player.GetComponent<ProximityVoiceChat>().test.text = index.ToString();
        player.GetComponent<PlayerNetwork>()._renderCamera.targetTexture = _cameraRenderTextures[index];
    }
}
