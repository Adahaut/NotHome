using Mirror;
using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct QuestNotificationMessage : NetworkMessage
{
    public string title;
}

public class QuestManager : NetworkBehaviour
{
    public GameObject _uiQuest;
    public GameObject _questButton;
    public static QuestManager Instance;
    //public List<QuestScriptableObject> _listQuest = new();

    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _lore;
    [SerializeField] private TextMeshProUGUI _objectif;
    [SerializeField] private QuestScriptableObject _actualQuest;
    private int _questUpLevel2;
    private int _questUpLevel3;
    private int _counterSpider;
    private int _counterX;
    private int _counterMetal;


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
    }

    public void NextQuest()
    {
        if (currentQuest < _listQuests.Count - 1 && _listQuests[currentQuest]._isComplet)
        {
            QuestNotificationMessage msg = new QuestNotificationMessage { title = _listQuests[currentQuest]._title };
            if (isServer)
            {
                NetworkServer.SendToAll(msg);
            }
            currentQuest++;
            NextQuest();
        }
    }
    public void SetQuestSpider()
    {
        int indexQuest = 2;
        if (currentQuest == indexQuest)
        {
            _counterSpider++;
            if (_counterSpider >= 3)
            {
                QuestComplete(indexQuest);
            }
        }
    }
    public void SetQuestX()
    {
        int indexQuest = 6;
        if (currentQuest == indexQuest)
        {
            _counterSpider++;
            if (_counterX >= 10)
            {
                QuestComplete(indexQuest);
            }
        }
    }

    public void SetQuestUpLevel2()
    {
        _questUpLevel2 += 1;
        if (_questUpLevel2 >= 4)
        {
            QuestStruct temp = _listQuests[7];
            temp._isComplet = true;
            _listQuests[7] = temp;
            NextQuest();
        }
    }

    public void SetQuestUpLevel3()
    {
        _questUpLevel3 += 1;
        if (_questUpLevel3 >= 3)
        {
            QuestStruct temp = _listQuests[11];
            temp._isComplet = true;
            _listQuests[11] = temp;
            NextQuest();
        }
    }
    public void SetQuestMetal()
    {
        int indexQuest = 1;
        if (currentQuest == indexQuest)
        {
            _counterMetal++;
            if (_counterMetal >= 5)
                QuestComplete(indexQuest);
        }
    }
    public void SetZoneQuest(string zone)
    {
        switch (zone)
        {
            case "Forest":
                QuestComplete(4);
                break;
            case "Mountain":
                QuestComplete(9);
                break;
            default:
                Debug.Log("No zone name");
                break;

        }
    }

    public void QuestComplete(int index)
    {
        if (index == currentQuest)
        {
            QuestStruct temp = _listQuests[index];
            temp._isComplet = true;

            _listQuests[index] = temp;
            NextQuest();
            //_objectif.color = Color.green;
        }
    }
}
