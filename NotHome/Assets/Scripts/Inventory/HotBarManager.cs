using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotBarManager : MonoBehaviour
{
    [SerializeField] private GameObject _hotBarSlot;
    [SerializeField] private GameObject _hotBar;

    [SerializeField] private int _hotBarNumber;

    [SerializeField] List<GameObject> _hotBarItems = new List<GameObject>();

    [SerializeField] private List<GameObject> _hotBarSlotList = new List<GameObject>();

    [SerializeField] private Color _hotBarSlotSelectedColor;
    [SerializeField] private Color _hotBarSlotUnselectedColor = Color.white;

    private PlayerAttack _playerAttack;

    public int _hotBarSlotIndex;
    private float _timeToHide;
    private bool _isOpen;
    private bool _isCoroutineRunning;

    private void Start()
    {
        InitializeHotBar();
        _playerAttack = GetComponentInParent<PlayerAttack>();
    }

    private void InitializeHotBar()
    {
        for (int i = 0; i < _hotBarNumber; i++)
        {
            AddHotBarSlot();
        }
        ChangeSlotsAlpha(0);
    }

    public void ResetTimer() { _timeToHide = 2f; }

    public bool IsOpen() {  return _isOpen; }

    public void UpdateSelectedHotBarSlot()
    {
        for (int i = 0; i < _hotBarSlotList.Count; ++i) 
        {
            _hotBarSlotList[i].GetComponent<Image>().color = /*_hotBarSlotUnselectedColor*/Color.white;
        }
        SetSelectedHotBarSlot();
        
        StartCoroutine(WaitAndDesactivateOtherWeapon());
    }

    private IEnumerator WaitAndDesactivateOtherWeapon()
    {
        if (_playerAttack._isAiming)
        {
            _playerAttack.StopAiming();
        }
        while (!_playerAttack._isAimingFinished || !_playerAttack._isRecoilFinished)
        {
            yield return new WaitForEndOfFrame();
        }

        UnActiveAll();
        _hotBarItems[_hotBarSlotIndex].SetActive(true);
        SetWeaponActive();
    }

    private void SetWeaponActive()
    {
        _playerAttack._isRangeWeaponEqupiped = false;
        _playerAttack._isMeleeWeaponEqupiped = false;
        if (_hotBarSlotIndex == 0)
        {
            _playerAttack._isRangeWeaponEqupiped = true;
        }
        else if (_hotBarSlotIndex == 1)
        {
            _playerAttack._isMeleeWeaponEqupiped = true;
        }
    }

    private void UnActiveAll()
    {
        for(int i = 0; i < _hotBarSlotList.Count; i++)
        {
            _hotBarItems[i].SetActive(false);
        }
    }

    private void SetSelectedHotBarSlot()
    {
        _hotBarSlotList[_hotBarSlotIndex].GetComponent<Image>().color = _hotBarSlotSelectedColor;
    }

    private void AddHotBarSlot()
    {
        GameObject _newHotBarSlot = Instantiate(_hotBarSlot);
        _newHotBarSlot.transform.SetParent(_hotBar.transform);
        _hotBarSlotList.Add(_newHotBarSlot);
    }

    private void ChangeSlotsAlpha(float _newAlpha)
    {
        for (int i = 0; i < _hotBarSlotList.Count; i++)
        {
            Color _slotColor = _hotBarSlotList[i].GetComponent<Image>().color;
            _slotColor.a = _newAlpha;
            _hotBarSlotList[i].GetComponent<Image>().color = _slotColor;
            _slotColor = _hotBarSlotList[i].transform.GetChild(0).GetComponent<Image>().color;
            _slotColor.a = _newAlpha;
            _hotBarSlotList[i].transform.GetChild(0).GetComponent<Image>().color = _slotColor;
        }
    }

    private void Update()
    {
        if (_timeToHide <= 0 && _isOpen)
        {
            StartFadeInFadeOut();
        }
        else if (_isOpen)
        {
            _timeToHide -= Time.deltaTime;
        }
    }

    public void StartFadeInFadeOut()
    {
        if (!_isCoroutineRunning)
            StartCoroutine(ShowAndHide(0.05f));
    }

    private IEnumerator ShowAndHide(float _interval)
    {
        _isCoroutineRunning = true;
        if (_isOpen)
        {
            _isOpen = false;
            float _aplha = 1f / 3f;
            for (int i = 1; i < 4; i++)
            {
                float _newAlpha = 1f - _aplha * i;
                ChangeSlotsAlpha(_newAlpha);
                yield return new WaitForSeconds(_interval);
            }
            _isCoroutineRunning = false;
            yield return null;
        }
        else
        {
            float _aplha = 1f / 8f;
            for (int i = 1; i < 9; i++)
            {
                float _newAlpha = _aplha * i;
                ChangeSlotsAlpha(_newAlpha);
                yield return new WaitForSeconds(_interval);
            }
            ResetTimer();
            _isOpen = true;
            _isCoroutineRunning = false;
            yield return null;
        }
    }
}
