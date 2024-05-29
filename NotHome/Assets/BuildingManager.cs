using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : NetworkBehaviour
{
    public NetworkBuilding[] buildings;

    public static BuildingManager instance;

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
        foreach (NetworkBuilding b in buildings)
        {
            NetworkServer.Spawn(b.gameObject);
        }
    }

    public void AssignAuthority(NetworkConnectionToClient conn)
    {
        foreach (NetworkBuilding b in buildings)
        {
            b.GetComponent<NetworkIdentity>().AssignClientAuthority(conn);
        }
    }

}

