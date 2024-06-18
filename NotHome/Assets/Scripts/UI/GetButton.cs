using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class GetButton : MonoBehaviour
{
    [SerializeField] private List<GameObject> _listButton;
    public PlayerInput _playerInput;
    public List<string> _listControl = new();
    [SerializeField] private List<ChangeControl> _changeControls = new();
    private int _indexButton;
    public static string _text = "99";
    private void Start()
    {
        for (int i = 0; i < _changeControls.Count; i++)
        {
            _listControl.Add("");
        }
        SetListControl();
    }
    public void SetListControl()
    {
        for (int i = 0; i < _changeControls.Count; i++)
        {
            _listControl[i] = _playerInput.actions.actionMaps[0].actions[_changeControls[i]._indexAction].bindings[_changeControls[i]._indexBinding].path.ToString();
        }
    }

    private void OnEnable()
    {
        _playerInput.actions.actionMaps[0].Disable();
        _playerInput.actions.actionMaps[4].Enable();
    }
    private void OnDisable()
    {
        _playerInput.actions.actionMaps[4].Disable();
    }
    public void Any(InputAction.CallbackContext ctx)
    {
        print("enter");
        InputSystem.onAnyButtonPress.CallOnce(ctrl => _text = ctrl.name);
        if (_text.Length < 2 || _text == "space" || _text == "leftShift" || _text == "semicolon")
        {
            _listButton[_indexButton].GetComponent<ChangeControl>().Change();
        }
            
            
    }
    public void OnClick(int index)
    {
        _indexButton = index;
    }
}
