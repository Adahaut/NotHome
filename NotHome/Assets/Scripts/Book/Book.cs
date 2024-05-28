using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Book : MonoBehaviour
{
    [Header("mobs descriptions")]
    [SerializeField] private List<BookDesciption> _mobsDescriptions = new List<BookDesciption>();

    private Image _bookMobImage;
    private TextMeshProUGUI _bookMobDescription;
    private TextMeshProUGUI _bookMobDanger;

    private int _actualIndex;
    private int _maxIndex;

    private void Awake()
    {
        _bookMobImage = transform.GetChild(2).GetComponent<Image>();
        _bookMobDescription = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        _bookMobDanger = transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        _actualIndex = 0;
        _maxIndex = _mobsDescriptions.Count;
        SetTextsAndImage(0);
        gameObject.SetActive(false);
    }

    private void SetTextsAndImage(int _index)
    {
        _bookMobImage.sprite = _mobsDescriptions[_index]._sprite;
        _bookMobDescription.text = _mobsDescriptions[_index]._description;
        _bookMobDanger.text = "Danger : " + _mobsDescriptions[_index]._dangerLevel;
        if (_mobsDescriptions[_index]._isDiscovered)
        {
            _bookMobImage.color = Color.white;
        }
        else
        {
            _bookMobImage.color = Color.black;
        }
    }

    public void NextPage()
    {
        if (_actualIndex == _maxIndex)
            return;

        _actualIndex++;
        SetTextsAndImage(_actualIndex);
    }

    public void PreviousPage()
    {
        if (_actualIndex == 0)
            return;

        _actualIndex--;
        SetTextsAndImage(_actualIndex);
    }


}
