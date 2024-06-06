using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button _loadSaveButton;
    [SerializeField] private Button _clearSavedDataButton;
    public static bool _loadSavedData;

    private void Start()
    {
        bool _saveFileExist = System.IO.File.Exists(Application.persistentDataPath + "/SavedData.json");
        _loadSaveButton.interactable = _saveFileExist;
        _clearSavedDataButton.interactable = _saveFileExist;
    }

    public void NewGameButton()
    {
        _loadSavedData = false;
        //SceneManager.LoadScene("");
    }

    public void LoadGameButton()
    {
        _loadSavedData = true;
        //SceneManager.LoadScene("");
    }    

    public void ClearSavedData()
    {
        System.IO.File.Delete(Application.persistentDataPath + "/SavedData.json");
        _loadSaveButton.interactable = false;
    }
}
