using UnityEngine;

public class DoorExit : MonoBehaviour
{
    [SerializeField] private GameObject _doorEnter;
    [SerializeField] private GameObject _doorExit;
    private int _nbPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _nbPlayer++;
            if (_nbPlayer >= 2)
            {
                _doorEnter.SetActive(true);
                _doorExit.SetActive(false);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _doorEnter.SetActive(false);
            _doorExit.SetActive(true);
            _nbPlayer--;
        }
    }
}
