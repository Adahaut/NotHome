using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [SerializeField] private Light _directionalLight;
    [SerializeField] private LightingPreset _preset;
    [SerializeField, Range(0, 24)] private float _timeOfDay;
    [SerializeField] private Material _nightSkyBox;
    [SerializeField] private Material _daySkyBox;

    public float _dayDurration;
    private float _factor;

    private void Awake()
    {
        ConvertMinuteToTime();
    }

    private void ConvertMinuteToTime()
    {
        _factor = _dayDurration / 3600f;
    }

    private void Update()
    {
        if (_preset == null)
            return;

        if (Application.isPlaying)
        {
            _timeOfDay += Time.deltaTime * _factor;
            _timeOfDay %= 24;
        }
        UpdateLighting(_timeOfDay / 24f);
    }

    private void UpdateLighting(float _timePercent)
    {
        if (_directionalLight != null)
        {
            _directionalLight.transform.localRotation = Quaternion.Euler(new Vector3(0, (_timePercent * 360f) - 90f, 0));
        }
    }

    //private void OnValidate()
    //{
    //    if (_directionalLight != null)
    //        return;

    //    if (RenderSettings.sun != null)
    //    {
    //        _directionalLight = RenderSettings.sun;
    //    }
    //    else
    //    {
    //        Light[] _lights = FindObjectsOfType<Light>();
    //        for (int i = 0; i < _lights.Length; i++)
    //        {
    //            if (_lights[i].type == LightType.Directional)
    //            {
    //                _directionalLight = _lights[i];
    //                return;
    //            }
    //        }
    //    }
    //}

}
