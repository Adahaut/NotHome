using Mirror;
using System.Globalization;
using TMPro;

public class PlayerNetwork : NetworkBehaviour
{
    public TMP_Text nameHoverHeadText;

    private string _displayName;

    public void SetDisplayName(string name)
    {
        _displayName = name;
        
    }

    private void Update()
    {
        nameHoverHeadText.text = _displayName;

        if(!isOwned)
        {
            nameHoverHeadText.text = _displayName;
        }
    }

}
