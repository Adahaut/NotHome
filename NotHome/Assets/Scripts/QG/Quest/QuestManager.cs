using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public GameObject _uiQuest;
    public GameObject _questButton;
    public static QuestManager Instance;
    public List<QuestScriptableObject> _listQuest = new();

    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _lore;
    [SerializeField] private TextMeshProUGUI _objectif;
    [SerializeField] private QuestScriptableObject _actualQuest;
    private int _questUpLevel2;
    private int _questUpLevel3;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        SetTextQuest();
    }
    public void SetQuestUpLevel2()
    {
        _questUpLevel2 += 1;
        if (_questUpLevel2 >= 4)
            _listQuest[7]._isComplet = true;
    }
    public void SetQuestUpLevel3()
    {
        _questUpLevel2 += 1;
        if (_questUpLevel2 >= 4)
            _listQuest[11]._isComplet = true;
    }
    public void OpenQuest()
    {
        _uiQuest.SetActive(true);
        MapManager.Instance._uiMap.SetActive(false);
        _questButton.GetComponent<Image>().color = new Color(132f / 255f, 132f / 255f, 132f / 255f);
        MapManager.Instance._mapButton.GetComponent<Image>().color = Color.white;
        SetTextQuest();
    }
    private void SetTextQuest()
    {
        _title.text = _actualQuest._title;
        _objectif.text = _actualQuest._objectif;
        _lore.text = _actualQuest._lore;
    }
    public void NextQuest()
    {
        if (_actualQuest._nextQuest != null && _actualQuest._isComplet)
        {
            _actualQuest = _actualQuest._nextQuest;
            SetTextQuest();
        }
    }
}
