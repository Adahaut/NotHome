using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Book : MonoBehaviour
{
    [Header("descriptions")]
    private List<List<BookDesciption>> _bookDescriptions = new List<List<BookDesciption>>();
    [SerializeField] private List<BookDesciption> _bookMobsDescriptions = new List<BookDesciption>();
    [SerializeField] private List<BookDesciption> _bookMapsDescriptions = new List<BookDesciption>();
    [SerializeField] private List<BookDesciption> _bookFoodsDescriptions = new List<BookDesciption>();

    private List<Transform> _buttons = new List<Transform>();
    private float _buttonOriginalPosY;

    private BookSection Mobs;
    //private BookSection Foods;
    private BookSection Maps;

    List<BookSection> _sections = new List<BookSection>();

    private List<GameObject> _sectionsGameObject = new List<GameObject>();

    private int _actualIndex;
    private int _maxIndex;
    private int _actualBookSection;

    struct BookSection
    {
        public Image _image;
        public TextMeshProUGUI _description;
        public TextMeshProUGUI _other;
    }

    private BookSection CreationSection(int _childIndex)
    {
        BookSection _newSection = new BookSection();
        _newSection._image = transform.GetChild(_childIndex).GetChild(1).GetComponent<Image>();
        _newSection._description = transform.GetChild(_childIndex).GetChild(2).GetComponent<TextMeshProUGUI>();
        _newSection._other = transform.GetChild(_childIndex).GetChild(3).GetComponent<TextMeshProUGUI>();

        return _newSection;
    }

    private void Awake()
    {
        Mobs = CreationSection(6);
        Maps = CreationSection(7);
        //Foods = CreationSection(6);

        _sections.Add(Mobs);
        _sections.Add(Maps);
        //_sections.Add(Foods);

        _actualIndex = 0;
        _maxIndex = _bookDescriptions.Count - 1;
        _actualBookSection = 0;

        _sectionsGameObject.Add(transform.GetChild(6).gameObject);
        _sectionsGameObject.Add(transform.GetChild(7).gameObject);
        //_sectionsGameObject.Add(transform.GetChild(6).gameObject);

        _bookDescriptions.Add(_bookMobsDescriptions);
        _bookDescriptions.Add(_bookMapsDescriptions);
        //_bookDescriptions.Add(_bookFoodsDescriptions);

        _buttons.Add(transform.GetChild(0));
        _buttons.Add(transform.GetChild(1));
        //_buttons.Add(transform.GetChild(2));
        _buttonOriginalPosY = _buttons[0].transform.position.y;
        ChangeSection(0);
    }


    private void UndactiveAll()
    {
        for(int i = 0; i < _bookDescriptions.Count; i++)
        {
            _sectionsGameObject[i].SetActive(false);
            print(_sectionsGameObject[i]);
            _buttons[i].position = new Vector2(_buttons[i].position.x, _buttonOriginalPosY);
        }
    }

    public void ChangeSection(int _index)
    {
        UndactiveAll();
        _buttons[_index].position = new Vector2(_buttons[_index].position.x, _buttons[_index].position.y + 10);
        _actualBookSection = _index;
        _sectionsGameObject[_index].SetActive(true);
        SetTextsAndImage(0);
    }

    private void SetTextsAndImage(int _index)
    {
        _sections[_actualBookSection]._image.sprite = _bookDescriptions[_actualBookSection][_index]._sprite;
        _sections[_actualBookSection]._description.text = _bookDescriptions[_actualBookSection][_index]._description;
        _sections[_actualBookSection]._other.text = _bookDescriptions[_actualBookSection][_index]._other;
        if (_bookDescriptions[_actualBookSection][_index]._isDiscovered)
        {
            _sections[_actualBookSection]._image.color = Color.white;
        }
        else
        {
            _sections[_actualBookSection]._image.color = Color.black;
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
