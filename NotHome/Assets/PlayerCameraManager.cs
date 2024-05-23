using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerCameraManager : NetworkBehaviour
{
    [SerializeField] private List<RenderTexture> _cameraRenderTextures;
    [SerializeField] private Camera _renderCameraPrefab;

    private Camera mainCamera;
    private Camera renderCamera;

    public override void OnStartClient()
    {
        base.OnStartClient();

        mainCamera = GetComponentInChildren<Camera>();

        renderCamera = Instantiate(_renderCameraPrefab, mainCamera.transform.position, mainCamera.transform.rotation, mainCamera.transform);
        int index = connectionToClient.connectionId % _cameraRenderTextures.Count;
        renderCamera.targetTexture = _cameraRenderTextures[index];
        renderCamera.enabled = true;

        renderCamera.transform.SetParent(mainCamera.transform);
    }

    private void Update()
    {
        if (!isOwned)
        {
            return;
        }

        if (renderCamera != null)
        {
            renderCamera.transform.position = mainCamera.transform.position;
            renderCamera.transform.rotation = mainCamera.transform.rotation;
        }
    }
}
