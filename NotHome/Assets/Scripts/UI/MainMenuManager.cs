using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _uiOption;
    [SerializeField] private GameObject _uiCredit;
    [SerializeField] private GameObject _uiMainMenu;
    [SerializeField] private GameObject _camera;

    private float _camY = 0;
    public void PlayGame()
    {
        SceneManager.LoadScene("FixGame");
    }
    public void Option()
    {
        _uiOption.SetActive(!_uiOption.activeSelf);
        _uiMainMenu.SetActive(!_uiOption.activeSelf);
    }
    public void Credit()
    {
        _uiCredit.SetActive(!_uiCredit.activeSelf);
        _uiMainMenu.SetActive(!_uiCredit.activeSelf);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void rotatecam()
    {
        _camY += 0.01f;
        _camera.transform.rotation = Quaternion.Euler(0, 0, _camY);
    }

    private void Update()
    {
        rotatecam();
    }
}
