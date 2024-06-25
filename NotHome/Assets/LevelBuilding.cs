using Mirror;
using UnityEngine;

public class LevelBuilding : NetworkBehaviour
{
    [SyncVar] public int _levelBuilding = 1;
    public ParticleSystem _particleSystem;
}
