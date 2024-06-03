using UnityEngine;

public class BuildInterractable : MonoBehaviour
{
    public int _index;
    public GameObject usedPlayer = null;
    public bool _isOpen = false;

    public void SetUsedPlayer(GameObject usedPlayer)
    { this.usedPlayer = usedPlayer; }
}
