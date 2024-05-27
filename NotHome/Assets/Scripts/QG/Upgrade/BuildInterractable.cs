using UnityEngine;

public class BuildInterractable : MonoBehaviour
{
    public int _index;
    public GameObject usedPlayer = null;

    public void SetUsedPlayer(GameObject usedPlayer)
    { this.usedPlayer = usedPlayer; }
}
