using Mirror;
using TMPro;

public class PlayerNetwork : NetworkBehaviour
{
    public TMP_Text nameHoverHeadText;

    private string _displayName;

    public void SetDisplayName(string name)
    {
        _displayName = name;
        nameHoverHeadText.text = _displayName;
    }
}
