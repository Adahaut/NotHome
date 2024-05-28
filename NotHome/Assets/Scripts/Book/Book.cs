using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Book : MonoBehaviour
{
    [Header("mobs descriptions")]
    [SerializeField] private List<Sprite> _mobsSprites = new List<Sprite>();
    [SerializeField] private List<string> _mobsNames = new List<string>();
    [SerializeField] private List<string> _mobsDesciption = new List<string>();
    public List<bool> _mobDiscovered = new List<bool>();

    private Image _bookMobImage;
    private TextMeshProUGUI _bookMobname;
    private TextMeshProUGUI _bookMobDescription;

    private int _actualIndex;
    private int _maxIndex;

    private void Awake()
    {
        _bookMobImage = transform.GetChild(2).GetComponent<Image>();
        _bookMobname = transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        _bookMobDescription = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        _actualIndex = 0;
        _maxIndex = _mobsSprites.Count;
        for (int i = 0; i < _mobsSprites.Count; i++)
        {
            _mobDiscovered.Add(false);
        }
        SetTextsAndImage(0);
        gameObject.SetActive(false);
    }

    private void SetTextsAndImage(int _index)
    {
        _bookMobImage.sprite = _mobsSprites[_index];
        _bookMobname.text = _mobsNames[_index];
        _bookMobDescription.text = _mobsDesciption[_index];
        if (_mobDiscovered[_index])
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
