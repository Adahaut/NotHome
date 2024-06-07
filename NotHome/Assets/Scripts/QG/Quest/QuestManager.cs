using Mirror;
using System.Collections.Generic;
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
                Debug.Log("Sending quest notification: " + _listQuests[currentQuest]._title);
                NetworkServer.SendToAll(msg);
            }
            else
            {
                Debug.LogWarning("Attempted to send quest notification from non-server instance.");
            }

            currentQuest++;
        }
    }

    public void SetQuestUpLevel2()
    {
        _questUpLevel2 += 1;
        if (_questUpLevel2 >= 4)
            QuestComplete(7);
    }

    public void SetQuestUpLevel3()
    {
        _questUpLevel2 += 1;
        if (_questUpLevel2 >= 4)
            QuestComplete(11);
    }

    public void QuestComplete(int index)
    {
        QuestStruct temp = _listQuests[index];
        temp._isComplet = true;

        _listQuests[index] = temp;
        NextQuest();
        //_objectif.color = Color.green;
    }
}
