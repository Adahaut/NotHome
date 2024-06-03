using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private float _stamina;
    [SerializeField] private int _hunger;
    [SerializeField] private int _thirst;
    [SerializeField] private float _oxygene;

    [SerializeField] private float _maxStamina;
    [SerializeField] private int _maxHunger;
    [SerializeField] private int _maxThirst;
    [SerializeField] private float _maxOxygene;

    [SerializeField] private Slider _staminaSlider;
    [SerializeField] private Slider _hungerSlider;
    [SerializeField] private Slider _thirstSlider;
    [SerializeField] private Slider _oxygeneSlider;

    [SerializeField] private Collider _BaseZone;
    [SerializeField] private bool _isInBase;
    private bool _oxygeneRegainBegin;
    private bool _oxygeneFallBegin;

    public float Stamina { get { return _stamina; } set { _stamina = value; } }
    public int Hunger { get { return _hunger; } set { _hunger = value; } }
    public int Thirst { get { return _thirst; } set { _thirst = value; } }
    public float Oxygene { get { return _oxygene; } set { _oxygene = value; } }

    public float MaxStamina { get { return _maxStamina; } set { _maxStamina = value; } }
    public int MaxHunger { get { return _maxHunger; } set { _maxHunger = value; } }
    public int MaxThirst { get { return _maxThirst; } set { _maxThirst = value; } }
    public float MaxOxygene { get { return _maxOxygene; } set { _maxOxygene = value; } }


    private void Start()
    {
        SetMaxStamina(_maxStamina);
        SetMaxHunger();
        SetMaxThirst();
        SetMaxOxygene(_maxOxygene);

        StartCoroutine(HungerBarFall());
        StartCoroutine(ThirstBarFall());
        StartCoroutine(OxygeneBarFall());
    }

    public void SetStaminaBar()
    {
        _staminaSlider.value = _stamina;
    }

    public void SetHungerBar()
    {
        _hungerSlider.value = _hunger;
    }

    public void SetThirstBar()
    {
        _thirstSlider.value = _thirst;
    }

    public void SetOxygeneBar()
    {
        _oxygeneSlider.value = _oxygene;
    }

    public void SetMaxStamina(float maxStamina)
    {
        _stamina = maxStamina;
        _staminaSlider.maxValue = _stamina;
        _staminaSlider.value = _stamina;
    }
    public void SetMaxHunger()
    {
        _hunger = _maxHunger;
        _hungerSlider.maxValue = _hunger;
        _hungerSlider.value = _hunger;
    }
    public void SetMaxThirst()
    {
        _thirst = _maxThirst;
        _thirstSlider.maxValue = _thirst;
        _thirstSlider.value = _thirst;
    }
    public void SetMaxOxygene(float maxOxygene)
    {
        _oxygene = maxOxygene;
        _oxygeneSlider.maxValue = _oxygene;
        _oxygeneSlider.value = _oxygene;
    }

    private IEnumerator HungerBarFall()
    {
        while (_hunger > 0)
        {
            _hunger--;
            SetHungerBar();
            yield return new WaitForSeconds(12f);
        }
    }

    private IEnumerator ThirstBarFall()
    {
        while (_thirst > 0)
        {
            _thirst--;
            SetThirstBar();
            yield return new WaitForSeconds(4f);
        }
    }

    private IEnumerator OxygeneBarFall()
    {
        while (!_isInBase)
        {
            _oxygene--;
            SetOxygeneBar();
            yield return new WaitForSeconds(10f);
        }
    }

    private IEnumerator OxygeneBarRegain()
    {
        while (_oxygene < _maxOxygene && _isInBase)
        {
            _oxygene++;
            SetOxygeneBar();
            yield return new WaitForSeconds(1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "BaseZone" && !_oxygeneRegainBegin)
        {
            _oxygeneFallBegin = false;
            _oxygeneRegainBegin = true;
            _isInBase = true;
            StartCoroutine(OxygeneBarRegain());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "BaseZone" && !_oxygeneFallBegin)
        {
            _oxygeneFallBegin = true;
            _oxygeneRegainBegin = false;
            _isInBase = false;
            StartCoroutine(OxygeneBarFall());
        }
    }
}
