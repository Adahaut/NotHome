using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _uiOption;
    [SerializeField] private GameObject _uiCredit;
    [SerializeField] private GameObject _uiMainMenu;
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
}
