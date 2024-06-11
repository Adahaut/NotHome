using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField] private Transform _posHigh;
    [SerializeField] private Transform _posDown;
    
    public static Ladder Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void TpLadder(Transform camera, float distRayCast, GameObject player)
    {
        if (Physics.Raycast(camera.position, camera.forward, out RaycastHit hit, distRayCast) && hit.collider.CompareTag("Ladder"))
        {
            if (Vector3.Distance(camera.position, _posHigh.position) < Vector3.Distance(camera.position, _posDown.position))
            {
                player.transform.position = _posDown.position;
            } 
            else
            {
                player.transform.position = _posHigh.position;
            }
        }
    }
}
