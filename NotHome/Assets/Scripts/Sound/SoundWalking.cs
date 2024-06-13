using System.Collections.Generic;
using UnityEngine;

public class SoundWalking : MonoBehaviour
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
        if (_characterController.isGrounded && _isMoving)
        {
            if (Physics.Raycast(_transform.position, _transform.TransformDirection(Vector3.down), out RaycastHit hit, _distRayCast))
            {
                switch (hit.collider.tag)
                {
                    case "Forest":
                        SoundZone.Instance.SetZone("Forest");
                        SetSound(0);
                        break;
                    case "Mountain":
                        SoundZone.Instance.SetZone("Mountain");
                        SetSound(2);
                        break;
                    case "Desert":
                        SoundZone.Instance.SetZone("Desert");
                        SetSound(4);
                        break;
                    default:
                        Debug.Log("No tag");
                        break;
                }
            }
        }
        else
        {
            StopSound();
        }
    }

    private void SetSound(int index)
    {
        if (_isRunning)
        {
            if (_audioSource.clip != _audioWalking[index + 1])
            {
                _indexSound = index + 1;
                _audioSource.clip = _audioWalking[index + 1];
                StartSound();
            }
        }
        else
        {
            if (_audioSource.clip != _audioWalking[index])
            {
                _indexSound = index;
                _audioSource.clip = _audioWalking[index];
                StartSound();
            }
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
