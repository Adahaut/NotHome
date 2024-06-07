using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCameraControlTower : NetworkBehaviour
{

    public RenderTexture[] renderTextures;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //other.gameObject.GetComponentInChildren<FollowCamera>().linkedCam.enabled = false;
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != other.gameObject)
                    players[i].GetComponent<PlayerCameraManager>().playerRenderCamera.targetTexture = renderTextures[i];
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != other.gameObject)
                    players[i].GetComponent<PlayerCameraManager>().playerRenderCamera.targetTexture = null;
            }
        }
    }
}
