using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCameraControlTower : NetworkBehaviour
{
    public List<GameObject> screens = new List<GameObject>();

    private void Start()
    {
        //en afficher que 3
        //Afficher no signal sur les autres
        StartCoroutine(SetupScreens());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            screens[other.GetComponent<PlayerCameraManager>().screenIndex].transform.GetChild(0).gameObject.SetActive(true);
            //other.gameObject.GetComponent<PlayerCameraManager>().screen.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            screens[other.GetComponent<PlayerCameraManager>().screenIndex].transform.GetChild(0).gameObject.SetActive(false);
            //other.gameObject.GetComponent<PlayerCameraManager>().screen.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    IEnumerator SetupScreens()
    {
        yield return new WaitForSeconds(1f);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++)
        {
            //gameObject.GetComponent<PlayerCameraManager>().screen.transform.GetChild(0).gameObject.SetActive(false);
            if (players[i].GetComponent<PlayerCameraManager>().screenIndex != -1)
            {
                screens[players[i].GetComponent<PlayerCameraManager>().screenIndex].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
}
