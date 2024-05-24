using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QG_Manager : MonoBehaviour
{
    public Transform _camera;
    [SerializeField] private float _distRayCast;
    [SerializeField] private string _text;
    
    private bool _canOpen;
    [HideInInspector] public bool _isOpen;

    [HideInInspector] public GameObject _gameObjectUi;
    public TextMeshProUGUI _textUi;

    public PC _playerController;
    public static QG_Manager Instance;
    [Header("HealthBarQG")]
    [SerializeField] private Slider _healthBarQG;
    [SerializeField] private TextMeshProUGUI _textHp;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        _textHp.text = _healthBarQG.value.ToString() + " / " + _healthBarQG.maxValue.ToString();
    }

    private void Update()
    {
        if (Physics.Raycast(_camera.position, _camera.forward, out RaycastHit hit, _distRayCast))
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
    public void SetHealthBar(float number)
    {
        _healthBarQG.value += number;
        _textHp.text = _healthBarQG.value.ToString() + " / " + _healthBarQG.maxValue.ToString();
        if (_healthBarQG.value <= 0)
        {
            Debug.Log("QG is dead");
        }
    }
    public void SetMaxHealthBar(float number)
    {
        float maxValue = _healthBarQG.maxValue;
        _healthBarQG.maxValue *= number;
        SetHealthBar(_healthBarQG.maxValue - maxValue);
    }
}
