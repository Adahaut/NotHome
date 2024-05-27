using UnityEngine;

public class DoorExit : MonoBehaviour
{
    [SerializeField] private GameObject _doorEnter;
    [SerializeField] private GameObject _doorExit;
    private int _nbPlayer;
    public static DoorExit Instance;
    private bool _qgIsLevel3;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_qgIsLevel3)
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
        if (other.CompareTag("Player") && !_qgIsLevel3)
        {
            _doorEnter.SetActive(false);
            _doorExit.SetActive(true);
            _nbPlayer--;
        }
    }
    public void QGLevel3()
    {
        _qgIsLevel3 = true;
        _doorEnter.SetActive(false);
        _doorExit.SetActive(false);
    }
}
