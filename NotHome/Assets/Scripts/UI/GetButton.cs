using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class GetButton : MonoBehaviour
{
    [SerializeField] private List<GameObject> _listButton;
    [SerializeField] private UnityEngine.InputSystem.PlayerInput _playerInput;
    private int _indexButton;
    public static string _text = "99";

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
