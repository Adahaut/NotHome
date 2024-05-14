using UnityEngine;
using Mirror;

public class TEMPORARY_Player : NetworkBehaviour
{

    [SerializeField] private Vector3 _movement = new Vector3();

    private void Start()
    {
        Screen.fullScreen = false;
    }

    [Client]
    void Update()
    {
        if(!isOwned) { return; }

        if(!Input.GetKeyDown(KeyCode.Space)) { return; }

        transform.Translate(_movement);
    }
}
