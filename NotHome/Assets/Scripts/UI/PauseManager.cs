using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _uiPause;
    [SerializeField] private GameObject _uiButton;
    [SerializeField] private GameObject _uiOption;

    

    
    public static bool _gameIsPaused;

    private void Start()
    {
        _gameIsPaused = _uiPause.activeSelf;
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
    public void Exit()
    {
        Application.Quit();
    }
    
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) 
        { 
            Resume();                           // A enlever
        }
    }
}
