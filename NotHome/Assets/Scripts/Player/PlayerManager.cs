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

    [SerializeField] public Image _staminaSlider;
    [SerializeField] public Image _hungerSlider;
    [SerializeField] public Image _thirstSlider;
    [SerializeField] public Image _oxygeneSlider;

    public GameObject _stamParent;

    [SerializeField] private Collider _BaseZone;
    [SerializeField] private bool _isInBase;
    private bool _oxygeneRegainBegin;
    private bool _oxygeneFallBegin;

    [SerializeField] private Transform _staminaEndPoint;
    [SerializeField] private Transform _staminaBeginPoint;

    public float Stamina { get { return _stamina; } set { _stamina = value; } }
    public int Hunger { get { return _hunger; } set { _hunger = value; } }
    public int Thirst { get { return _thirst; } set { _thirst = value; } }
    public float Oxygene { get { return _oxygene; } set { _oxygene = value; } }

    public float MaxStamina { get { return _maxStamina; } set { _maxStamina = value; } }
    public int MaxHunger { get { return _maxHunger; } set { _maxHunger = value; } }
    public int MaxThirst { get { return _maxThirst; } set { _maxThirst = value; } }
    public float MaxOxygene { get { return _maxOxygene; } set { _maxOxygene = value; } }

    float _initScale;
    private void Start()
    {
        _stamParent = _staminaSlider.transform.parent.gameObject;
        _stamParent.transform.position = _staminaEndPoint.position;
        _stamParent.SetActive(false);
        _initScale = _stamParent.transform.localScale.x;
        SetMaxStamina(_maxStamina);
        SetMaxHunger();
        SetMaxThirst();
        SetMaxOxygene(_maxOxygene);

        //StartCoroutine(HungerBarFall());
        //StartCoroutine(ThirstBarFall());
        StartCoroutine(OxygeneBarFall());
    }

    public void SetStaminaBar()
    {
        _staminaSlider.fillAmount = _stamina / MaxStamina;
    }

    public void SetHungerBar()
    {
        _hungerSlider.fillAmount = _hunger / _maxHunger;
    }

    public void SetThirstBar()
    {
        _thirstSlider.fillAmount = _thirst / _maxThirst;
    }

    public void SetOxygeneBar()
    {
        _oxygeneSlider.fillAmount = _oxygene / _maxOxygene;
    }

    public void SetMaxStamina(float maxStamina)
    {
        _stamina = maxStamina;
        _maxStamina = maxStamina;
        _staminaSlider.fillAmount = _maxStamina;
        if (_stamParent.active)
            StartStamina(false, 1f);
    }
    public void SetMaxHunger()
    {
        _hunger = _maxHunger;
        //_hungerSlider.fillAmount = _maxHunger;
    }
    public void SetMaxThirst()
    {
        _thirst = _maxThirst;
        //_thirstSlider.fillAmount = _maxThirst;
    }
    public void SetMaxOxygene(float maxOxygene)
    {
        _oxygene = maxOxygene;
        _maxOxygene = maxOxygene;
        _oxygeneSlider.fillAmount = _maxOxygene;
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

    private IEnumerator StaminaAppearDisapear(bool Reverse, float totalTime)
    {
        
        if (!Reverse)
        {
            yield return new WaitForSeconds(1f);
            float time = 0f;
            while(time/totalTime < 1) 
            {
                time += Time.deltaTime;
                Color color = _stamParent.GetComponent<Image>().color;
                color.a = 1 - time / totalTime;
                _stamParent.GetComponent<Image>().color = color;
                _staminaSlider.GetComponent<Image>().color = color;
                _stamParent.transform.position = Vector3.Lerp(_staminaBeginPoint.position, _staminaEndPoint.position, time / totalTime);
                _stamParent.transform.localScale = new Vector3(_initScale * (1 - (time / totalTime)), _initScale * (1 - (time / totalTime)),
                    _initScale * (1 - (time / totalTime)));
                yield return new WaitForEndOfFrame();
            }
            _stamParent.SetActive(false);
        }
        else if (Reverse)
        {
            _stamParent.SetActive(true);
            float time = 0f;
            while (time / totalTime < 1)
            {
                time += Time.deltaTime;
                Color color = _stamParent.GetComponent<Image>().color;
                color.a = time / totalTime;
                _stamParent.GetComponent<Image>().color = color;
                _staminaSlider.GetComponent<Image>().color = color;
                _stamParent.transform.position = Vector3.Lerp(_staminaEndPoint.position, _staminaBeginPoint.position, time / totalTime);
                _stamParent.transform.localScale = new Vector3(_initScale * (time / totalTime), _initScale * (time / totalTime),
                    _initScale * (time / totalTime));
                yield return new WaitForEndOfFrame();
            }
        }
    }

    public void StartStamina(bool Reverse, float totalTime)
    {
        StartCoroutine(StaminaAppearDisapear(Reverse, totalTime));
    }
}
