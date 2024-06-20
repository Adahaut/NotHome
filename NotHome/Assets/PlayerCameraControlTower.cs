using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraControlTower : NetworkBehaviour
{
    public List<GameObject> screens = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            screens[other.GetComponent<PlayerCameraManager>().screenIndex].transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            screens[other.GetComponent<PlayerCameraManager>().screenIndex].transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
