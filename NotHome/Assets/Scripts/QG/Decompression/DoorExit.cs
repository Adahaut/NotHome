using UnityEngine;

public class DoorExit : MonoBehaviour
{
    [SerializeField] private GameObject _doorEnter;
    [SerializeField] private GameObject _doorExit;
    private int _nbPlayer;
    public static DoorExit Instance;
    private bool _qgIsLevel3;
    [SerializeField] private ParticleSystem _smokeParticle;
    [SerializeField] private AudioSource _soundDecompression;
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
            print("enter");
            _nbPlayer++;
            if (_nbPlayer >= 1)
            {
                _soundDecompression.Play();
                _smokeParticle.Play();
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
