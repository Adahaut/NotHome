using Mirror;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class SlenderMan : MonoBehaviour
{
    [SerializeField] private float _checkRadius = 10f;
    private NavMeshAgent _agent;
    private Transform _thisTransform;
    private GameObject[] _players;
    private GameObject _closestPlayer;
    private bool _isVisibleByAnyPlayer;
    [SerializeField] private float _killingDistance;
    public Animator _animator;
    [SerializeField] private AudioSource[] _audioSources;

    private void Start()
    {
        _players = GameObject.FindGameObjectsWithTag("Player");
        _agent = GetComponent<NavMeshAgent>();
        _thisTransform = transform;
        _animator = GetComponentInChildren<Animator>();

        //check for the closest player and visibility
        StartCoroutine(UpdateClosestPlayerAndVisibility());
    }

    private void Update()
    {
        if (_closestPlayer == null)
        {
            _agent.enabled = false;
            return;
        }

        if (_isVisibleByAnyPlayer)
        {
            _animator.speed = 0f;
            _agent.enabled = false;
        }
        else
        {
            _animator.speed = 1f;
            _agent.enabled = true;
            _agent.SetDestination(_closestPlayer.transform.position);
        }

    }

    private IEnumerator UpdateClosestPlayerAndVisibility()
    {
        while (true)
        {
            _closestPlayer = GetClosestPlayer();
            _isVisibleByAnyPlayer = _players.Any(player => IsEnemyVisibleByPlayer(player));

            //Update the check every 0.5 seconds
            yield return new WaitForSeconds(0.5f);
        }
    }

    private GameObject GetClosestPlayer()
    {
        GameObject closestPlayer = null;
        float closestDistance = Mathf.Infinity;

        foreach (var player in _players)
        {
            float _distance = Vector3.Distance(_thisTransform.position, player.transform.position);
            if (_distance < closestDistance)
            {
                closestPlayer = player;
                closestDistance = _distance;
            }

            if(_distance < _killingDistance)
            {
                //closestPlayer.GetComponent<LifeManager>().TakeDamage(5000);
                StartCoroutine(Attack(closestPlayer));
            }
        }

        return closestPlayer;
    }

    private bool IsEnemyVisibleByPlayer(GameObject player)
    {
        Camera playerCam = player.GetComponentInChildren<Camera>();

        // Check if the enemy is in the player's camera frustum
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerCam);
        if (!GeometryUtility.TestPlanesAABB(planes, GetComponent<Collider>().bounds))
        {
            return false;
        }

        // Check if there is a direct line of sight
        Vector3 directionToEnemy = _thisTransform.position - playerCam.transform.position;
        if (Physics.Raycast(playerCam.transform.position, directionToEnemy, out RaycastHit hit))
        {
            if (hit.transform.gameObject == gameObject)
            {
                return true;
            }
        }

        return false;
    }

    private IEnumerator Attack(GameObject closestPlayer)
    {
        _animator.SetBool("IsAttacking", true);
        _audioSources[2].Play();
        yield return new WaitForSeconds(1.15f);
        var playerLifeManager = closestPlayer.GetComponent<LifeManager>();
        if (playerLifeManager != null)
        {
            DealDamage(playerLifeManager);
        }
    }

    [Server]
    private void DealDamage(LifeManager playerLifeManager)
    {
        playerLifeManager.TakeDamage(5000);
    }

}
