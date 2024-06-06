using Mirror;
using System.Collections;
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

    [Header("POPUP")]
    [SerializeField] private GameObject popup;
    [SerializeField] private Animator popupAnimator;
    [SerializeField] private TMP_Text titleQuestCompleted;

    QuestManager _questManager;

    private void OnEnable()
    {
        _questManager = QuestManager.Instance;

        //Enable quest & disable map
        _uiQuest.SetActive(true);
        MapManager.Instance._uiMap.SetActive(false);
        //Change colors of buttons
        _questButton.GetComponent<Image>().color = new Color(132f / 255f, 132f / 255f, 132f / 255f);
        MapManager.Instance._mapButton.GetComponent<Image>().color = Color.white;
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
            PopUpQuestAchieve();
            _questManager.NextQuest();
        }
    }

    [ClientRpc]
    public void PopUpQuestAchieve()
    {
        popup.SetActive(true);
        titleQuestCompleted.text = _questManager._listQuests[_questManager.currentQuest]._title;
        StartCoroutine(TEMPCoroutine());
    }

    IEnumerator TEMPCoroutine()
    {
        yield return new WaitForSeconds(2f);
        popup.SetActive(false);
    }
}
