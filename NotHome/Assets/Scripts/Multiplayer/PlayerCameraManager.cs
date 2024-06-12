using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraManager : NetworkBehaviour
{
    public List<RenderTexture> renderTextures = new List<RenderTexture>();
    public List<GameObject> screens = new List<GameObject> ();

    public Camera playerRenderCamera;
    [HideInInspector] [SyncVar] public int screenIndex;

    private void Start()
    {
        for (int i = 0; i < renderTextures.Count; i++)
        {
            screens.Add(GameObject.Find("CameraPlane" + i.ToString()));
        }
    }
    public override void OnStartClient()
    {
        if (isOwned)
        {
            GameObject.FindAnyObjectByType<PlayerCameraControlTower>().AddScreen(screenIndex);
            StartCoroutine(LoadCameras());
        }
    }

    IEnumerator LoadCameras()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<PlayerCameraManager>().playerRenderCamera.targetTexture = renderTextures[i];
            //players[i].GetComponent<PlayerCameraManager>().screenIndex = i;
            screens[i].transform.GetChild(0).gameObject.SetActive(false);
        }
    }

}
