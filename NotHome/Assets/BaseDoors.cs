using Mirror;
using UnityEngine;

public class BaseDoors : NetworkBehaviour
{
    [SyncVar] private int currentPlayerCount;
    private Animator doorAnimator;
    [SerializeField] private AudioClip doorAudio;

    private void Start()
    {
        doorAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            currentPlayerCount++;
            if(!doorAnimator.GetBool("Open"))
            {
                doorAnimator.SetBool("Open", true);
                AudioSource.PlayClipAtPoint(doorAudio, transform.position, 0.2f);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentPlayerCount--;
            if(currentPlayerCount <= 0)
            {
                currentPlayerCount = 0;
                doorAnimator.SetBool("Open", false);
                AudioSource.PlayClipAtPoint(doorAudio, transform.position, 0.2f);
            }
        }
    }
}
