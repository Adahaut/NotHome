using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : NetworkBehaviour
{
    public int _id;
    public string _name;
    public float _growingTime;
    public Sprite _img;
    [HideInInspector] public int _index;

    [HideInInspector] public bool _isPlanted;

    public IEnumerator GrowPlant()
    {
        yield return new WaitForSeconds(_growingTime);
    }

    public void StartGrow(Transform position, int index)
    {
        this._index = index;
        //StartCoroutine(GrowPlant());
    }
}
