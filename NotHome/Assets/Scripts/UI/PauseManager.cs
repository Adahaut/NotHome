using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _uiPause;
    [SerializeField] private GameObject _uiButton;
    [SerializeField] private GameObject _uiOption;
    [SerializeField] private GameObject _uiControl;

    [SerializeField] private PlayerInput _playerInput;
    public static bool _gameIsPaused;
    public static PauseManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _gameIsPaused = _uiPause.activeSelf;
    }
    public void Resume()
    {
        if (!_gameIsPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            _playerInput.actions.actionMaps[0].Disable();
            _uiPause.SetActive(true);
            _gameIsPaused = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            _playerInput.actions.actionMaps[0].Enable();
            _uiPause.SetActive(false);
            _uiButton.SetActive(true);
            _uiOption.SetActive(false);
            _gameIsPaused = false;
        }
    }
    public void Option()
    {
        _uiOption.SetActive(!_uiOption.activeSelf);
        _uiButton.SetActive(!_uiOption.activeSelf);
    }
    public void Control()
    {
        _uiControl.SetActive(!_uiControl.activeSelf);
        _uiOption.SetActive(!_uiControl.activeSelf);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
