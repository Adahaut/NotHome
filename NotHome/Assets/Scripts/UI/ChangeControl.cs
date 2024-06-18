using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ChangeControl : MonoBehaviour
{
    public string _control = "";
    private PlayerInput _playerInput;
    public int _indexAction;
    public int _indexBinding;
    private void Start()
    {
        _playerInput = GetComponentInParent<GetButton>()._playerInput;
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
            List<string> list = GetComponentInParent<GetButton>()._listControl;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == "<Keyboard>/" + _control) 
                    return;
            }
            _playerInput.actions.actionMaps[0].actions[_indexAction].ChangeBinding(_indexBinding).WithPath("<Keyboard>/" + _control);
            ChangeQwerty();
            GetComponentInChildren<TextMeshProUGUI>().text = _control.ToUpper();
            GetComponentInParent<GetButton>().SetListControl();
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
