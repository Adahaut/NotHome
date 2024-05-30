using System.Collections;
using UnityEngine;

public class DoorExitMountain : MonoBehaviour
{
    [SerializeField] private GameObject _doorEnter;
    [SerializeField] private GameObject _doorExit;
    private int _nbPlayer;
    public static DoorExitMountain Instance;
    private bool _qgIsLevel3;
    private bool _isDecompression;
    [SerializeField] private GameObject _smokeParticle;
    [SerializeField] private GameObject _alarmSAS;
    [SerializeField] private GameObject _light;
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
            _nbPlayer += 1;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !_qgIsLevel3)
        {
            _smokeParticle.SetActive(false);
            _doorExit.SetActive(true);
            _nbPlayer -= 1;
            _isDecompression = false;
            if (_nbPlayer <= 0)
                _doorEnter.SetActive(true);
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
            if (hit.collider.CompareTag("DecompressionMountain") && !_isDecompression)
            {
                if (_doorEnter.activeSelf && _doorExit.activeSelf)
                {
                    _doorEnter.SetActive(false);
                }
                else if (_nbPlayer >= 1)
                {
                    bool door = false;
                    _alarmSAS.SetActive(true);
                    _light.SetActive(true);
                    _isDecompression = true;
                    if (_doorExit.activeSelf)
                        door = true;
                    _doorEnter.SetActive(true);
                    _doorExit.SetActive(true);
                    StartCoroutine(StartParticle(1, door));
                }
            }
            else if (hit.collider.CompareTag("DecompressionExitMountain") && !_isDecompression)
            {
                _doorExit.SetActive(false);
            }
        }
    }
    private IEnumerator StartParticle(float second,bool door)
    {
        yield return new WaitForSeconds(second);
        _soundDecompression.Play();
        _smokeParticle.SetActive(true);
        yield return new WaitForSeconds(_soundDecompression.clip.length - 1);
        _doorExit.SetActive(!door);
        _doorEnter.SetActive(door);
        _alarmSAS.SetActive(false);
        _light.SetActive(false);
    }
}