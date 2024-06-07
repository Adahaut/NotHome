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
    [SerializeField] private AudioSource notificationSound;

    public void PopUpQuestAchieve(string text)
    {
        popup.SetActive(true);
        popupAnimator.SetBool("OpenNotification", true);
        titleQuestCompleted.text = text;
        notificationSound.Play();
        StartCoroutine(Tempcoroutine());
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        NetworkClient.RegisterHandler<QuestNotificationMessage>(OnClientReceiveMessage);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<QuestNotificationMessage>(OnServerReceiveMessage);
    }

    void OnClientReceiveMessage(QuestNotificationMessage msg)
    {
        PopUpQuestAchieve(msg.title);
    }

    void OnServerReceiveMessage(NetworkConnection conn, QuestNotificationMessage msg)
    {
        NetworkServer.SendToAll(msg);
    }

    IEnumerator Tempcoroutine()
    {
        yield return new WaitForSeconds(3.5f);
        popupAnimator.SetBool("OpenNotification", false);
    }
}
