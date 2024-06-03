using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plant : MonoBehaviour
{
    public Sprite _img;
    public string _name;
    public float _timer;
    public int _index;
    public bool _isUsed = false;
    public Plant(string name, Sprite s) 
    {
        _name = name;
        _img = s;
    }


}
