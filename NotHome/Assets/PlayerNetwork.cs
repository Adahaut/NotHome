using Mirror;
using System.Globalization;
using TMPro;

public class PlayerNetwork : NetworkBehaviour
{
    public TMP_Text _nameHoverHeadText;

    private string _displayName;

    public void SetDisplayName(string name)
    {
        _displayName = name;
        _nameHoverHeadText.text = _displayName;
    }

}
