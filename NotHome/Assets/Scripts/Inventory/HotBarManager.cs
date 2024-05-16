using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotBarManager : MonoBehaviour
{
    [SerializeField] private GameObject _hotBarSlot;
    [SerializeField] private GameObject _hotBar;

    [SerializeField] private int _hotBarNumber;

    [SerializeField] private List<GameObject> _hotBarSlotList = new List<GameObject>();

    [SerializeField] private Color _hotBarSlotSelectedColor;
    [SerializeField] private Color _hotBarSlotUnselectedColor;

    private int _hotBarSlotIndex;
    private float _timeToHide;

    private void Start()
    {
        InitializeHotBar();
    }

    private void InitializeHotBar()
    {
        for (int i = 0; i < _hotBarNumber; i++)
        {
            AddHotBarSlot();
        }
        gameObject.SetActive(false);
    }

    public void UpdateSelectedHotBarSlot(int _indexAddition)
    {
        for (int i = 0; i < _hotBarSlotList.Count; ++i) 
        {
            _hotBarSlotList[i].GetComponent<Image>().color = _hotBarSlotUnselectedColor;
        }
        _hotBarSlotIndex += _indexAddition;
        if (_hotBarSlotIndex > 3)
        {
            _hotBarSlotIndex = 0;
        }
        else if (_hotBarSlotIndex < 0)
        {
            _hotBarSlotIndex = 3;
        }
        SetSelectedHotBarSlot();
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
}
