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

                if (hit.collider.transform.parent.GetComponentInChildren<DoorExit>()._nameZone != "Final" /*|| canOpenAllSAS*/)
                {
                    if (hit.collider.CompareTag("Decompression") && !_isDecompression)
                    {
                        print("try 1");
                        hit.collider.transform.parent.GetComponentInChildren<DoorExit>().SetActiveObject();
                    }
                    else if (hit.collider.CompareTag("DecompressionMountain") && !_isDecompression) // && heure <= 20h
                    {
                        print("try 2");
                        hit.collider.transform.parent.GetComponentInChildren<DoorExit>().SetActiveObject();
                    }
                    else if (hit.collider.CompareTag("DecompressionExit") && !_isDecompression)
                    {
                        print("try 3");
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
            print("decompression");
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
            SpawnItemAndMobsByZone();
            print(_checker);
            print(_checker.Check());
        }
    }

    private void SpawnItemAndMobsByZone()
    {
        print("dedans spawn");
        if (isServer && _checker.Check())
        {
            print("serveur");
            switch (_nameZone)
            {
                case "Desert":
                    print("desert");
                    _spawnerManager.DestroyAndSpawnItems(0);
                    print("items fini");
                    _enemiesSpawner.SpawnMobOfZone(0);
                    print("fini");
                    break;

                case "Mountain":
                    print("mountain");
                    _spawnerManager.DestroyAndSpawnItems(1);
                    print("items fini");

                    _enemiesSpawner.SpawnMobOfZone(1);
                    print("fini");
                    break;

                case "Forest":
                    print("forest");
                    _spawnerManager.DestroyAndSpawnItems(2);
                    print("items fini");

                    _enemiesSpawner.SpawnMobOfZone(2);
                    print("fini");
                    break;

                case "Final":
                    print("desert");
                    _enemiesSpawner.SpawnMobOfZone(3);
                    print("fini");
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
            _spawnerManager.DestroyAndSpawnItems(_zone);
            _enemiesSpawner.SpawnMobOfZone(_zone);
            _zoneSpawned = true;
        }
        else if(!_zoneSpawned)
        {
            _spawnerManager.SpawnItems(_zone);
            _enemiesSpawner.SpawnMobOfZone(_zone);
            _zoneSpawned = true;
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
