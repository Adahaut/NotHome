using System.Collections;
using UnityEngine;

public class PlayerDeathAndRespawn : MonoBehaviour
{
    private Transform _playerTransform;
    private PC _playerController;

    private Transform _playerRespawnPoint;

    private void Start()
    {
        _playerTransform = transform;
        _playerRespawnPoint = transform;
        _playerController = GetComponent<PC>();
    }

    public void PlayerDeath()
    {
        _playerTransform.rotation = Quaternion.Euler(0, 0, 90);
        _playerController.IsDead = true;
    }

    public void Respawn()
    {
        _playerTransform = _playerRespawnPoint;
        _playerController.IsDead = false;
    }


}
