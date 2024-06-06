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

    private void OnEnable()
    {
        _questManager = QuestManager.Instance;

        //Enable quest & disable map
        _uiQuest.SetActive(true);
        MapManager.Instance._uiMap.SetActive(false);
        //Change colors of buttons
        _questButton.GetComponent<Image>().color = new Color(132f / 255f, 132f / 255f, 132f / 255f);
        MapManager.Instance._mapButton.GetComponent<Image>().color = Color.white;

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
        int currentQuest = _questManager.currentQuest;
        if (currentQuest < _questManager._listQuests.Count - 1 && _questManager._listQuests[currentQuest]._isComplet)
        {
            _questManager.currentQuest++;
            SetTextQuest();
        }



        //if (_actualQuest._nextQuest != null && _actualQuest._isComplet)
        //{
        //    _actualQuest = _actualQuest._nextQuest;
        //    if (_actualQuest._isComplet)
        //        ColorText(0);
        //    else
        //        _objectif.color = Color.white;
        //    SetTextQuest();
        //}
    }

    //public void ColorText(int index)
    //{
    //    _listQuest[index]._isComplet = true;
    //    _objectif.color = Color.green;
    //}
}
