using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedObject : NetworkBehaviour
{
    public int GT;

    SeedObject(int gT)
    {
        GT = gT;
    }

    public IEnumerator GrowPlant()
    {
        yield return new WaitForSeconds(GT);
    }

    public void StartGrow(Transform position, int index)
    {
        this._index = index;
        //transform.position = position.position;
        // Ajoutez ici votre logique de croissance
    }
}
