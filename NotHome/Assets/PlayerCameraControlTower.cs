using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCameraControlTower : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerCameraManager>().screen.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerCameraManager>().screen.SetActive(true);
        }
    }
}
