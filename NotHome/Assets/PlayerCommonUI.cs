using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommonUI : MonoBehaviour
{
    [SerializeField] private GameObject FieldPanel;

    public void CloseFieldPanel()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject p in players)
        {
            p.GetComponentInChildren<PlayerFieldUI>().DisablePanel();
        }
    }
}
