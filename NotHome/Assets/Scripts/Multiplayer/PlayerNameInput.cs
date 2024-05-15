using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameInput : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_InputField _nameInputField;
    [SerializeField] private Button _continueButton;

    public static string DisplayName { get; private set; }

    private const string PlayerPrefNameKey = "PlayerName";

    private void Start()
    {
        SetUpInputField();
    }

    private void SetUpInputField()
    {
        if(!PlayerPrefs.HasKey(PlayerPrefNameKey)) { return; }

        string defaultName = PlayerPrefs.GetString(PlayerPrefNameKey);

        _nameInputField.text = defaultName;

        SetPlayerName(defaultName);
    }

    public void SetPlayerName(string name)
    {
        if(name == "----")
        {
            _continueButton.interactable = !string.IsNullOrEmpty(_nameInputField.text);
        }
        else
        {
            _continueButton.interactable = !string.IsNullOrEmpty(name);
        }
        
    }

    public void SavePlayerName()
    {
        DisplayName = _nameInputField.text;

        PlayerPrefs.SetString(PlayerPrefNameKey, DisplayName);
    }
}
