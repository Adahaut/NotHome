using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbite : MonoBehaviour
{
    public float _speed;
    public float _rota;

    // Update is called once per frame
    void Update()
    {
        _rota += _speed;
        transform.rotation = Quaternion.Euler(0, 0, _rota);
    }
}
