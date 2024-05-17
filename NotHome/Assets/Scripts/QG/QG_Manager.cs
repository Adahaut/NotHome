using TMPro;
using UnityEngine;

public class QG_Manager : MonoBehaviour
{
    [SerializeField] private GameObject _camera;
    [SerializeField] private float _distRayCast;
    [SerializeField] private string _text;
    
    private bool _canOpen;
    [HideInInspector] public bool _isOpen;

    [HideInInspector] public GameObject _gameObjectUi;
    public TextMeshProUGUI _textUi;

    public PlayerController _playerController;
    public static QG_Manager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, _distRayCast))
        {
            if (hit.collider.gameObject.GetComponent<BuildInterractable>() != null)
            {
                _gameObjectUi = hit.collider.gameObject;
                _textUi.text = _text;
                _canOpen = true;
            }
            else
            {
                _textUi.text = "";
                _canOpen = false;
            }
        }
        else
        {
            _canOpen = false;
            _textUi.text = "";
        }
    }
    public void OpenUi()
    {
        if (_canOpen)
        {
            _isOpen = true;
            _gameObjectUi.GetComponent<BuildInterractable>().OpenUiGameObject();
        }
    }
}
