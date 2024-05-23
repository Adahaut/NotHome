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

    //[SerializeField] private Camera _renderCameraPrefab;

    //private Camera mainCamera;
    //private Camera renderCamera;

    //public override void OnStartClient()
    //{
    //    mainCamera = GetComponentInChildren<Camera>();

    //    renderCamera = Instantiate(_renderCameraPrefab, mainCamera.transform.position, mainCamera.transform.rotation, mainCamera.transform);
    //    renderCamera.targetTexture = _cameraRenderTextures[connectionToClient.connectionId];
    //    renderCamera.enabled = true;

    //    renderCamera.transform.SetParent(mainCamera.transform);
    //}

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        StartCoroutine(begin());
    }

    public void AddPlayer(GameObject player)
    {
        _players.Add(player);
    }

    IEnumerator begin()
    {
        yield return new WaitForSeconds(2);
        for (int i = 0; i < _players.Count; i++)
        {
            _players[i].GetComponent<ProximityVoiceChat>().test.text = "working";
            _players[i].GetComponent<PlayerNetwork>()._renderCamera.targetTexture = _cameraRenderTextures[i];
        }
    }
}
