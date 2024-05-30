using System.Collections;
using UnityEngine;

public class DoorExit : MonoBehaviour
{
    [SerializeField] private GameObject _doorEnter;
    [SerializeField] private GameObject _doorExit;
    private int _nbPlayer;
    public static DoorExit Instance;
    private bool _qgIsLevel3;
    private bool _isDecompression;
    [SerializeField] private GameObject _smokeParticle;
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
            _nbPlayer++;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !_qgIsLevel3)
        {
            _smokeParticle.SetActive(false);
            _doorEnter.SetActive(false);
            _doorExit.SetActive(true);
            _nbPlayer--;
            _isDecompression = false;
        }
    }
    public void QGLevel3()
    {
        _qgIsLevel3 = true;
        _doorEnter.SetActive(false);
        _doorExit.SetActive(false);
    }
    public void OpenDoor(Transform camera)
    {
        if (Physics.Raycast(camera.position, camera.forward, out RaycastHit hit, PC.Instance.GetDistRayCast()))
        {
            if (hit.collider.CompareTag("Decompression") && _nbPlayer >= 1 && !_isDecompression)
            {
                _isDecompression = true;
                _doorEnter.SetActive(true);
                StartCoroutine(StartParticle(1));
            }
            else if (hit.collider.CompareTag("DecompressionMountain") && _nbPlayer >=1 && !_isDecompression)// && Heure <= 20h)
            {
                _isDecompression = true;
                _doorEnter.SetActive(true);
                StartCoroutine(StartParticle(1));
            } 
        }
    }
    private IEnumerator StartParticle(float second)
    {
        yield return new WaitForSeconds(second);
        _soundDecompression.Play();
        _smokeParticle.SetActive(true);
        yield return new WaitForSeconds(_soundDecompression.clip.length - 1);
        _doorExit.SetActive(false);
    }
}
