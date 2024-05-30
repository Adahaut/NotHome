using Mirror;
using UnityEngine;

[System.Serializable]
public class SeedObject : NetworkBehaviour
{
    public Seed seedStruct = new Seed();

    //public IEnumerator GrowPlant()
    //{
    //    yield return new WaitForSeconds(GT);
    //}

    public void StartGrow(Transform position, int index)
    {
        //this._index = index;
        //transform.position = position.position;
        // Ajoutez ici votre logique de croissance
    }

    
}
