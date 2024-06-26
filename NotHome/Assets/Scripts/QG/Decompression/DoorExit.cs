using Mirror;
using System.Collections;
using UnityEngine;

public class DoorExit : NetworkBehaviour
{
    //[SerializeField] private GameObject _doorEnter;
    //[SerializeField] private GameObject _doorExit;
    private int _nbPlayer;
    public static DoorExit Instance;
    private bool _isDecompression;
    private bool _isGoingOut = true;
    [SerializeField] private string _nameZone;
    [SerializeField] private GameObject _smokeParticle;
    [SerializeField] private GameObject _alarmSAS;
    [SerializeField] private GameObject _light;
    [SerializeField] private AudioSource _soundDecompression;
    public AudioClip sasClip;
    private PeopleInBaseChecker _checker;

    [SerializeField] private ItemSpawnerManager _spawnerManager;
    private EnemiesSpawner _enemiesSpawner;
    [SerializeField] private Animator enterDoorAnimator;
    [SerializeField] private Animator exitDoorAnimator;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if(_spawnerManager == null)
            _spawnerManager = GameObject.Find("ItemsWaypoints").GetComponent<ItemSpawnerManager>();
        if(_enemiesSpawner == null)
            _enemiesSpawner = GameObject.Find("EnemiesSpawner").GetComponent<EnemiesSpawner>();
        if(_checker == null)
            _checker = GameObject.Find("BaseZone").GetComponent<PeopleInBaseChecker>();

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _nbPlayer += 1;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _nbPlayer -= 1;
            if(exitDoorAnimator.GetBool("Open"))
            {
                QuestManager.Instance.QuestComplete(0);
                QuestManager.Instance.SetZoneQuest(_nameZone);
            }
            _smokeParticle.SetActive(false);
            if (_nbPlayer <= 0)
            {
                _isDecompression = false;
                exitDoorAnimator.SetBool("Open", false);
                enterDoorAnimator.SetBool("Open", false);
            }  
        }
    }

    public void OpenDoor(Transform camera, float distRayCast, bool canOpenAllSAS)
    {
        if (Physics.Raycast(camera.position, camera.forward, out RaycastHit hit, distRayCast))
        {
            if (hit.collider.transform.parent && hit.collider.transform.parent.GetComponentInChildren<DoorExit>() != null)
            {
                if (hit.collider.transform.parent.GetComponentInChildren<DoorExit>()._nameZone != "Final" || canOpenAllSAS)
                {
                    if (hit.collider.CompareTag("Decompression") && !_isDecompression)
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
            else
                SpawnItemAndMobsByZone();
            enterDoorAnimator.SetBool("Open", false);
            exitDoorAnimator.SetBool("Open", false);
            //_doorExit.SetActive(true);
            StartCoroutine(StartParticle(1, door));
            //print(_checker);
            //print(_checker.Check());
        }
    }

    private void SpawnItemAndMobsByZone()
    {
        if (isServer && _checker.Check())
        {
            print("dedans");
            switch (_nameZone)
            {
                case "Desert":
                    SpawnItem(0, _spawnerManager._desertSpawn);
                    _spawnerManager._desertSpawn = true;
                    break;

                case "Mountain":
                    SpawnItem(1, _spawnerManager._mountainSpawn);
                    _spawnerManager._mountainSpawn = true;
                    break;

                case "Forest":
                    SpawnItem(2, _spawnerManager._forestSpawn);
                    _spawnerManager._forestSpawn = true;
                    break;

                case "Final":
                    _enemiesSpawner.SpawnMobOfZone(3);
                    break;

                default:
                    print("wrong zone name");
                    break;
            }
        }
    }

    private void SpawnItem(int _zone, bool _zoneSpawned)
    {
        if (_spawnerManager._canSpawn && !_zoneSpawned)
        {
            _spawnerManager.DestroyAll();
            _spawnerManager.DestroyAndSpawnItems(_zone);
            _enemiesSpawner.SpawnMobOfZone(_zone);
        }
        else if(!_zoneSpawned)
        {
            _spawnerManager.SpawnItems(_zone);
            _enemiesSpawner.SpawnMobOfZone(_zone);
        }
    }

    private IEnumerator StartParticle(float second, bool door)
    {
        yield return new WaitForSeconds(second);
        //_soundDecompression.Play();
        CmdPlaySound(transform.position);
        _smokeParticle.SetActive(true);
        yield return new WaitForSeconds(_soundDecompression.clip.length - 1);
        exitDoorAnimator.SetBool("Open", !door);
        //_doorExit.SetActive(!door);
        enterDoorAnimator.SetBool("Open", door);
        
        //_doorEnter.SetActive(door);
        _alarmSAS.SetActive(false);
        _light.SetActive(false);
    }

    [Command(requiresAuthority = false)]
    void CmdPlaySound(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(sasClip, position, 0.7f);
        RpcPlaySound(position);
    }

    [ClientRpc]
    void RpcPlaySound(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(sasClip, position, 0.7f);
    }
}
