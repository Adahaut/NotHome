using System.Collections;
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
    public void TpLadder(Transform camera, float distRayCast, PlayerController playerController)
    {
        if (Physics.Raycast(camera.position, camera.forward, out RaycastHit hit, distRayCast) && hit.collider.CompareTag("Ladder"))
        {
            if (Vector3.Distance(camera.position + Vector3.down, hit.collider.GetComponentInParent<Ladder>()._posHigh.position) > Vector3.Distance(camera.position + Vector3.down, hit.collider.GetComponentInParent<Ladder>()._posDown.position))
            {
                StartCoroutine(CharacterMove(1, hit.collider.GetComponentInParent<Ladder>()._posHigh, playerController.transform));
            } 
            else
            {
                StartCoroutine(CharacterMove(1, hit.collider.GetComponentInParent<Ladder>()._posDown, playerController.transform));
            }
        }
    }
    private IEnumerator CharacterMove(float total_time,Transform endPos, Transform player)
    {
        float speed = 20;
        float time = 0f;
        Vector3 end_pos = endPos.position;
        Vector3 start_pos = player.position;
        while (time / total_time < 1)
        {
                time += Time.deltaTime * speed;
                player.transform.position = Vector3.Lerp(start_pos, end_pos, time / total_time);
                yield return null;
        }
    }
}
