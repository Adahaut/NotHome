using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class NoFog : MonoBehaviour
{
    public List<Camera> _cameraWithoutFog = new();

    private void Start()
    {
        RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
    }
    void OnDestroy()
    {
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
    }
    void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        if (_cameraWithoutFog.Contains(camera)) 
            RenderSettings.fog = false;
        else
            RenderSettings.fog = true;
    }
}