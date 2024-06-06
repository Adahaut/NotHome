using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUIPopup : NetworkBehaviour
{
    [Header("POPUP")]
    [SerializeField] private GameObject popup;
    [SerializeField] private Animator popupAnimator;
    [SerializeField] private TMP_Text titleQuestCompleted;

    public void PopUpQuestAchieve(string text)
    {
        popup.SetActive(true);
        titleQuestCompleted.text = text;
        StartCoroutine(Tempcoroutine());
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        NetworkClient.RegisterHandler<QuestNotificationMessage>(OnClientReceiveMessage);
    }

    void OnClientReceiveMessage(QuestNotificationMessage msg)
    {
        PopUpQuestAchieve(msg.title);
        Debug.Log("caca");
    }

    IEnumerator Tempcoroutine()
    {
        yield return new WaitForSeconds(3f);
        popup.SetActive(false);
    }
}
