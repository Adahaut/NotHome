using Steamworks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FieldSlotsLists : MonoBehaviour
{
    public List<Transform> _listSlots = new();
    //public List<TMP_Text> _listTexts = new();

    public TMP_Text debug;

    private void FixedUpdate()
    {
        //UpdateText();
    }

    private void Update()
    {
        debug.text = "";
        for (int i = 0; i < NewFieldManager.instance._seedPlantedObjects.Count; i++)
        {
            if (NewFieldManager.instance._seedPlantedObjects[i] != null)
                debug.text += i + NewFieldManager.instance._seedPlantedObjects[i].seedStruct._name + "\n";
            else
                debug.text += i + "\n";

        }
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
