using UnityEngine;
using Mirror;

public class CommandTest : NetworkBehaviour
{
    private void Update()
    {
        if (isOwned && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Calling CmdTest");
            CmdTest();
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdTest()
    {
        Debug.Log("CmdTest called on server");
    }
}
