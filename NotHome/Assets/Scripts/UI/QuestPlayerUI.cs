using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestPlayerUI : NetworkBehaviour
{
    public GameObject _uiQuest;
    public GameObject _questButton;

    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _lore;
    [SerializeField] private TMP_Text _objectif;

    

    QuestManager _questManager;
    public static QuestPlayerUI Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        _questManager = QuestManager.Instance;

        //Enable quest & disable map
        _uiQuest.SetActive(true);
        //Change colors of buttons
    }
    public void OpenQuest()
    {
        OnEnable();
    }

    private void Update()
    {
        SetTextQuest();
    }

    private void SetTextQuest()
    {
        int currentIndex = _questManager.currentQuest;
        _title.text = _questManager._listQuests[currentIndex]._title;
        _objectif.text = _questManager._listQuests[currentIndex]._objectif;
        _lore.text = _questManager._listQuests[currentIndex]._lore;
    }

    [Command]
    public void NextQuest()
    {
        if (isOwned)
        {
            _questManager.NextQuest();
        }
    }

    
}
