using Mirror.Examples.Chat;
using TMPro;
using UnityEngine;

public class QG_Manager : MonoBehaviour
{
    public GameObject _camera;
    [SerializeField] private float _distRayCast;
    [SerializeField] private string _text;
    
    private bool _canOpen;
    [HideInInspector] public bool _isOpen;

    [HideInInspector] public GameObject _gameObjectUi;
    public TextMeshProUGUI _textUi;

    public PC _playerController;
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
    public void OpenUi(PC _newPlayerController)
    {
        _playerController = _newPlayerController;
        if (_canOpen)
        {
            _isOpen = true;
            _gameObjectUi.GetComponent<BuildInterractable>().OpenUiGameObject(_playerController);
        }
    }
}
