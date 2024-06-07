using System.Collections;
using System.Linq;
using UnityEngine;

public class CheckEnemiesVision : MonoBehaviour
{
    [SerializeField] private float _checkInterval = 0.5f;
    private Camera _enemyCam;
    private GameObject[] _players;
    public GameObject _visiblePlayer { get; private set; }

    private void Start()
    {
        _enemyCam = GetComponentInChildren<Camera>();
        _players = GameObject.FindGameObjectsWithTag("Player");

        StartCoroutine(CheckVisibilityCoroutine());
    }

    private IEnumerator CheckVisibilityCoroutine()
    {
        while (true)
        {
            _visiblePlayer = GetFirstVisiblePlayer();
            Debug.Log(_visiblePlayer.name);
            yield return new WaitForSeconds(_checkInterval);
        }
    }

    private GameObject GetFirstVisiblePlayer()
    {
        foreach (var player in _players)
        {
            if (IsVisible(player))
            {
                return player;
            }
        }

        return null;
    }

    private bool IsVisible(GameObject target)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_enemyCam);
        if (!GeometryUtility.TestPlanesAABB(planes, target.GetComponent<Collider>().bounds))
        {
            return false;
        }

        // Check if there is a direct line of sight
        Vector3 directionToTarget = target.transform.position - _enemyCam.transform.position;
        if (Physics.Raycast(_enemyCam.transform.position, directionToTarget, out RaycastHit hit))
        {
            if (hit.transform.gameObject == target)
            {
                return true;
            }
        }

        return false;
    }
}
