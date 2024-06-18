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
            CmdPlayFartSound(_randomFartIndex);
        }
    }

    [Command]
    void CmdPlayFartSound(int clipindex)
    {
        RpcPlayFartSound(clipindex, transform, spawnPoint);

        GameObject _newFartParticles = Instantiate(_fartParticle);
        NetworkServer.Spawn(_newFartParticles);
        _newFartParticles.transform.position = spawnPoint.position;
        _newFartParticles.transform.rotation = transform.rotation;
        _newFartParticles.GetComponent<ParticleSystem>().Play();

        StartCoroutine(DestroyObjectOnServer(_newFartParticles));
    }

    [ClientRpc]
    void RpcPlayFartSound(int clipIndex, Transform playerTransform, Transform spawn)
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
