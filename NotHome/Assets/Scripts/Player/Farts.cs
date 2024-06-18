using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farts : NetworkBehaviour
{
    [SerializeField] private List<AudioClip> _fartsSound = new List<AudioClip>();
    [SerializeField] private GameObject _fartParticle;

    [SerializeField] private Transform spawnPoint;

    public void PlayRandomFartSound()
    {
        int _randomFartIndex = Random.Range(0, _fartsSound.Count);
        if(isOwned)
        {
            CmdPlayFartSound(_randomFartIndex, transform, spawnPoint);
        }
    }

    [Command]
    void CmdPlayFartSound(int clipindex, Transform playerTransform, Transform spawn)
    {
        RpcPlayFartSound(clipindex);
        RpcPlayFartParticles(playerTransform, spawn);
        
    }

    [ClientRpc]
    void RpcPlayFartSound(int clipIndex)
    {
        AudioSource.PlayClipAtPoint(_fartsSound[clipIndex], this.transform.position, 0.5f);
    }

    [ClientRpc]
    void RpcPlayFartParticles(Transform playerTransform, Transform spawn)
    {
        GameObject _newFartParticles = Instantiate(_fartParticle, spawn.position, spawn.rotation);
        _newFartParticles.GetComponent<ParticleSystem>().Play();

        StartCoroutine(DestroyObjectOnServer(_newFartParticles));
    }

    IEnumerator DestroyObjectOnServer(GameObject obj)
    {
        yield return new WaitForSeconds(2f);
        Destroy(obj);
    }
}
