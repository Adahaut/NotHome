using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    [Header("Option")]
    [SerializeField] private Toggle _toggleFullScreen;
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private TMP_Dropdown _dropdownResolution;
    private Resolution[] _resolutions;
    private Transform _transform;
    private void Start()
    {
        _transform = transform;
        _toggleFullScreen.isOn = Screen.fullScreen;
        GetResolution();
    }
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
    public void SetVolumeSound(float volume)
    {
        _audioMixer.SetFloat("SFX", volume);
    }
    public void SetVolumeMusic(float volume)
    {
        _audioMixer.SetFloat("Music", volume);
    }
    private void GetResolution()
    {
        _resolutions = Screen.resolutions.Select(_resolutions => new Resolution { width = _resolutions.width, height = _resolutions.height }).Distinct().ToArray();
        _dropdownResolution.ClearOptions();
        List<string> options = new();
        int currentResolution = 0;
        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + "x" + _resolutions[i].height;
            options.Add(option);
            if (_resolutions[i].width == Screen.width && _resolutions[i].height == Screen.height)
            {
                currentResolution = i;
            }
        }
        _dropdownResolution.AddOptions(options);
        _dropdownResolution.value = currentResolution;
        _dropdownResolution.RefreshShownValue();
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetSensitivity(float sensitivity)
    {
        GetComponentInParent<PlayerController>()._sensitivity = sensitivity;
        print(sensitivity);
    }
}
