using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCinematic : MonoBehaviour
{
    [SerializeField] private GameObject _spaceSkybox;
    [SerializeField] private GameObject _meteorit;

    public void ActiveSpaceSkybox()
    {
        _spaceSkybox.SetActive(true);
    }

    public void ActiveMeteorit()
    {
        _meteorit.SetActive(true);
    }
}
