using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class GetButton : MonoBehaviour
{
    [SerializeField] private List<GameObject> _listButton;
    private int _indexButton;
    public static string _text = "99";
    public void Any(InputAction.CallbackContext ctx)
    {
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
