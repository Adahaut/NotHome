using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveHQCinematic : MonoBehaviour
{
    [SerializeField] private GameObject _hqObjects;
    
    public void ActiveHQ()
    {
        _hqObjects.SetActive(true);
    }
}