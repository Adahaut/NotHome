using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SoundWalking : NetworkBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private float _distRayCast;
    [SerializeField] private List<AudioClip> _audioWalking = new();
    private Transform _transform;
    private CharacterController _characterController;
    [HideInInspector] public bool _isMoving;
    private int _indexSound = -1;
    [HideInInspector] public bool _isRunning;

    public static SoundWalking Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        _characterController = GetComponentInParent<CharacterController>();
        _transform = GetComponent<Transform>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isOwned)
            return;

        _isMoving = _characterController.velocity.magnitude > 0.1f; // Threshold for movement
        _isRunning = _isMoving && Input.GetKey(KeyCode.LeftShift); // Running if moving and holding Shift

        if (_characterController.isGrounded && _isMoving)
        {
            if (Physics.Raycast(_transform.position, _transform.TransformDirection(Vector3.down), out RaycastHit hit, _distRayCast))
            {
                Terrain terrain = hit.collider.GetComponent<Terrain>();

                if (terrain != null)
                {
                    TerrainData terrainData = terrain.terrainData;
                    Vector3 terrainPos = terrain.transform.position;
                    Vector3 hitPos = hit.point;

                    int mapX = Mathf.RoundToInt((hitPos.x - terrainPos.x) / terrainData.size.x * terrainData.alphamapWidth);
                    int mapZ = Mathf.RoundToInt((hitPos.z - terrainPos.z) / terrainData.size.z * terrainData.alphamapHeight);
                    float[,,] alphaMap = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);

                    for (int i = 0; i < terrainData.alphamapLayers; i++)
                    {
                        if (alphaMap[0, 0, i] > 0.5f) // threshold to determine dominant texture
                        {
                            CmdSetSound(i);
                            break;
                        }
                    }
                }
                else
                {
                    Debug.Log("Not a terrain");
                }
            }
        }
        else
        {
            CmdStopSound();
        }
    }

    [Command]
    private void CmdSetSound(int textureIndex)
    {
        RpcSetSound(textureIndex, _isRunning);
    }

    [ClientRpc]
    private void RpcSetSound(int index, bool isRunning)
    {
        SetSound(index, isRunning);
    }

    [Command]
    public void CmdStopSound()
    {
        RpcStopSound();
    }

    [ClientRpc]
    public void RpcStopSound()
    {
        StopSound();
    }

    private void SetSound(int index, bool isRunning)
    {
        int soundIndex = isRunning ? index + 1 : index; // If running, use next sound in list
        if (_audioSource.clip != _audioWalking[soundIndex])
        {
            _indexSound = soundIndex;
            _audioSource.clip = _audioWalking[soundIndex];
            StartSound();
        }
    }

    public void StartSound()
    {
        if (!_audioSource.isPlaying && _characterController.isGrounded)
            _audioSource.Play();
    }

    public void StopSound()
    {
        if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
            _audioSource.clip = null;
        }
    }
}