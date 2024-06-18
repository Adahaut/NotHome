using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
            CmdPlayFart(_randomFartIndex, spawnPoint.position);
        }
    }

    [Command]
    void CmdPlayFart(int clipindex, Vector3 spawnPosition)
    {
        RpcPlayFartSound(clipindex);

        GameObject particleInstance = Instantiate(_fartParticle, spawnPosition, Quaternion.identity);

        //GameObject _newFartParticles = Instantiate(_fartParticle, spawn.position, spawn.rotation);
        NetworkServer.Spawn(particleInstance);
        //_newFartParticles.GetComponent<ParticleSystem>().Play();

        Destroy(particleInstance, 2f);

    }

    [ClientRpc]
    void RpcPlayFartSound(int clipIndex)
    {
        AudioSource.PlayClipAtPoint(_fartsSound[clipIndex], this.transform.position, 0.25f);
    }
}
