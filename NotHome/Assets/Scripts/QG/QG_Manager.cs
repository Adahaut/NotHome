using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QG_Manager : MonoBehaviour
{
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
