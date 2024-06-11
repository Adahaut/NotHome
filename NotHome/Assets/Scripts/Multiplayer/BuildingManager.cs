using Mirror;
using UnityEngine;

public class BuildingManager : NetworkBehaviour
{
    public static BuildingManager instance;

    public GameObject[] buildings;
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        foreach (GameObject b in buildings)
        {
            NetworkServer.Spawn(b);
        }
    }

    public void AssignAuthority(NetworkConnectionToClient conn, GameObject b)
    {
        b.GetComponent<NetworkIdentity>().AssignClientAuthority(conn);
    }

}

