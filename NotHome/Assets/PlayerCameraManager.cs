using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerCameraManager : MonoBehaviour
{
    [SerializeField] private List<RenderTexture> _cameraRenderTextures;
    [SerializeField] private Camera _renderCameraPrefab;

    public static PlayerCameraManager instance;

    int i = 0;

    private void Awake()
    {
        instance = this;
        i = 0;
    }

    public void UpdateCameras(GameObject player)
    {
        Camera mainCamera = player.GetComponentInChildren<Camera>();

        Camera renderCamera = Instantiate(_renderCameraPrefab, mainCamera.transform.position, mainCamera.transform.rotation, mainCamera.transform);
        renderCamera.targetTexture = _cameraRenderTextures[i];
        renderCamera.enabled = true;

        renderCamera.transform.SetParent(mainCamera.transform);
        i++;
    }
}
