using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUIPopup : MonoBehaviour
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
        print("caca");
    }

    public static void AllPlayerPopup(string text)
    {
        foreach (var playerUIPopup in FindObjectsOfType<PlayerUIPopup>())
        {
            playerUIPopup.PopUpQuestAchieve(text);
        }
    }

    IEnumerator Tempcoroutine()
    {
        yield return new WaitForSeconds(3f);
        popup.SetActive(false);
    }
}
