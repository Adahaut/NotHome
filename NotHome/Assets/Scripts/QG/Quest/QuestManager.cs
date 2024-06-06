using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("POPUP")]
    [SerializeField] private GameObject popup;
    [SerializeField] private Animator popupAnimator;
    [SerializeField] private TMP_Text titleQuestCompleted;

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

    //Call this function to set next quest
    public void NextQuest()
    {
        if (currentQuest < _listQuests.Count - 1 && _listQuests[currentQuest]._isComplet)
        {
            currentQuest++;
            PopUpQuestAchieve();
        }
    }
    public void PopUpQuestAchieve()
    {
        popup.SetActive(true);
        titleQuestCompleted.text = _listQuests[currentQuest]._title;
        StartCoroutine(TEMPCoroutine());
    }

    IEnumerator TEMPCoroutine()
    {
        yield return new WaitForSeconds(2f);
        popup.SetActive(false);
    }

    public void SetQuestUpLevel2()
    {
        //_questUpLevel2 += 1;
        //if (_questUpLevel2 >= 4)
        //    ColorText(7);
    }
    public void SetQuestUpLevel3()
    {
        //_questUpLevel2 += 1;
        //if (_questUpLevel2 >= 4)
        //    ColorText(11);
    }

    public void ColorText(int index)
    {
        //_listQuest[index]._isComplet = true;
        //_objectif.color = Color.green;
    }
}
