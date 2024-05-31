using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [SerializeField] private Light _directionalLight;
    [SerializeField] private LightingPreset _preset;
    [SerializeField, Range(0, 24)] private float _timeOfDay;
    [SerializeField] private Material _nightSkyBox;
    [SerializeField] private Material _daySkyBox;

    private void Update()
    {
        if (_preset == null)
            return;

        if (Application.isPlaying)
        {
            _timeOfDay += Time.deltaTime;
            _timeOfDay %= 24;
        }
        UpdateLighting(_timeOfDay / 24f);
    }

    private void UpdateLighting(float _timePercent)
    {
        RenderSettings.ambientLight = _preset._ambiantColor.Evaluate(_timePercent);
        RenderSettings.fogColor = _preset._fogColor.Evaluate(_timePercent);

        if( _directionalLight != null)
        {
            _directionalLight.color = _preset._directionalColor.Evaluate(_timePercent);
            _directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((_timePercent * 360f) - 90f, -170f, 0));
            if (RenderSettings.skybox != _nightSkyBox && _timePercent > 0.8f)
                RenderSettings.skybox = _nightSkyBox;
            else if (RenderSettings.skybox != _daySkyBox && _timePercent > 0.2f && _timePercent < 0.8f)
                RenderSettings.skybox = _daySkyBox;
        }
    }

    private void OnValidate()
    {
        if (_directionalLight != null)
            return;

        if(RenderSettings.sun != null)
        {
            _directionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] _lights = FindObjectsOfType<Light>();
            for(int i = 0; i < _lights.Length; i++)
            {
                if (_lights[i].type == LightType.Directional)
                {
                    _directionalLight = _lights[i];
                    return;
                }
            }
        }
    }

}
