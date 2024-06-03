using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject _uiPause;
    [SerializeField] private GameObject _uiOption;
    [SerializeField] private Toggle _toggleFullScreen;
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private TMP_Dropdown _dropdownResolution;
    private Resolution[] _resolutions;
    public static bool _gameIsPaused;

    private void Start()
    {
        
        _gameIsPaused = _uiPause.activeSelf;
        _toggleFullScreen.isOn = Screen.fullScreen;

        GetResolution();
    }
    public void Resume()
    {
        if (!_gameIsPaused)
        {
            _uiPause.SetActive(true);
            _gameIsPaused = true;
        }
        else
        {
            _uiPause.SetActive(false);
            _gameIsPaused = false;
        }
    }
    public void Option()
    {
        _uiOption.SetActive(!_uiOption.activeSelf);
    }
    public void Exit()
    {
        Application.Quit();
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
        _resolutions = Screen.resolutions.Select(_resolutions => new Resolution { width = _resolutions.width, height = _resolutions.height}).Distinct().ToArray();
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
}
