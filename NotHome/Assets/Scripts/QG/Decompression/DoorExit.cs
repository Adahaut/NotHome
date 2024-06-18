using Mirror;
using System.Collections;
using UnityEngine;

public class DoorExit : NetworkBehaviour
{
    //[SerializeField] private GameObject _doorEnter;
    //[SerializeField] private GameObject _doorExit;
    private int _nbPlayer;
    public static DoorExit Instance;
    private bool _qgIsLevel3;
    private bool _isDecompression;
    [SerializeField] private string _nameZone;
    [SerializeField] private GameObject _smokeParticle;
    [SerializeField] private GameObject _alarmSAS;
    [SerializeField] private GameObject _light;
    [SerializeField] private AudioSource _soundDecompression;

    [SerializeField] private Animator enterDoorAnimator;
    [SerializeField] private Animator exitDoorAnimator;

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
            _nbPlayer -= 1;
            if(exitDoorAnimator.GetBool("Open"))
            {
                QuestManager.Instance.QuestComplete(0);
                QuestManager.Instance.SetZoneQuest(_nameZone);
            }
            _smokeParticle.SetActive(false);
            exitDoorAnimator.SetBool("Open", false);
            _isDecompression = false;
            if (_nbPlayer <= 0)
                enterDoorAnimator.SetBool("Open", false);
        }
    }
    public void QGLevel3()
    {
        _qgIsLevel3 = true;
    }

    public void OpenDoor(Transform camera, float distRayCast)
    {
        if (Physics.Raycast(camera.position, camera.forward, out RaycastHit hit, distRayCast))
        {
            if (hit.collider.CompareTag("Decompression")  && !_isDecompression)
            {
                hit.collider.transform.parent.GetComponentInChildren<DoorExit>().SetActiveObject();
            }
            else if (hit.collider.CompareTag("DecompressionMountain") && !_isDecompression) // && heure <= 20h
            {
                hit.collider.transform.parent.GetComponentInChildren<DoorExit>().SetActiveObject();
            }
            else if (hit.collider.CompareTag("DecompressionExit") && !_isDecompression)
            {
                hit.collider.transform.parent.GetComponentInChildren<DoorExit>().exitDoorAnimator.SetBool("Open", true);
                hit.collider.transform.parent.GetComponentInChildren<DoorExit>().enterDoorAnimator.SetBool("Open", false);
            }
        }
    }

    [Command(requiresAuthority = false)]
    private void SetActiveObject()
    {
        ActiveObejts();
    }

    [ClientRpc]
    void ActiveObejts()
    {
        if (!enterDoorAnimator.GetBool("Open") && !exitDoorAnimator.GetBool("Open") && !_isDecompression)
        {
            enterDoorAnimator.SetBool("Open", true);
        }
        else if (_nbPlayer >= 1 && !_isDecompression)
        {
            bool door = false;
            _light.SetActive(true);
            _alarmSAS.SetActive(true);
            _isDecompression = true;
            if (!enterDoorAnimator.GetBool("Open"))
                door = true;
            enterDoorAnimator.SetBool("Open", false);
            exitDoorAnimator.SetBool("Open", false);
            //_doorExit.SetActive(true);
            StartCoroutine(StartParticle(1, door));
        }
    }

    private IEnumerator StartParticle(float second, bool door)
    {
        yield return new WaitForSeconds(second);
        _soundDecompression.Play();
        _smokeParticle.SetActive(true);
        yield return new WaitForSeconds(_soundDecompression.clip.length - 1);
        exitDoorAnimator.SetBool("Open", door);
        //_doorExit.SetActive(!door);
        enterDoorAnimator.SetBool("Open", !door);
        
        //_doorEnter.SetActive(door);
        _alarmSAS.SetActive(false);
        _light.SetActive(false);
    }
}
