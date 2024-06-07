using Mirror;
using System.Collections;
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
        if(isOwned)
        {
            Debug.Log("PopUpQuestAchieve called with text: " + text);
            popup.SetActive(true);
            popupAnimator.SetBool("OpenNotification", true);
            titleQuestCompleted.text = text;
            notificationSound.Play();
            StartCoroutine(Tempcoroutine());
        }
        
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log("Client started and handler registered");
        NetworkClient.RegisterHandler<QuestNotificationMessage>(OnClientReceiveMessage);
    }

    void OnClientReceiveMessage(QuestNotificationMessage msg)
    {
        Debug.Log("Client received message: " + msg.title);
        PopUpQuestAchieve(msg.title);
    }

    IEnumerator Tempcoroutine()
    {
        yield return new WaitForSeconds(3.5f);
        popupAnimator.SetBool("OpenNotification", false);
    }
}
