using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerFieldSlot : MonoBehaviour
{
    public List<Transform> _listSlots = new();
    public List<TMP_Text> _listTexts = new();

    private void FixedUpdate()
    {
        //UpdateText();
    }

    //public void UpdateText()
    //{
    //    for (int i = 0; i < FieldManager.Instance._seedTimers.Count; i++)
    //    {
    //        //int seedTime = (int)FieldManager.Instance._seedTimers[i].currentTimer;
    //        //_listTexts[i].text = (seedTime / 60).ToString("00") + ":" + (seedTime % 60).ToString("00");
    //    }
    //}

    
}
