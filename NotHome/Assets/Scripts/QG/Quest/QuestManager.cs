using Mirror;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : NetworkBehaviour
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


    public List<QuestStruct> _listToFillInInspector = new List<QuestStruct>();
    public SyncList<QuestStruct> _listQuests = new SyncList<QuestStruct>();
    [SyncVar] public int currentQuest;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        
        foreach(QuestStruct s in _listToFillInInspector)
        {
            _listQuests.Add(s);
        }
        //SetTextQuest();
    }

    private void Update()
    {
        print(_listQuests.Count);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
    }

    public void SetQuestUpLevel2()
    {
        _questUpLevel2 += 1;
        if (_questUpLevel2 >= 4)
            ColorText(7);
    }
    public void SetQuestUpLevel3()
    {
        _questUpLevel2 += 1;
        if (_questUpLevel2 >= 4)
            ColorText(11);
    }
    private void SetTextQuest()
    {
        //_title.text = _actualQuest._title;
        //_objectif.text = _actualQuest._objectif;
        //_lore.text = _actualQuest._lore;
    }
    public void ColorText(int index)
    {
        _listQuest[index]._isComplet = true;
        _objectif.color = Color.green;
    }
    public void NextQuest()
    {
        if (_actualQuest._nextQuest != null && _actualQuest._isComplet)
        {
            _actualQuest = _actualQuest._nextQuest;
            if (_actualQuest._isComplet )
                ColorText(0);
            else
                _objectif.color= Color.white;
            SetTextQuest();
        }
    }
}
