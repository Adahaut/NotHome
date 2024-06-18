using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farts : NetworkBehaviour
{
    [SerializeField] private List<AudioClip> _fartsSound = new List<AudioClip>();
    [SerializeField] private GameObject _fartParticle;

    private void Awake()
    {
        //_fartOrigine = transform.GetChild(6).GetComponent<AudioSource>();
    }

    public void PlayRandomFartSound()
    {
        int _randomFartIndex = Random.Range(0, _fartsSound.Count);
        if(isOwned)
        {
            CmdPlayFartSound(_randomFartIndex);
        }

        //_fartOrigine.Play();
        
    }

    [Command]
    void CmdPlayFartSound(int clipindex)
    {
        RpcPlayFartSound(clipindex);

        GameObject _newFartParticles = Instantiate(_fartParticle);
        NetworkServer.Spawn(_newFartParticles);
        _newFartParticles.transform.position = transform.position;
        _newFartParticles.transform.rotation = Quaternion.Inverse(transform.rotation);
        _newFartParticles.transform.SetParent(null);
        _newFartParticles.GetComponent<ParticleSystem>().Play();
        
        StartCoroutine(DestroyObjectOnServer(_newFartParticles));
    }

    [ClientRpc]
    void RpcPlayFartSound(int clipIndex)
    {
        AudioSource.PlayClipAtPoint(_fartsSound[clipIndex], this.transform.position, 0.5f);
    }

    IEnumerator DestroyObjectOnServer(GameObject obj)
    {
        yield return new WaitForSeconds(2f);
        NetworkServer.Destroy(obj); 
        Destroy(obj);
    }
}
