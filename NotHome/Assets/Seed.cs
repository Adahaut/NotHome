using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : NetworkBehaviour
{
    [SyncVar]
    public int seedId;

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

    public void StartGrow(Transform parent, int index)
    {
        this._index = index;
        transform.SetParent(parent);
        // Ajoutez ici votre logique de croissance
    }
}
