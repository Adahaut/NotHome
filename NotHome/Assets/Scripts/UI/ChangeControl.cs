using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.EventSystems;

public class ChangeControl : MonoBehaviour
{
    private string _control = "";
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private int _indexAction;
    [SerializeField] private int _indexBinding;
    private void Start()
    {
        _control = _playerInput.actions.actionMaps[0].actions[_indexAction].bindings[_indexBinding].path.ToString();
        _control = _control[11..];
        ChangeQwerty();
        GetComponentInChildren<TextMeshProUGUI>().text = _control.ToUpper();
    }
    public void Change()
    {
        _control = GetButton._text;
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            _playerInput.actions.actionMaps[0].actions[_indexAction].ChangeBinding(_indexBinding).WithPath("<Keyboard>/" + _control);
            ChangeQwerty();
            GetComponentInChildren<TextMeshProUGUI>().text = _control.ToUpper();
        }
    }
    private void ChangeQwerty()
    {
        if (_control == "q")
            _control = "a";
        else if (_control == "a")
            _control = "q";
        if (_control == "w")
            _control = "z";
        else if (_control == "z")
            _control = "w";
        if (_control == "semicolon")
            _control = "m";
    }
}
