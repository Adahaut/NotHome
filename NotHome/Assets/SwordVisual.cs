using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordVisual : MonoBehaviour
{
    [SerializeField] private List<GameObject> _listSword = new();
    [SerializeField] private List<GameObject> _listSwordMesh = new();

    public void NextSword(int index)
    {
        for (int i = 0; i < _listSword.Count; i++)
        {
            _listSword[i].SetActive(false);
            _listSwordMesh[i].SetActive(false);
        }
        _listSword[index].SetActive(true);
        _listSwordMesh[index].SetActive(true);
    }
}
